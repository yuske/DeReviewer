using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;

namespace DeReviewer.Tests
{
    public class SelfModelBase
    {
        private const string ModelNamespace = "DeReviewer.Tests.Model";
        private readonly string subModelName;
        private readonly ModuleDefMD module;

        public SelfModelBase()
            :this(String.Empty)
        {
            
        }
        
        public SelfModelBase(string subModelName)
        {
            this.subModelName = subModelName;
            module = ModuleDefMD.Load(typeof(SelfModelBase).Assembly.Location);
        }

        public IEnumerable<TypeDef> EnumerateTypes()
        {
            var fullModelNamespace = !String.IsNullOrWhiteSpace(subModelName)
                ? $"{ModelNamespace}.{subModelName}"
                : ModelNamespace;
            
            return module.GetTypes().Where(typeDef => typeDef.FullName.StartsWith(fullModelNamespace));
        }
    }
}