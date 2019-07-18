using System;
using System.Linq;
using System.Reflection;
using DeReviewer.KnowledgeBase;
using NUnit.Framework;

namespace DeReviewer.Tests
{
    public class KnowledgeBasePatternTests
    {
        private readonly IndexDb index;
        
        public KnowledgeBasePatternTests()
        {
            index = new IndexDb(typeof(Context).Assembly.Location);
            index.Build();
        }

        [Test]
        //[Ignore("for debugging only, change type and methodName vars for your case")]
        public void SingleCase()
        {
            var type = typeof(KnowledgeBase.Cases.XslCompiledTransformPatterns);
            var methodName = nameof(KnowledgeBase.Cases.XslCompiledTransformPatterns.XsltLoadWithPayload); 

            var context = Context.CreateToAnalyze();
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            var errors = Loader.ExecuteCase(context, type, new[] {method});
            //var patternGroup = Loader.GetPatternGroup(type, methodName);

            Assert.That(errors, Is.Empty);
            Assert.That(context.Patterns.Count, Is.GreaterThan(0));
            
            var builder = new CallGraphBuilder(index);
            var graph = builder.CreateGraph(context.Patterns);
            
            Assert.That(graph.EntryNodes.Count, Is.GreaterThan(0));
            Assert.That(graph.EntryNodes.Keys
                .Contains(new MethodUniqueName($"{type.FullName}::{methodName}()")));
        }
        
        [Test]
        public void AllCases()
        {
            foreach (var type in Loader.GetCaseTypes())
            {
                Console.WriteLine($"{type}:");
                foreach (var method in Loader.GetCaseMethods(type))
                {
                    Console.WriteLine($"    {method}");
                    
                    var context = Context.CreateToAnalyze();
                    var errors = Loader.ExecuteCase(context, type, new[] {method});
                    
                    Assert.That(errors, Is.Empty);
                    Assert.That(context.Patterns.Count, Is.GreaterThan(0));
                    
                    var builder = new CallGraphBuilder(index);
                    var graph = builder.CreateGraph(context.Patterns);
            
                    Assert.That(graph.EntryNodes.Count, Is.GreaterThan(0));
                    Assert.That(graph.EntryNodes.Keys
                        .Contains(new MethodUniqueName($"{type.FullName}::{method.Name}()")));
                }
            }
        }
    }
}