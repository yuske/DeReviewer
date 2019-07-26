using System;

namespace DeReviewer.Analysis
{
    public class PatternInfo
    {
        public PatternInfo(MethodUniqueSignature method, Version requiredOlderVersion)
        {
            Method = method;
            RequiredOlderVersion = requiredOlderVersion;
        }

        public MethodUniqueSignature Method { get; }
        public Version RequiredOlderVersion { get; }

        public override string ToString() => $"{Method}, v{RequiredOlderVersion}";
    }
}