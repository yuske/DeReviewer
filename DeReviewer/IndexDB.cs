using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using DeReviewer.KnowledgeBase.Internals;

namespace DeReviewer
{
    internal class IndexDb
    {
        private struct Statistic
        {
            public long AssemblyCount;
            public long TypeCount;
            public long MethodCount;

            public TimeSpan AssemblyLoading;
            public TimeSpan IndexBuilding;
        }
        
        // TODO: add restriction to add any items to the empty list
        private readonly List<CallInfo> empty = new List<CallInfo>();
        private Statistic stat = new Statistic();
        private readonly string path;
        
        //<method call> -> <assembly of the method call> -> <method definition w/ (1)>
        private Dictionary<MethodUniqueName, Dictionary<AssemblyInfo, List<CallInfo>>> callers;
        
        public IndexDb(string path)
        {
            this.path = path;
        }

        public void Build()
        {
            var timer = Stopwatch.StartNew();
            callers = new Dictionary<MethodUniqueName, Dictionary<AssemblyInfo, List<CallInfo>>>();
            var assemblies = new List<ModuleDefMD>();

            if (File.Exists(path))
            {
                LoadAssembly(path);
            }
            else
            {
                //foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    LoadAssembly(file);
                }
            }

            stat.AssemblyCount = assemblies.Count;
            stat.AssemblyLoading = timer.Elapsed;
            
            timer = Stopwatch.StartNew();
            BuildIndexes(assemblies);
            stat.IndexBuilding = timer.Elapsed;

            void LoadAssembly(string fileName)
            {
                try
                {
                    assemblies.Add(ModuleDefMD.Load(fileName));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error load '{Path.GetFileName(fileName)}': {e.Message}");
                }
            }
        }

        // TODO: use IReadOnlyCollection<CallInfo> 
        public List<CallInfo> GetCalls(MethodUniqueName methodSignature)
        {
            if (callers.TryGetValue(methodSignature, out var callInfoMap))
            {
                if (callInfoMap.Count == 1)
                {
                    return callInfoMap.Values.First();
                }

                var result = new List<CallInfo>();
                foreach (var list in callInfoMap.Values)
                {
                    result.AddRange(list);
                }
                
                return result;
            }
            
            return empty;
        }

        public List<CallInfo> GetCalls(MethodUniqueName methodSignature, AssemblyInfo assemblyInfo)
        {
            if (assemblyInfo == null ||
                assemblyInfo.Name == (UTF8String) null ||
                assemblyInfo.Name == UTF8String.Empty ||
                assemblyInfo.Version == null ||
                assemblyInfo.Version == AssemblyInfo.EmptyVersion)
            {
                return GetCalls(methodSignature);
            }
            
            if (callers.TryGetValue(methodSignature, out var callInfoMap))
            {
                return callInfoMap[assemblyInfo];
            }

            return empty;
        }

        public List<CallInfo> GetCalls(PatternInfo pattern)
        {
            if (callers.TryGetValue(pattern.Method, out var callInfoMap))
            {
                if (callInfoMap.Count == 1)
                {
                    var assemblyInfo = callInfoMap.Keys.First();
                    return pattern.RequiredOlderVersion == null ||
                           pattern.RequiredOlderVersion == AssemblyInfo.EmptyVersion ||
                           assemblyInfo.Version < pattern.RequiredOlderVersion
                                ? callInfoMap.Values.First() 
                                : empty;
                }
                
                var result = new List<CallInfo>();
                foreach (var pair in callInfoMap)
                {
                    if (pattern.RequiredOlderVersion == null ||
                        pattern.RequiredOlderVersion == AssemblyInfo.EmptyVersion ||
                        pair.Key.Version < pattern.RequiredOlderVersion)
                    {
                        result.AddRange(pair.Value);
                    }
                }

                return result;
            }

            return empty;
        }

        public void ShowStatistic()
        {
            Console.WriteLine($"Assemblies: {stat.AssemblyCount}");
            Console.WriteLine($"----------");
            Console.WriteLine($"loading {stat.AssemblyLoading}");
            Console.WriteLine();
            Console.WriteLine($"Types: {stat.TypeCount}");
            Console.WriteLine($"Methods: {stat.MethodCount}");
            Console.WriteLine($"----------");
            Console.WriteLine($"indexing {stat.IndexBuilding}");
            Console.WriteLine();
        }

        private void BuildIndexes(List<ModuleDefMD> modules)
        {
            foreach (var module in modules)
            {
                foreach (var typeDef in module.GetTypes())
                {
                    stat.TypeCount++;
                    if (!typeDef.HasMethods) continue;

                    foreach (var methodDef in typeDef.Methods)
                    {
                        stat.MethodCount++;
                        if (!methodDef.HasBody) continue;
                        if (!methodDef.Body.HasInstructions) continue;
                        
                        CallInfo cacheInfo = null;
                        foreach (var instruction in methodDef.Body.Instructions)
                        {
                            // TODO: check opcode
                            //    call: 802655
                            //    callvirt: 918700
                            //    newobj: 255686
                            //    ldsfld: 27607
                            //    ldfld: 57948
                            //    stfld: 23493
                            //    ldftn: 41261
                            //    ldflda: 4556
                            //    ldvirtftn: 1453
                            //    stsfld: 2023
                            //    ldtoken: 622
                            //    ldsflda: 24
                            if (instruction.Operand is IMethod methodOperand)
                            {
                                CallInfo newInfo;
                                if (cacheInfo == null)
                                {
                                    newInfo = new CallInfo(
                                        new AssemblyInfo(module.Assembly.Name, module.Assembly.Version),
                                        new MethodUniqueName(methodDef),
                                        instruction.OpCode,
                                        methodDef.IsPublicGlobalVisibility(),
                                        methodDef.FindOverrides().Select(md => new MethodUniqueName(md)).ToList());
                                    cacheInfo = newInfo;
                                }
                                else
                                {
                                    newInfo = new CallInfo(
                                        cacheInfo.AssemblyInfo,
                                        cacheInfo.Signature,
                                        instruction.OpCode,
                                        cacheInfo.IsPublic,
                                        cacheInfo.OverrideSignatures);
                                }
                                
                                // TODO PERF add cache for AssemblyInfo
                                var assemblyInfo = new AssemblyInfo(
                                    methodOperand.DeclaringType.DefinitionAssembly.Name,
                                    methodOperand.DeclaringType.DefinitionAssembly.Version);
                                var key = new MethodUniqueName(methodOperand);
                                if (callers.TryGetValue(key, out var callInfoMap))
                                {
                                    if (callInfoMap.TryGetValue(assemblyInfo, out var list))
                                    {
                                        list.Add(newInfo);
                                    }
                                    else
                                    {
                                        callInfoMap.Add(assemblyInfo, new List<CallInfo>(){newInfo});
                                    }
                                }
                                else
                                {
                                    callers.Add(key, new Dictionary<AssemblyInfo, List<CallInfo>>()
                                    {
                                        {assemblyInfo, new List<CallInfo>(){newInfo}}
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}