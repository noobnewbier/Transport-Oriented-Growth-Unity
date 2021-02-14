using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TransportOrientedGrowthTree.Di
{
    internal static class ReflectionUtils
    {
        public static IEnumerable<MethodInfo> GetMethodsByAttribute(Type type, Type attributeType)
        {
            if (!attributeType.IsSubclassOf(typeof(Attribute))) throw new ArgumentNullException($"{attributeType.FullName} is not an attribute");

            return type.GetMethods().Where(m => m.GetCustomAttribute(attributeType, false) != null);
        }

        public static MethodInfo GetMethodByAttribute(Type type, Type attributeType)
        {
            var methods = GetMethodsByAttribute(type, attributeType);

            var methodInfos = methods.ToList();
            if (methodInfos.Count > 1)
                throw new ArgumentException($"There is more than one method with type: {type.FullName} with the attribute: {attributeType.FullName}");

            if (!methodInfos.Any())
                throw new ArgumentException($"There is no method with type: {type.FullName} with the attribute: {attributeType.FullName}");

            return methodInfos.First();
        }
    }
}