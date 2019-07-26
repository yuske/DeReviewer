using System;
using System.Linq;
using System.Reflection;
using System.Text;
using dnlib.DotNet;

namespace DeReviewer.Analysis
{
    public static class MethodUniqueSignatureExtensions
    {
        public static MethodUniqueSignature CreateMethodUniqueName(this MethodInfo method)
        {
            var fullName = $"{method.DeclaringType.FullName}::{method.Name}" +
                           $"({String.Join(",", method.GetParameters().Select(p => p.ParameterType.FullName))})";
            
            return new MethodUniqueSignature(fullName);
        }

        internal static MethodUniqueSignature CreateMethodUniqueName(this IMethod method)
        {
            string fullName;
            
            // TODO: PERF don't use method.FullName, it's an expensive operation
            var name = method.FullName;
            var firstSpace = name.IndexOf(' ');
            if (firstSpace < 0 || firstSpace >= name.Length - 1)
            {
                Console.WriteLine($"ERROR: The method {name} doesn't contain return value");
                fullName = ReplaceGenericParameters(new StringBuilder(name));
            }
            else
            {
                // remove return value
                var sb = new StringBuilder(name);
                sb.Remove(0, firstSpace + 1);
                fullName = ReplaceGenericParameters(sb);
            }
            
            return new MethodUniqueSignature(fullName);
        }
        
        // temporary solution to analyze generic calls
        // w/o restriction on generic parameters
        internal static string ReplaceGenericParameters(StringBuilder sb)
        {
            int start = -1;
            int nesting = 0;
            for (int i = 0; i < sb.Length; i++)
            {
                if (start == -1 && sb[i] == '`')
                {
                    start = i;
                }
                else if (sb[i] == '<')
                {
                    nesting++;
                    if (start == -1)
                    {
                        start = i;
                    }
                }
                else if (sb[i] == '>')
                {
                    nesting--;
                    if (nesting < 0)
                    {
                        throw new Exception($"Incorrect method signature {sb}");
                    }

                    if (nesting == 0)
                    {
                        var length = i - start + 1;
                        sb.Remove(start, length);
                        i -= length;
                        start = -1;
                    }
                }
            }

            return sb.ToString();
        }
    }
}