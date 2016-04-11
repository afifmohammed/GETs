using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    public static class Return
    {
        public static Func<TIn, TDependency, IEnumerable<TOut>> List<TIn, TDependency, TOut>(Func<TIn, TDependency, TOut> function)
        {
            return (i, d) => new[] { function(i, d) };
        }
    }

    static class Extensions
    {
        public static string FriendlyName(this Type type)
        {
            if (type.GetGenericArguments().Length == 0)
            {
                return type.Name;
            }
            var genericArguments = type.GetGenericArguments();
            var typeDefeninition = type.Name;
            var unmangledName = typeDefeninition.Substring(0, typeDefeninition.IndexOf("`"));
            return unmangledName + "(of " + String.Join(",", genericArguments.Select(FriendlyName)) + ")";
        }

        public static TypeContract Contract(this Type t)
        {
            return new TypeContract(t);
        }

        public static TypeContract Contract(this object t)
        {
            return new TypeContract(t);
        }        
    }
}
