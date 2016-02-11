using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenGetTaskCalledBeforeAndAfterSingleWaits : CompletionSourceNoResultTestBase
    {
        private Task[] _waits;

        public WhenGetTaskCalledBeforeAndAfterSingleWaits()
        {
            Source.GetTask();
            _waits = new[]
            {
                Source.WaitAsync(),
                Source.WaitAsync(),
                Source.WaitAsync()
            };
            Source.GetTask();
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
        public void CallCountIsTwo()
        {
            Assert.Equal(2, Source.CallCount);
        }
    }
}
