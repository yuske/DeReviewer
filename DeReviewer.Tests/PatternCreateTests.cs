using System.Linq;
using DeReviewer.KnowledgeBase;
using DeReviewer.Tests.Model.Overloading;
using NUnit.Framework;

namespace DeReviewer.Tests
{
    // TODO: add tests for ref/out params, abstract class, interface
    public class PatternCreateTests : Case
    {
        private Context context;
        
        [SetUp]
        public void Setup()
        {
            context = Context.CreateToAnalyze();
            Initialize(context);
        }
        
        [Test]
        public void CreateByNameOfProcedureOverloadedOnlyDeclaringClass()
        {
            var obj = new DerivedClass();
            Pattern.CreateByName(it => obj.Foo(5));

            var derivedName = $"{typeof(DerivedClass)}::{nameof(DerivedClass.Foo)}";
            var baseName = $"{typeof(BaseClass)}::{nameof(BaseClass.Foo)}";
            Assert.That(context.Patterns.Select(info => info.Method.ToString()), Is.EquivalentTo(new []
            {
                $"{derivedName}()",
                $"{derivedName}(System.Int32)",
                $"{derivedName}(System.String)",
                $"{derivedName}(,System.Int32)",    // TODO: ignored generic parameter
                $"{derivedName}(System.Boolean,System.Object)",
                $"{derivedName}(System.Int32,System.Int32,System.Int32)",
                $"{baseName}()",
                $"{baseName}(System.Char)",
                $"{derivedName}(System.Double)",
                //$"{baseName}(System.Double)",    // TODO: ignored overloaded method
                $"{baseName}(System.Object)",    // it's Foo(dynamic d)
                
            }));
        }
        
        [Test]
        public void CreateByNameOfFunctionOverloadedOnlyDeclaringClass()
        {
            var obj = new DerivedClass();
            Pattern.CreateByName(it => obj.Foo(1, 2, 3));
            
            var derivedName = $"{typeof(DerivedClass)}::{nameof(DerivedClass.Foo)}";
            var baseName = $"{typeof(BaseClass)}::{nameof(BaseClass.Foo)}";
            Assert.That(context.Patterns.Select(info => info.Method.ToString()), Is.EquivalentTo(new []
            {
                $"{derivedName}()",
                $"{derivedName}(System.Int32)",
                $"{derivedName}(System.String)",
                $"{derivedName}(T,System.Int32)",    // TODO: ignored generic parameter
                $"{derivedName}(System.Boolean,System.Object)",
                $"{derivedName}(System.Int32,System.Int32,System.Int32)",
                $"{baseName}()",
                $"{baseName}(System.Char)",
                $"{derivedName}(System.Double)",
                $"{baseName}(System.Double)",    // TODO: ignored overloaded method
                $"{baseName}(System.Object)",    // it's Foo(dynamic d)
            }));
        }

        [Test]
        public void CreateByNameInGenericClass()
        {
            var obj = new GenericClass<string>();
            Pattern.CreateByName(it => obj.Foo());
            
            var className = $"{typeof(GenericClass<string>)}::{nameof(GenericClass<string>.Foo)}";
            Assert.That(context.Patterns.Select(info => info.Method.ToString()), Is.EquivalentTo(new []
            {
                $"{className}()",
                $"{className}(System.String)",
            }));
        }
    }
}