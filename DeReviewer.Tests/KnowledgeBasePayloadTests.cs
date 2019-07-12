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
        
        [Test]
        //[Ignore("for debugging only, change type and methodName vars for your case")]
        public void SingleCase()
        {
            var type = typeof(KnowledgeBase.Cases.BinaryFormatterPatterns);
            var methodName = nameof(KnowledgeBase.Cases.BinaryFormatterPatterns.Deserialize); 
            
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            var errors = Test(type, method);
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void AllCases()
        {
            var errors = new List<string>();
            foreach (var type in Loader.GetCaseTypes())
            {
                Console.WriteLine($"{type}:");
                foreach (var method in Loader.GetCaseMethods(type))
                {
                    Console.WriteLine($"    {method}");
                    errors.AddRange(Test(type, method));
                }
            }
            
            Assert.That(errors, Is.Empty);
        }

        private List<string> Test(Type type, MethodInfo method)
        {
            var id = Guid.NewGuid().ToString();
            var context = Context.CreateToTest($"echo some-text > {id}");
            
            var errors = Loader.ExecuteCase(context, type, new[] {method});

            var attempt = 0;
            while (attempt++ < 5)
            {
                Thread.Sleep(100);
                if (File.Exists(id))
                {
                    DeleteFileSafe(id);
                    return errors;
                }
            }
            
            errors.Add($"The payload has not been executed ({id})");
            return errors;
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