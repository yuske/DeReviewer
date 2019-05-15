using System.Collections.Generic;
using DeReviewer.KnowledgeBase.Internals;

namespace DeReviewer.KnowledgeBase
{
    public static class Dsl
    {
        public static string PayloadCommand { get; set; } = "calc";

        public static Mode Mode { get; set; } = Mode.Test;
        
        public static List<PatternInfo> Patterns { get; set; } = new List<PatternInfo>();
    }
    
    public enum Mode
    {
        Analyze,
        Test
    }
}