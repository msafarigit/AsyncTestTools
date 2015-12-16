using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class WhenGetTaskCalledOnceBeforeSingleWaits : CompletionSourceNoResultAndArgumentTestBase
    {
        private Task[] _waits;

        public WhenGetTaskCalledOnceBeforeSingleWaits()
        {
            Source.GetTask(ArgOne);
            _waits = new[]
            {
                Source.WaitAsync(),
                Source.WaitAsync(),
                Source.WaitAsync()
            };
        }

        [Fact]
        public void FirstWaitCompletes()
        {
            Assert.True(_waits[0].Wait(TimeSpan.FromSeconds(2)));
        }

        [Fact]
        public void FurtherWaitsAreNotComplete()
        {
            Assert.False(_waits[1].IsCompleted);
            Assert.False(_waits[2].IsCompleted);
        }

        [Fact]
        public void ReportsOneArgument()
        {
            Assert.Equal(1, Source.Arguments.Count);
        }

        [Fact]
        public void RecordsFirstArgument()
        {
            Assert.Same(ArgOne, Source.Arguments[0]);
        }
    }
}
