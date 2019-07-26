using DeReviewer.Analysis;
using dnlib.DotNet;
using NUnit.Framework;

namespace DeReviewer.Tests
{
    public class YamlDotNetTests : SelfModelBase
    {
        public YamlDotNetTests()
            :base("Deserializers")
        {
            
        }

        [Test]
        public void MethodUniqueNameGenericTest()
        {
            var name = MethodUniqueSignature.Create(
                "YamlDotNet.Serialization.BuilderSkeleton`1<YamlDotNet.Serialization.DeserializerBuilder>::.ctor()");
            Assert.That(name.ToString(), Is.EqualTo("YamlDotNet.Serialization.BuilderSkeleton::.ctor()"));
            
            var name2 = MethodUniqueSignature.Create("System.Collections.Generic.IDictionary`2<System.String,System.Object> YamlDotNet.Serialization.Deserializer::Deserialize<System.Collections.Generic.IDictionary`2<System.String,System.Object>>(System.String)");
            Assert.That(name2.ToString(), Is.EqualTo("System.Collections.Generic.IDictionary YamlDotNet.Serialization.Deserializer::Deserialize(System.String)"));
        }
        
        [Test]
        public void DeserializeGeneric()
        {
            var count = 0;
            foreach (var typeDef in base.EnumerateTypes())
            {
                if (typeDef.Name == "YamlDotNet")
                {
                    foreach (var methodDef in typeDef.Methods)
                    {
                        foreach (var instruction in methodDef.Body.Instructions)
                        {
                            if (instruction.Operand is IMethod methodOperand)
                            {
                                var name = methodOperand.FullName;
                                if (name.Contains("Deserializer::Deserialize"))
                                {
                                    count++;
                                    var m = methodOperand.CreateMethodUniqueName();
                                    Assert.That(m.ToString(), Is.EqualTo("YamlDotNet.Serialization.Deserializer::Deserialize(System.String)"));
                                }
                            }
                        }
                    }
                    
                    break;
                }
            }

            Assert.That(count, Is.EqualTo(1));
        }
    }
}