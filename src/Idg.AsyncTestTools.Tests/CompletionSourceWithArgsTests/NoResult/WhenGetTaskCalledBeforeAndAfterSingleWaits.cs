using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class WhenGetTaskCalledBeforeAndAfterSingleWaits : CompletionSourceNoResultAndArgumentTestBase
    {
        private Task[] _waits;

        public WhenGetTaskCalledBeforeAndAfterSingleWaits()
        {
            Source.GetTask(ArgOne);
            _waits = new[]
            {
                Source.WaitAsync(),
                Source.WaitAsync(),
                Source.WaitAsync()
            };
            Source.GetTask(ArgTwo);
        }

        [Fact]
        public void FirstWaitCompletes()
        {
            Assert.True(_waits[0].Wait(TimeSpan.FromSeconds(2)));
        }

        [Fact]
        public void SecondWaitCompletes()
        {
            Assert.True(_waits[1].Wait(TimeSpan.FromSeconds(2)));
        }

        [Fact]
        public void FurtherWaitsAreNotComplete()
        {
            Assert.False(_waits[2].IsCompleted);
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
