using System;
using System.Collections.Generic;

namespace Queries
{
    static class Function
    {
        public static KeyValuePair<FunctionContract, object> ToKvp<TInput, TResult, TDependency>(
            Func<TInput, TDependency, IEnumerable<TResult>> function,
            Func<Func<TInput, TDependency, IEnumerable<TResult>>, Func<TInput, IEnumerable<TResult>>> provideDependency)
            where TResult : class
        {
            return new KeyValuePair<FunctionContract, object>(
                new FunctionContract(typeof(TInput).Contract(), typeof(TResult).Contract()),
                Downcast(provideDependency(function)) as object);
        }

        public static KeyValuePair<FunctionContract, object> ToKvp<TInput, TResult>(
            Func<TInput, IEnumerable<TResult>> function)
            where TResult : class
        {
            return new KeyValuePair<FunctionContract, object>(
                new FunctionContract(typeof(TInput).Contract(), typeof(TResult).Contract()),
                Downcast(function));                
        }

        static Func<object, IEnumerable<TResult>> Downcast<TInput, TResult>(
            Func<TInput, IEnumerable<TResult>> query) 
            where TResult : class
        {
            return c => query((TInput)c);                
        }        
    }
}
