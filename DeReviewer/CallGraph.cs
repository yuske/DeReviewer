using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DeReviewer
{
    internal class CallGraph
    {
        public Dictionary<MethodUniqueName, CallGraphNode> Nodes { get; } = new Dictionary<MethodUniqueName, CallGraphNode>();
        public List<CallGraphNode> EntryNodes { get; } = new List<CallGraphNode>();

        public bool IsEmpty => Nodes.Count == 0;

        public void RemoveDuplicatePaths()
        {
            void RemoveDuplicateFrom(List<CallGraphNode> list)
            {
                var cache = new HashSet<CallGraphNode>();
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    var item = list[i];
                    if (!cache.Contains(item))
                    {
                        cache.Add(item);
                    }
                    else
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            
            foreach (var node in Nodes.Values)
            {
                RemoveDuplicateFrom(node.OutNodes);
                RemoveDuplicateFrom(node.InNodes);
            }
        }

        public void RemoveNonPublicEntryNodes()
        {
            var processingNodes = new Queue<CallGraphNode>(EntryNodes);
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
                        if (!outNode.IsPublic)
                        {
                            processingNodes.Enqueue(outNode);
                        }
                        else
                        {
                            EntryNodes.Add(outNode);
                        }
                    }
                }
                        
                EntryNodes.Remove(node);
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