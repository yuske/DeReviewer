using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using NUnit.Framework;

namespace DeReviewer.Tests
{
    // TODO: check call Explicit method
    public class InheritanceModelTests : SelfModelBase
    {
        public InheritanceModelTests()
            :base("Inheritance")
        {
        }
        
        [Test]
        public void MethodOverridesTest()
        {
            var result = new Dictionary<string, string[]>();
            foreach (var typeDef in EnumerateTypes())
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    if (methodDef.Name == ".ctor") continue;
                    
                    Console.WriteLine($"{methodDef.FullName}: {methodDef.HasOverrides}");
                    result.Add(methodDef.FullName, methodDef.FindOverrides().Select(md => md.FullName).ToArray());
                }
            }
            
            Assert.That(result, Is.EquivalentTo(new Dictionary<string, string[]>
            {
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.AbstractClass::Foo()", 
                    new []{"System.Void DeReviewer.Tests.Model.Inheritance.IInterface::Foo()"}
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.AbstractClass::Bar()", 
                    new string[0]
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.DerivedClass::Foo()", 
                    new []
                    {
                        "System.Void DeReviewer.Tests.Model.Inheritance.SpecificClass::Foo()",
                        "System.Void DeReviewer.Tests.Model.Inheritance.AbstractClass::Foo()",
                        "System.Void DeReviewer.Tests.Model.Inheritance.IInterface::Foo()"
                    }
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.DerivedClass::Bar()", 
                    new []
                    {
                        "System.Void DeReviewer.Tests.Model.Inheritance.SpecificClass::Bar()",
                        "System.Void DeReviewer.Tests.Model.Inheritance.AbstractClass::Bar()"
                    }
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.DerivedClass::JustVirtual()", 
                    new []{"System.Void DeReviewer.Tests.Model.Inheritance.SpecificClass::JustVirtual()"}
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.DerivedClass::DeReviewer.Tests.Model.Inheritance.IInterface2.Explicit()", 
                    new []{"System.Void DeReviewer.Tests.Model.Inheritance.IInterface2::Explicit()"}
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.IInterface::Foo()", 
                    new string[0]
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.IInterface2::Explicit()", 
                    new string[0]
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.SpecificClass::JustVirtual()", 
                    new string[0]
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.SpecificClass::Bar()", 
                    new []{"System.Void DeReviewer.Tests.Model.Inheritance.AbstractClass::Bar()"}
                },
                {
                    "System.Void DeReviewer.Tests.Model.Inheritance.SpecificClass::Foo()", 
                    new []
                    {
                        "System.Void DeReviewer.Tests.Model.Inheritance.AbstractClass::Foo()",
                        "System.Void DeReviewer.Tests.Model.Inheritance.IInterface::Foo()"
                    }
                },
            }));
        }
    }
}