using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.WithResult
{
    public class WhenOneResultSuppliedBeforeTaskFetched : CompletionSourceWithResultAndArgumentTestBase
    {
        private Task<string>[] _tasks;

        public WhenOneResultSuppliedBeforeTaskFetched()
        {
            Source.SupplyResult(ResultOne);
            _tasks = new[]
            {
                Source.GetTask(ArgOne),
                Source.GetTask(ArgTwo)
            };
        }

        [Fact]
        public void FirstTaskProducesResult()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Equal(ResultOne, _tasks[0].Result);
        }

        [Fact]
        public void SecondTaskIsNotComplete()
        {
            Assert.False(_tasks[1].IsCompleted);
        }

        [Fact]
        public void ReportsTwoArguments()
        {
            Assert.Equal(2, Source.Arguments.Count);
        }

        [Fact]
        public void RecordsFirstArgument()
        {
            Assert.Same(ArgOne, Source.Arguments[0]);
        }

        [Fact]
        public void RecordsSecondArgument()
        {
            Assert.Same(ArgTwo, Source.Arguments[1]);
        }
    }
}
