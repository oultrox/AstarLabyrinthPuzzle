using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleBus.Internal
{
    public static class PredefinedAssemblyUtil
    {
        enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass
        }


        static AssemblyType? GetAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                _ => null
            };
        }

        public static List<Type> GetTypes(Type interfaceType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            List<Type> types = new List<Type>();

            for (int i = 0; i < assemblies.Length; i++)
            {
                AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
                if (assemblyType != null)
                {
                    assemblyTypes.Add((AssemblyType) assemblyType, assemblies[i].GetTypes());
                }
            }
            
            if (assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var csharpTypes))
                AddTypesFromAssembly(csharpTypes, interfaceType, types);

            if (assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharpFirstPass, out var firstPassTypes))
                AddTypesFromAssembly(firstPassTypes, interfaceType, types);

            return types;
        }

        static void AddTypesFromAssembly(Type[] assemblies, Type interfaceType,  ICollection<Type> types)
        {
            if (assemblies == null) return;

            for (int i = 0; i < assemblies.Length; i++)
            {
                Type type = assemblies[i];

                if (type != interfaceType && interfaceType.IsAssignableFrom(type))
                {
                    types.Add(type);
                }
            }
        }
    }
}