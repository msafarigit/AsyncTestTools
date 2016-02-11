using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class WhenGetTaskCalledBeforeAndAfterDoubleWait : CompletionSourceNoResultTestBase
    {
        private Task[] _waits;

        public WhenGetTaskCalledBeforeAndAfterDoubleWait()
        {
            Source.GetTask();
            _waits = new[]
            {
                Source.WaitAsync(2),
                Source.WaitAsync()
            };
            Source.GetTask();
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
        public void CallCountIsTwo()
        {
            Assert.Equal(2, Source.CallCount);
        }
    }
}
