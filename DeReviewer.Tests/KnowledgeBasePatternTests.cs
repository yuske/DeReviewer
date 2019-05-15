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
            index = new IndexDb(typeof(Dsl).Assembly.Location);
            index.Build();
        }
        
        //[Ignore("for debugging only, change TestCase attribute for your case")]
        [TestCase(typeof(KnowledgeBase.Cases.YamlDotNet), nameof(KnowledgeBase.Cases.YamlDotNet.MostGenericPattern))]
        public void SingleCase(Type type, string methodName)
        {
            Dsl.Mode = Mode.Analyze;
            var obj = Loader.CreateObjectOf(type);
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            var testCase = Loader.CreateMethodCall(obj, method);
            testCase();
            
            // DSL should contains right patterns
            Assert.That(Dsl.Patterns.Count, Is.GreaterThan(0));
            
            var builder = new CallGraphBuilder(index);
            var graph = builder.CreateGraph(Dsl.Patterns);
            
            Assert.That(graph.EntryNodes.Count, Is.GreaterThan(0));
            Assert.That(graph.EntryNodes.Select(node => node.MethodSignature).Contains(new MethodUniqueName($"{type.FullName}::{methodName}()")));
        }
    }
}