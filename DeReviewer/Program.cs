using System;
using System.IO;
using CommandLine;
using DeReviewer.Analysis;
using DeReviewer.CommandLine;

namespace DeReviewer
{
    static class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<AnalyzeOptions, PayloadOptions>(args)
                .MapResult(
                    (AnalyzeOptions options) => RunAnalysis(options),
                    (PayloadOptions options) => GeneratePayload(options),
                    errs => 1);
        }

        private static int RunAnalysis(AnalyzeOptions options)
        {
            var indexDb = new IndexDb(options.Directory);
            indexDb.Build();
            indexDb.ShowStatistic();
            
            var callGraphBuilder = new CallGraphBuilder(indexDb);
            CreateCleanDirectory(options.Output);
            foreach (var group in Loader.GetPatternGroups())
            {
                var callGraph = callGraphBuilder.CreateGraph(group.Patterns);
                callGraphBuilder.ShowStatistic();
                if (callGraph.IsEmpty) continue;

                callGraph.RemoveDuplicatePaths();
                
                var groupDirectory = Path.Combine(options.Output, group.Name);
                Directory.CreateDirectory(groupDirectory);
                callGraph.Dump(Path.Combine(groupDirectory, "full.png"));
                callGraph.DumpSeparateUsages(Path.Combine(groupDirectory, "usages"));

                callGraph.RemoveSameClasses();
                //callGraph.RemoveMiddleNodes();
                callGraph.Dump(Path.Combine(groupDirectory, "classes.png"));

                //callGraph.RemoveNonPublicEntryNodes();
                //callGraph.Dump(Path.Combine(groupDirectory, "public.png"));
            }
            
            return 0;
        }

        private static void CreateCleanDirectory(string name)
        {
            if (Directory.Exists(name))
            {
                Directory.Delete(name, true);
            }

            Directory.CreateDirectory(name);
        }

        private static int GeneratePayload(PayloadOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
