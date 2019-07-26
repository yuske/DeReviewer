using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace DeReviewer.Analysis
{
    public class CallInfo
    {
        public CallInfo(AssemblyInfo assemblyInfo, MethodUniqueSignature signature, OpCode opcode, bool isPublic, 
            List<MethodUniqueSignature> overrideSignatures)
        {
            AssemblyInfo = assemblyInfo;
            Signature = signature;
            Opcode = opcode;
            IsPublic = isPublic;
            OverrideSignatures = overrideSignatures;
        }

        public AssemblyInfo AssemblyInfo { get; }
        
        public MethodUniqueSignature Signature { get; }
        
        public OpCode Opcode { get; }
        
        public bool IsPublic { get; }
        
        public List<MethodUniqueSignature> OverrideSignatures { get; }
    }
}