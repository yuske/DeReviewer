using System;

namespace DeReviewer.KnowledgeBase.Internals
{
    public class PatternInfo
    {
        public PatternInfo(MethodUniqueName method, Version requiredOlderVersion)
        {
            Method = method;
            RequiredOlderVersion = requiredOlderVersion;
        }

        public MethodUniqueName Method { get; }
        public Version RequiredOlderVersion { get; }

        public override string ToString() => $"{Method}, v{RequiredOlderVersion}";
    }
}