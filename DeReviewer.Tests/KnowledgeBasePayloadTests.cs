using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using DeReviewer.KnowledgeBase;
using NUnit.Framework;

namespace DeReviewer.Tests
{
    public class KnowledgeBasePayloadTests
    {
        public KnowledgeBasePayloadTests()
        {
            var directory = Path.GetDirectoryName(typeof(KnowledgeBasePayloadTests).Assembly.Location);
            Environment.CurrentDirectory = directory ?? throw new InvalidOperationException("Assembly.Location is null");
        }
        
        //[Ignore("for debugging only, change TestCase attribute for your case")]
        [TestCase(typeof(KnowledgeBase.Cases.YamlDotNet), nameof(KnowledgeBase.Cases.YamlDotNet.MostGenericPattern))]
        public void SingleCase(Type type, string methodName)
        {
            Test(() =>
            {
                var obj = Loader.CreateObjectOf(type);
                var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                var testCase = Loader.CreateMethodCall(obj, method);
                testCase();
            });
        }

        [Test]
        public void AllCases()
        {
            var errors = new List<string>();
            var typeCaseSample = typeof(DeReviewer.KnowledgeBase.Cases.YamlDotNet);
            foreach (var type in typeCaseSample.Assembly.GetTypes())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                // ReSharper disable once PossibleNullReferenceException
                if (!type.Namespace.StartsWith(typeCaseSample.Namespace)) continue;

                object instance;
                try
                {
                    instance = Loader.CreateObjectOf(type);
                }
                catch (Exception e)
                {
                    errors.Add($"{Title(type)} Error default constructor calling. {e.Message}");
                    continue;
                }
                
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    //skip Object methods
                    if (method.ToString() == "Boolean Equals(System.Object)" ||
                        method.ToString() == "System.String ToString()" ||
                        method.ToString() == "Int32 GetHashCode()" ||
                        method.ToString() == "System.Type GetType()")
                    {
                        continue;
                    }
                    
                    try
                    {
                        var testCase = Loader.CreateMethodCall(instance, method);
                        Test(testCase, false);
                    }
                    catch (Exception e)
                    {
                        errors.Add($"{Title(type, method)} {e.Message}");
                    }
                }
            }
            
            Assert.That(errors, Is.Empty);
        }

        private void Test(Action testCase, bool showException = true)
        {
            var id = Guid.NewGuid().ToString();
            Dsl.PayloadCommand = $"echo some-text > {id}";
            try
            {
                testCase();
            }
            catch (Exception e)
            {
                if (showException)
                {
                    Console.WriteLine(e);
                }
            }

            var attempt = 0;
            while (attempt++ < 5)
            {
                Thread.Sleep(100);
                if (File.Exists(id))
                {
                    DeleteFileSafe(id);
                    return;
                }
            }
            
            throw new Exception($"The payload has not been executed ({id})");
        }

        private void DeleteFileSafe(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private string Title(Type type, MethodInfo method = null) =>
            method == null ? $"{type.Name}:" : $"{type.Name}.{method.Name}():";
    }
}