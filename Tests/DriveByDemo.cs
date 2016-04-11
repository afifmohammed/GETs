using System;
using System.Collections.Generic;
using System.Linq;
using Queries;
using Xunit;

namespace Tests
{
    public class DriveByDemo
    {
        class Connection<T> : IDisposable
        {
            public Connection(List<T> list)
            {
                List = list;
            }

            public List<T> List { get; private set; }

            public void Dispose()
            {
                List = null;
            }
        }

        class StartingWith : Request<string>
        {
            public string Prefix;
        }

        class GivenLength : Request<string>
        {
            public int Length { get; set; }
        }

        class Total
        {
            public Total(int value)
            {
                Value = value;
            }
            public readonly int Value;
        }

        class TotalWordsForGivenLength : Request<Total>
        {
            public int Length { get; set; }
        }

        static IEnumerable<string> ByLength(GivenLength query, Connection<string> connection)
        {
            return connection.List.Where(x => x.Length == query.Length);
        }

        static IEnumerable<string> ByPrefix(StartingWith query, Connection<string> connection)
        {
            return connection.List.Where(x => x.StartsWith(query.Prefix));
        }

        static Total TotalByLength(TotalWordsForGivenLength query, Connection<string> connection)
        {
            return new Total(ByLength(new GivenLength { Length = query.Length }, connection).Count());
        }

        [Fact]
        public void WorksOutOfTheBox()
        {
            var list = new[] { "gone", "gost", "goose", "guava" }.ToList();
            var anotherList = new[] { "great", "gofer" }.ToList();

            new QueryRegistration<Connection<string>>(() => new Connection<string>(list))
                .Register<StartingWith, string>(ByPrefix)
                .Register<GivenLength, string>(ByLength);

            new QueryRegistration<Connection<string>>(() => new Connection<string>(anotherList))
                .Register<StartingWith, string>(ByPrefix)
                .Register<TotalWordsForGivenLength, Total>(TotalByLength, Return.List);

            Assert.Equal(
                new[] { "gone", "gost", "goose", "gofer" }
                    .OrderBy(x => x)
                    .ToList(), 
                Query<string>.By(new StartingWith { Prefix = "go" })
                    .OrderBy(x => x)
                    .ToList());

            Assert.Equal(
                new[] { "goose", "guava" }
                    .OrderBy(x => x)
                    .ToList(),
                Query<string>.By(new GivenLength { Length = 5 })
                    .OrderBy(x => x)
                    .ToList());

            Assert.Equal(
                2, 
                Query<Total>.By(new TotalWordsForGivenLength { Length = 5 })
                    .Single()
                    .Value);
        }
    }
}
