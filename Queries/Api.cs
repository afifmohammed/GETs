using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    public interface Request<TResult>
    {}

    public delegate IEnumerable<TResult> QueryFunction<TResult>(Request<TResult> input);
    
    public static class Query<TResult> where TResult : class
    {
        public static QueryFunction<TResult> By = 
            input => QueryRoutes.Routes
                .Where(r => r.Key.Equals(Contract(input)))
                .SelectMany(r => ((Func<object, IEnumerable<TResult>>)(r.Value))(input));

        static FunctionContract Contract(Request<TResult> input)
        {
            return new FunctionContract(input.Contract(), typeof(TResult).Contract());
        }        
    }

}
