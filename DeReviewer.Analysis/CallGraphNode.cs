using System.Collections.Generic;

namespace DeReviewer
{
    public class CallGraphNode
    {
        private readonly CallInfo info;

        public CallGraphNode(CallInfo info)
        {
            this.info = info;
            InNodes = new List<CallGraphNode>();
            OutNodes = new List<CallGraphNode>();
        }

        public AssemblyInfo AssemblyInfo => info.AssemblyInfo;
        public string AssemblyName => info.AssemblyInfo.Name;

        public MethodUniqueName MethodSignature => info.Signature;

        public bool IsPublic => info.IsPublic;

        public List<CallGraphNode> InNodes { get; }
        public List<CallGraphNode> OutNodes { get; }

        public override string ToString() => $"{MethodSignature} v.{AssemblyInfo.Version}";
    }
}