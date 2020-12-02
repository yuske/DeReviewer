using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using dnlib.DotNet.Emit;

namespace DeReviewer.Analysis
{
    public class CallGraph
    {
        public Dictionary<MethodUniqueSignature, CallGraphNode> Nodes { get; } = new Dictionary<MethodUniqueSignature, CallGraphNode>();
        public Dictionary<MethodUniqueSignature, CallGraphNode> EntryNodes { get; } = new Dictionary<MethodUniqueSignature, CallGraphNode>();
        public Dictionary<MethodUniqueSignature, CallGraphNode> Roots { get; private set; } = new Dictionary<MethodUniqueSignature, CallGraphNode>();

        public bool IsEmpty => Nodes.Count == 0;

        public void RemoveDuplicatePaths()
        {
            foreach (var node in Nodes.Values)
            {
                RemoveDuplicateFrom(node.OutNodes);
                RemoveDuplicateFrom(node.InNodes);
                
                void RemoveDuplicateFrom(List<CallGraphNode> list)
                {
                    var cache = new HashSet<CallGraphNode>();
                    for (var i = list.Count - 1; i >= 0; i--)
                    {
                        var item = list[i];
                        if (cache.Contains(item) || item == node)
                        {
                            list.RemoveAt(i);
                        }
                        else
                        {
                            cache.Add(item);
                        }
                    }
                }
            }
        }

        public void RemoveNonPublicEntryNodes()
        {
            var processingNodes = new Queue<CallGraphNode>(EntryNodes.Values);
            while (processingNodes.Count > 0)
            {
                var node = processingNodes.Dequeue();
                if (node.IsPublic) continue;
                
                foreach (var outNode in node.OutNodes)
                {
                    if (outNode == node) continue;
                    
                    outNode.InNodes.Remove(node);
                    if ((outNode.InNodes.Count == 0) ||
                        (outNode.InNodes.Count == 1 && outNode.Equals(outNode.InNodes[0])))    // simple recursive calls
                    {
                        // TODO: Add detection of recursive calls like A() -> B() -> A()
                        if (outNode.IsPublic)
                        {
                            EntryNodes.Add(outNode.MethodSignature, outNode);
                        }
                        else
                        {
                            processingNodes.Enqueue(outNode);
                        }
                    }
                }
                        
                EntryNodes.Remove(node.MethodSignature);
                Nodes.Remove(node.MethodSignature);
            }
        }

        public void RemoveMiddleNodes()
        {
            var replacedNodes = new Dictionary<CallGraphNode, List<CallGraphNode>>(Nodes.Count / 2);  
            var handledNodes = new HashSet<CallGraphNode>(Nodes.Count);
            var processingNodes = new Queue<CallGraphNode>(Roots.Values);
            //Console.WriteLine($"Nodes: {Nodes.Count}");
            while (processingNodes.Count > 0)
            {
                var node = processingNodes.Dequeue();
                if (!handledNodes.Add(node))
                    continue;

                for (var i = node.InNodes.Count - 1; i >= 0; i--)
                {
                    var inNode = node.InNodes[i];
                    if (replacedNodes.TryGetValue(inNode, out var list))
                    {
                        list.Add(node);
                        continue;
                    }

                    if (inNode.AssemblyName == node.AssemblyName &&
                        inNode.InNodes.Count > 0 &&
                        inNode.InNodes.All(x => x.AssemblyName == inNode.AssemblyName))
                    {
                        Nodes.Remove(inNode.MethodSignature);
                        node.InNodes.RemoveAt(i);

                        if (!replacedNodes.TryGetValue(inNode, out list))
                        {
                            list = new List<CallGraphNode>();
                            replacedNodes.Add(inNode, list);
                        }

                        if (replacedNodes.TryGetValue(node, out var previousList))
                        {
                            list.AddRange(previousList);
                        }
                        else
                        {
                            list.Add(node);
                        }
                    }

                    processingNodes.Enqueue(inNode);
                }
            }

            //Console.WriteLine($"Nodes: {Nodes.Count}");
            ///////////////////////////////////////////////////////////////////////
            handledNodes.Clear();
            processingNodes = new Queue<CallGraphNode>(EntryNodes.Values);
            while (processingNodes.Count > 0)
            {
                var node = processingNodes.Dequeue();
                if (!handledNodes.Add(node))
                    continue;

                for (var i = node.OutNodes.Count - 1; i >= 0; i--)
                {
                    var outNode = node.OutNodes[i];
                    if (replacedNodes.TryGetValue(outNode, out var list))
                    {
                        node.OutNodes.RemoveAt(i);
                        node.OutNodes.AddRange(list);
                        foreach (var replacedNode in list)
                        {
                            replacedNode.InNodes.Add(node);
                            processingNodes.Enqueue(replacedNode);
                        }
                    }
                    else
                    {
                        processingNodes.Enqueue(outNode);
                    }
                }
            }
        }
        
        public void RemoveSameClasses()
        {
            var handledNodes = new HashSet<CallGraphNode>(Nodes.Count);
            var processingNodes = new Queue<(CallGraphNode, CallGraphNode)>();
            
            EntryNodes.Clear();
            Nodes.Clear();
            var roots = Roots.Values;
            Roots = new Dictionary<MethodUniqueSignature, CallGraphNode>();
            foreach (var rootNode in roots)
            {
                // we keep all parents of root(s)
                if (!handledNodes.Add(rootNode))
                    continue;
                
                var mappedRootNode = new CallGraphNode(rootNode.info);
                Roots.Add(mappedRootNode.MethodSignature, mappedRootNode);
                Nodes.Add(mappedRootNode.MethodSignature, mappedRootNode);
                if (rootNode.InNodes.Count == 0 && !EntryNodes.ContainsKey(mappedRootNode.MethodSignature))
                    EntryNodes.Add(mappedRootNode.MethodSignature, mappedRootNode);
                    
                foreach (var inNode in rootNode.InNodes)
                {
                    var mappedNode = new CallGraphNode(inNode.info);
                    mappedRootNode.InNodes.Add(mappedNode);
                    mappedNode.OutNodes.Add(mappedRootNode);
                    
                    if (!Nodes.ContainsKey(mappedNode.MethodSignature))
                    {
                        Nodes.Add(mappedNode.MethodSignature, mappedNode);
                        if (inNode.InNodes.Count == 0)
                        {
                            if (!EntryNodes.ContainsKey(mappedNode.MethodSignature))
                                EntryNodes.Add(mappedNode.MethodSignature, mappedNode);
                        }
                        else
                        {
                            processingNodes.Enqueue((inNode, mappedNode));    
                        }
                    }

                }
            }
            
            // and keep only different classes for other parents
            while (processingNodes.Count > 0)
            {
                var (node, mappedNode) = processingNodes.Dequeue();
                if (!handledNodes.Add(node))
                    continue;

                var mappedInNodes = new HashSet<CallGraphNode>(mappedNode.InNodes);
                foreach (var inNode in node.InNodes)
                {
                    var classSignature = new MethodUniqueSignature(inNode.MethodSignature.ToClassName());
                    if (Nodes.TryGetValue(classSignature, out var mappedInNode))
                    {
                        if (mappedInNode != mappedNode && mappedInNodes.Add(mappedInNode))
                        {
                            mappedInNode.OutNodes.Add(mappedNode);
                        }
                    }
                    else
                    {
                        mappedInNode = new CallGraphNode(new CallInfo(
                            inNode.AssemblyInfo,
                            classSignature,
                            OpCodes.Nop,
                            inNode.IsPublic,
                            null));
                        
                        mappedInNodes.Add(mappedInNode);
                        mappedInNode.OutNodes.Add(mappedNode);
                        Nodes.Add(mappedInNode.MethodSignature, mappedInNode);
                    }

                    if (inNode.InNodes.Count == 0)
                    {
                        if (!EntryNodes.ContainsKey(mappedInNode.MethodSignature))
                            EntryNodes.Add(mappedInNode.MethodSignature, mappedInNode);
                    }
                    else
                    {
                        processingNodes.Enqueue((inNode, mappedInNode));
                    }
                }

                if (mappedNode.InNodes.Count != mappedInNodes.Count)
                {
                    mappedNode.InNodes.Clear();
                    mappedNode.InNodes.AddRange(mappedInNodes);
                }
            }
        }
        
        public void Dump(string path)
        {
            Console.WriteLine($"{path}: EntryPoints ({EntryNodes.Count}), Nodes ({Nodes.Count})");
            var entryPointsStatPath = Path.Combine(Path.GetDirectoryName(path) ?? String.Empty,
                Path.GetFileNameWithoutExtension(path) + ".stat.txt");
            var statByAssemblies = EntryNodes.Values.GroupBy(
                node => node.AssemblyName, 
                (key, nodes) => $"{key}: {nodes.Count()}");
                //(key, nodes) => new {Name = key, Count = nodes.Count()});
            File.WriteAllLines(entryPointsStatPath, statByAssemblies);
            File.AppendAllText(entryPointsStatPath, "\n");
            File.AppendAllText(entryPointsStatPath, $"Total: {EntryNodes.Count} (all nodes {Nodes.Count})");
            
            try
            {
                // may try >sfdp -x -Goverlap=prism -Tpng magic.gv > data.png 
                var pathGraphVizFile = Path.Combine(
                    Path.GetDirectoryName(path) ?? String.Empty, 
                    Path.GetFileNameWithoutExtension(path) + ".gv");
                var graphViz = new GraphViz(this);
                graphViz.Save(pathGraphVizFile);
                if (Nodes.Count < 0 || Nodes.Count > 1000)
                    return;
                
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = "\"" + pathGraphVizFile + "\" -Tpng -o \"" + path + "\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });

                process?.WaitForExit();
                //File.Delete(pathGraphVizFile);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error create a graph dump: {0}", exception);
            }
        }
    }
}