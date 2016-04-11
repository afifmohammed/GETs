using System;
using System.Collections.Generic;

namespace Queries
{
    static class Function
    {
        public static KeyValuePair<FunctionContract, Func<object, IEnumerable<object>>> ToKvp<TInput, TResult, TDependency>(
            Func<TInput, TDependency, IEnumerable<TResult>> function,
            Func<Func<TInput, TDependency, IEnumerable<TResult>>, Func<TInput, IEnumerable<TResult>>> provideDependency)
            where TResult : class
        {
            return new KeyValuePair<FunctionContract, Func<object, IEnumerable<object>>>(
                new FunctionContract(typeof(TInput).Contract(), typeof(TResult).Contract()),
                Downcast(provideDependency(function)));
        }

        public static KeyValuePair<FunctionContract, Func<object, IEnumerable<object>>> ToKvp<TInput, TResult>(
            Func<TInput, IEnumerable<TResult>> function)
            where TResult : class
        {
            return new KeyValuePair<FunctionContract, Func<object, IEnumerable<object>>>(
                new FunctionContract(typeof(TInput).Contract(), typeof(TResult).Contract()),
                Downcast(function));                
        }

        public static IEnumerable<TResult> Cast<T, TResult>(
            IEnumerable<T> collection) 
            where TResult : class
        {
            foreach (var item in collection)
            {
                yield return item as TResult;
            }
        }

        static Func<object, IEnumerable<object>> Downcast<TInput, TResult>(
            Func<TInput, IEnumerable<TResult>> query) 
            where TResult : class
        {
            return c => Cast<TResult, object>(query((TInput)c));                
        }        
    }
}
