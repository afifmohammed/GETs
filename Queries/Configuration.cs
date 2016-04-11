using System;
using System.Collections.Generic;

namespace Queries
{
    public class QueryConfiguration<TConnection> where TConnection : IDisposable
    {
        Func<TConnection> ConnectionFactory;

        public QueryConfiguration(Func<TConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public QueryConfiguration<TConnection> Register<TInput, TOutput>(
            Func<TInput, TConnection, IEnumerable<TOutput>> function)
            where TOutput : class
        {
            QueryRoutes.Routes.Add(Function.ToKvp(function, ProvideConnection));
            return this;
        }

        Func<TInput, IEnumerable<TResult>> ProvideConnection<TInput, TResult>(
            Func<TInput, TConnection, IEnumerable<TResult>> query)
            where TResult : class
        {
            return input =>
            {
                using (var c = ConnectionFactory())
                    return query(input, c);
            };
        }
    }

    static class QueryRoutes
    {
        public static readonly List<KeyValuePair<FunctionContract, Func<object, IEnumerable<object>>>> Routes =
            new List<KeyValuePair<FunctionContract, Func<object, IEnumerable<object>>>>();
    }
}
