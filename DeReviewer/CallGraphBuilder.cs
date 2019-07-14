using System;
using System.Collections.Generic;
using System.Diagnostics;
using dnlib.DotNet.Emit;
using DeReviewer.KnowledgeBase.Internals;
using dnlib.DotNet;

namespace DeReviewer
{
    internal class CallGraphBuilder
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

        private CallGraph CreateGraphInternal(List<PatternInfo> patterns)
        {
            var graph = new CallGraph();
            var processingNodes = new Queue<CallGraphNode>();
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
                        new List<MethodUniqueName>(0));
                    var node = new CallGraphNode(info);
                    graph.Nodes.Add(pattern.Method, node);
                    processingNodes.Enqueue(node);
                }
            }
            
            while (processingNodes.Count > 0)
            {
                var node = processingNodes.Dequeue();
                var calls = index.GetCalls(node.MethodSignature, node.AssemblyInfo); 
                if (calls.Count == 0)
                {
                    graph.EntryNodes.Add(node);
                }
                else
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
                        
                        HandleCallGraphNode(callInfo.Signature);
                        foreach (var overrideMethodSignature in callInfo.OverrideSignatures)
                        {
                            HandleCallGraphNode(overrideMethodSignature);
                        }

                        void HandleCallGraphNode(MethodUniqueName signature)
                        {
                            if (graph.Nodes.TryGetValue(signature, out var existingNode))
                            {
                                existingNode.OutNodes.Add(node);
                                node.InNodes.Add(existingNode);
                            }
                            else
                            {
                                var newNode = new CallGraphNode(callInfo);
                                newNode.OutNodes.Add(node);
                                node.InNodes.Add(newNode);

                                graph.Nodes.Add(signature, newNode);
                                processingNodes.Enqueue(newNode);
                            }
                        }
                    }
                }
            }

            return graph; 
        }
    }
}