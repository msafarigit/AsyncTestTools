using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.WithResult
{
    public class WhenGetTaskCalledBeforeAndAfterDoubleWait : CompletionSourceWithResultAndArgumentTestBase
    {
        private Task[] _waits;

        public WhenGetTaskCalledBeforeAndAfterDoubleWait()
        {
            Source.GetTask(ArgOne);
            _waits = new[]
            {
                Source.WaitAsync(2),
                Source.WaitAsync()
            };
            Source.GetTask(ArgTwo);
        }

        [Fact]
        public void DoubleWaitCompletes()
        {
            Assert.True(_waits[0].Wait(TimeSpan.FromSeconds(2)));
        }

        [Fact]
        public void FurtherWaitsAreNotComplete()
        {
            Assert.False(_waits[1].IsCompleted);
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
