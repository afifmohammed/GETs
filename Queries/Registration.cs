﻿using System;
using System.Collections.Generic;

namespace Queries
{
    public class QueryRegistration<TConnection> where TConnection : IDisposable
    {
        readonly Func<TConnection> _connectionFactory;

        public QueryRegistration(Func<TConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public QueryRegistration<TConnection> Register<TInput, TOutput>(
            Func<TInput, TConnection, TOutput> function,
            Func<Func<TInput, TConnection, TOutput>, Func<TInput, TConnection, IEnumerable<TOutput>>> list)
            where TOutput : class
        {
            QueryRoutes.Routes.Add(Function.ToKvp(list(function), ProvideConnection));
            return this;
        }

        public QueryRegistration<TConnection> Register<TInput, TOutput>(
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
                using (var c = _connectionFactory())
                    return query(input, c);
            };
        }
    }

    static class QueryRoutes
    {
        public static readonly List<KeyValuePair<FunctionContract, object>> Routes =
            new List<KeyValuePair<FunctionContract, object>>();
    }
}
