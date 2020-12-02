using System;
using System.Collections.Generic;
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace DeReviewer.Analysis
{
    public class CallGraphBuilder
    {
        private struct Statistic
        {
            public HashSet<Code> IgnoredOpcodes;
            public TimeSpan CallGraphBuilding;
        }
        
        private Statistic stat;
        private readonly IndexDb index;

        public CallGraphBuilder(IndexDb index)
        {
            this.index = index;
            
            stat.IgnoredOpcodes = new HashSet<Code>();
        }
        
        public void ShowStatistic()
        {
            Console.WriteLine($"Call Graph");
            Console.WriteLine(
                $"Ignored Opcodes: ({stat.IgnoredOpcodes.Count}) [{String.Join(", ", stat.IgnoredOpcodes)}]");
            Console.WriteLine($"----------");
            Console.WriteLine($"building: {stat.CallGraphBuilding}");
            Console.WriteLine();
        }

        public CallGraph CreateGraph(List<PatternInfo> patterns)
        {
            var timer = Stopwatch.StartNew();
            try
            {
                return CreateGraphInternal(patterns);
            }
            finally
            {
                stat.CallGraphBuilding = timer.Elapsed;
            }
        }

        private class ProcessingEntity
        {
            public ProcessingEntity(MethodUniqueSignature signature, CallGraphNode node)
            {
                Signature = signature;
                Node = node;
            }

            public MethodUniqueSignature Signature { get; }
            public CallGraphNode Node { get; }
        }

        private CallGraph CreateGraphInternal(List<PatternInfo> patterns)
        {
            var graph = new CallGraph();
            var processingEntities = new Queue<ProcessingEntity>();
            foreach (var pattern in patterns)
            {
                var calls = index.GetCalls(pattern);
                if (calls.Count > 0)
                {
                    // HACK if we found call w/ version restriction
                    // then we add it and find again w/o version restrictions
                    // in practice the result should be the same (but not always) 
                    var info = new CallInfo(
                        new AssemblyInfo(UTF8String.Empty, pattern.RequiredOlderVersion), 
                        pattern.Method,
                        OpCodes.Call, 
                        true,
                        new List<MethodUniqueSignature>(0));
                    var node = new CallGraphNode(info);
                    graph.Nodes.Add(pattern.Method, node);
                    graph.Roots.Add(pattern.Method, node);
                    processingEntities.Enqueue(new ProcessingEntity(node.MethodSignature, node));
                }
            }
            
            while (processingEntities.Count > 0)
            {
                var entity = processingEntities.Dequeue();
                
                var calls = index.GetCalls(entity.Signature/*, entity.Node.AssemblyInfo*/);
                if (calls.Count != 0)
                {
                    foreach (var callInfo in calls)
                    {
                        if (callInfo.Opcode.Code != Code.Call &&
                            callInfo.Opcode.Code != Code.Callvirt &&
                            callInfo.Opcode.Code != Code.Newobj &&
                            callInfo.Opcode.Code != Code.Ldtoken)          // used into Expression Tree in KB, need to implement DFA
                            //resume.Opcode.Code != Code.Ldvirtftn &&    // can use with calli 
                            //resume.Opcode.Code != Code.Ldftn)          // https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.calli?view=netframework-4.7.2             
                        {
                            // just ignore it for now, need to implement DFA
                            stat.IgnoredOpcodes.Add(callInfo.Opcode.Code);
                            continue;
                        }

                        if (graph.Nodes.TryGetValue(callInfo.Signature, out var callingNode))
                        {
                            // TODO: need to add a new kind of cycled edge to remove recursive calls in RemoveNonPublicEntryNodes 
                            //callingNode.OutNodes.Add(entity.Node);
                            //entity.Node.InNodes.Add(callingNode);
                        }
                        else
                        {
                            callingNode = new CallGraphNode(callInfo);
                            callingNode.OutNodes.Add(entity.Node);
                            entity.Node.InNodes.Add(callingNode);

                            graph.Nodes.Add(callInfo.Signature, callingNode);
                            processingEntities.Enqueue(new ProcessingEntity(callInfo.Signature, callingNode));

                            foreach (var overrideSignature in callInfo.OverrideSignatures)
                            {
                                processingEntities.Enqueue(new ProcessingEntity(overrideSignature, callingNode));
                            }
                        }
                    }
                }
            }
            
            foreach (var node in graph.Nodes.Values)
            {
                if (node.InNodes.Count != 0) continue;
                
                graph.EntryNodes.Add(node.MethodSignature, node);
            }

            return graph; 
        }
    }
}