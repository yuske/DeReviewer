using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DeReviewer
{
    public class CallGraph
    {
        public Dictionary<MethodUniqueName, CallGraphNode> Nodes { get; } = new Dictionary<MethodUniqueName, CallGraphNode>();
        public Dictionary<MethodUniqueName, CallGraphNode> EntryNodes { get; } = new Dictionary<MethodUniqueName, CallGraphNode>();

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
        
        public void Dump(string path)
        {
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