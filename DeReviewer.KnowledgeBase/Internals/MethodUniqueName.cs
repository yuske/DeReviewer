using System;
using System.Linq;
using System.Reflection;
using System.Text;
using dnlib.DotNet;

namespace DeReviewer
{
    public sealed class MethodUniqueName
    {
        private readonly string fullName;
        public MethodUniqueName(IMethod method)
        {
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
        }
        
        public MethodUniqueName(MethodInfo method)
        {
            fullName = $"{method.DeclaringType.FullName}::{method.Name}" +
                       $"({String.Join(",", method.GetParameters().Select(p => p.ParameterType.FullName))})";
        }

        public MethodUniqueName(string fullNameWithoutReturnType)
        {
            fullName = ReplaceGenericParameters(new StringBuilder(fullNameWithoutReturnType));
        }

        public override string ToString() => fullName;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is MethodUniqueName c) return Equals(c);
            return false;
        }

        public override int GetHashCode()
        {
            return fullName.GetHashCode();
        }

        public static bool operator ==(MethodUniqueName left, MethodUniqueName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MethodUniqueName left, MethodUniqueName right)
        {
            return !Equals(left, right);
        }
        
        private bool Equals(MethodUniqueName other)
        {
            return string.Equals(fullName, other.fullName);
        }

        // temporary solution to analyze generic calls
        // w/o restriction on generic parameters
        private string ReplaceGenericParameters(StringBuilder sb)
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
                        throw new Exception($"Incorrect method signature {sb.ToString()}");
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