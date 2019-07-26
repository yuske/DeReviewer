using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace DeReviewer
{
    public class CallInfo
    {
        public CallInfo(AssemblyInfo assemblyInfo, MethodUniqueName signature, OpCode opcode, bool isPublic, 
            List<MethodUniqueName> overrideSignatures)
        {
            AssemblyInfo = assemblyInfo;
            Signature = signature;
            Opcode = opcode;
            IsPublic = isPublic;
            OverrideSignatures = overrideSignatures;
        }

        public AssemblyInfo AssemblyInfo { get; }
        
        public MethodUniqueName Signature { get; }
        
        public OpCode Opcode { get; }
        
        public bool IsPublic { get; }
        
        public List<MethodUniqueName> OverrideSignatures { get; }
    }
}