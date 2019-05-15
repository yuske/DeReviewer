using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DeReviewer.KnowledgeBase;
using DeReviewer.KnowledgeBase.Internals;

namespace DeReviewer
{
    internal static class Loader
    {
        public static object CreateObjectOf(Type type)
        {
            var defaultCtor = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
            return defaultCtor();
        }

        public static Action CreateMethodCall(object instance, MethodInfo method)
        {
            if (method == null)
            {
                throw new Exception("The public(!) instance(!) method has not found");
            }
            
            if (method.GetParameters().Length > 0)
            {
                throw new Exception("The method must not have parameters");
            }

            var instanceExpression = Expression.Constant(instance);
            return Expression.Lambda<Action>(Expression.Call(instanceExpression, method)).Compile();
        }
        
        public static IEnumerable<PatternGroup> GetPatternGroups()
        {
            Dsl.Mode = Mode.Analyze;
            
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
                    instance = CreateObjectOf(type);
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
                        var testCase = CreateMethodCall(instance, method);
                        testCase();
                    }
                    catch (Exception e)
                    {
                        errors.Add($"{Title(type, method)} {e.Message}");
                    }
                }

                yield return new PatternGroup(type.Name, Dsl.Patterns);
                Dsl.Patterns = new List<PatternInfo>();
            }
        }
        
        private static string Title(Type type, MethodInfo method = null) =>
            method == null ? $"{type.Name}:" : $"{type.Name}.{method.Name}():";
    }
}