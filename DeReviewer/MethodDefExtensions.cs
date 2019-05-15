using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace dnlib.DotNet
{
    internal static class MethodDefExtensions
    {
        public static IEnumerable<MethodDef> FindOverrides(this MethodDef methodDef)
        {
            if (!methodDef.IsVirtual)
            {
                yield break;
            }
            
            if (methodDef.HasOverrides)
            {
                foreach (var baseMethod in methodDef.Overrides)
                {
                    var baseMethodDef = baseMethod.MethodDeclaration.ResolveMethodDef();
                    if (baseMethodDef == null)
                    {
                        //Console.WriteLine($"Error resolving method {baseMethod.MethodDeclaration.FullName}");
                    }
                    else
                    {
                        yield return baseMethodDef;
                    }
                }
                
                yield break;
            }

            var typeDef = methodDef.DeclaringType;
            var baseType = typeDef.BaseType;
            if (baseType != null)
            {
                var baseTypeDef = typeDef.BaseType.ResolveTypeDef();
                if (baseTypeDef == null)
                {
                    //Console.WriteLine($"Error resolving base type {typeDef.BaseType.FullName}");
                }
                else
                {
                    var baseMethod = baseTypeDef.FindMethod(methodDef.Name, methodDef.MethodSig);
                    if (baseMethod != null)
                    {
                        yield return baseMethod;
                        foreach (var method in FindOverrides(baseMethod))
                        {
                            yield return method;
                        }
                    }
                }
            }

            foreach (var baseInterface in typeDef.Interfaces)
            {
                if (baseInterface.Interface == null) continue;
                
                var baseInterfaceDef = baseInterface.Interface.ResolveTypeDef();
                if (baseInterfaceDef == null)
                {
                    //Console.WriteLine($"Error resolving interface {baseInterface.Interface.FullName}");
                }
                else
                {
                    var baseMethod = baseInterfaceDef.FindMethod(methodDef.Name, methodDef.MethodSig);
                    if (baseMethod != null)
                    {
                        yield return baseMethod;
                        foreach (var method in FindOverrides(baseMethod))
                        {
                            yield return method;
                        }
                    }
                }
            }
        }
        
        public static bool IsPublicGlobalVisibility(this MethodDef method)
        {
            if (method.IsPrivate ||
                method.IsFamilyAndAssembly ||
                method.IsAssembly)
            {
                return false;
            }

            return IsPublicGlobalVisibility(method.DeclaringType);
        }

        public static bool IsPublicGlobalVisibility(this TypeDef type)
        {
            if (!type.IsNested)
            {
                return type.IsPublic;
            }

            if (type.IsNestedPrivate ||
                type.IsNestedFamilyAndAssembly ||
                type.IsNestedAssembly)
            {
                return false;
            }
            
            // this nested class is public, let's check parent classes
            return IsPublicGlobalVisibility(type.DeclaringType);
        }
    }
}