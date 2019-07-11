using System.Linq;
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

            var patternGroup = Loader.GetPatternGroup(type, methodName);

            // DSL should contains right patterns
            Assert.That(patternGroup.Patterns.Count, Is.GreaterThan(0));
            
            var builder = new CallGraphBuilder(index);
            var graph = builder.CreateGraph(patternGroup.Patterns);
            
            Assert.That(graph.EntryNodes.Count, Is.GreaterThan(0));
            Assert.That(graph.EntryNodes.Select(node => node.MethodSignature)
                .Contains(new MethodUniqueName($"{type.FullName}::{methodName}()")));
        }
    }
}