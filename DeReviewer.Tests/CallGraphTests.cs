using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using NUnit.Framework;

namespace DeReviewer.Tests
{
    public class CallGraphTests
    {
        [Test]
        public void Dump()
        {
            // A B
            //  C
            // D E
            //  F
            var a = CreateCallGraphNode("A");
            var b = CreateCallGraphNode("B");
            var c = CreateCallGraphNode("C");
            var d = CreateCallGraphNode("D");
            var e = CreateCallGraphNode("E");
            var f = CreateCallGraphNode("F");
            Link(a, c);
            Link(b, c);
            Link(c, d);
            Link(c, e);
            Link(d, f);
            Link(e, f);

            var callGraph = CreateCallGraph(a, b, c, d, e, f);
            callGraph.Dump(@"DumpTest.png");
            Assert.True(true);
        }

        private CallGraphNode CreateCallGraphNode(string name)
        {
            var info = new CallInfo(
                new AssemblyInfo(UTF8String.Empty, AssemblyInfo.EmptyVersion), 
                new MethodUniqueName(name),
                OpCodes.Call, 
                true,
                new List<MethodUniqueName>(0));
            
            return new CallGraphNode(info);
        }

        private void Link(CallGraphNode from, CallGraphNode to)
        {
            from.OutNodes.Add(to);
            to.InNodes.Add(from);
        }

        private CallGraph CreateCallGraph(params CallGraphNode[] nodes)
        {
            var callGraph = new CallGraph();
            foreach (var node in nodes)
            {
                callGraph.Nodes.Add(node.MethodSignature, node);
            }

            return callGraph;
        }
    }
}