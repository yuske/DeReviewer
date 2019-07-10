using System.Collections.Generic;
using DeReviewer.KnowledgeBase.Internals;

namespace DeReviewer.KnowledgeBase
{
    public class Context
    {
        public enum ExecutionMode
        {
            Analyze,
            Test
        }

        public static Context CreateToTest(string payloadCommand) =>
            new Context(ExecutionMode.Test, payloadCommand);
        
        public static Context CreateToAnalyze() =>
            new Context(ExecutionMode.Analyze, null);

        private Context(ExecutionMode mode, string command)
        {
            Mode = mode;
            PayloadCommand = command;
        }

        public string PayloadCommand { get; }

        public ExecutionMode Mode { get; }
        
        public List<PatternInfo> Patterns { get; } = new List<PatternInfo>();
    }
}