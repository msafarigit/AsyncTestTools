using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class WhenOneExceptionSuppliedBeforeTaskFetched : CompletionSourceNoResultAndArgumentTestBase
    {
        private Task[] _tasks;

        public WhenOneExceptionSuppliedBeforeTaskFetched()
        {
            Source.SupplyException(ErrorOne);
            _tasks = new[]
            {
                Source.GetTask(ArgOne),
                Source.GetTask(ArgTwo)
            };
        }

        [Fact]
        public void FirstTaskProducesException()
        {
            Assert.True(_tasks[0].ContinueWith(_ => { }).Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Same(ErrorOne, _tasks[0].Exception.InnerException);
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
