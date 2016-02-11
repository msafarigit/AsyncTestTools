using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenGetTaskCalledOnceBeforeSingleWaits : CompletionSourceNoResultTestBase
    {
        private Task[] _waits;

        public WhenGetTaskCalledOnceBeforeSingleWaits()
        {
            Source.GetTask();
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
        public void CallCountIsOne()
        {
            Assert.Equal(1, Source.CallCount);
        }
    }
}
