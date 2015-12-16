using System;
using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class WhenOneResultSuppliedBeforeAndOneExceptionAfterTasksFetched : CompletionSourceNoResultAndArgumentTestBase
    {
        private Task[] _tasks;

        public WhenOneResultSuppliedBeforeAndOneExceptionAfterTasksFetched()
        {
            Source.Complete();
            _tasks = new[]
            {
                Source.GetTask(ArgOne),
                Source.GetTask(ArgTwo),
                Source.GetTask(ArgThree)
            };
            Source.SupplyException(ErrorTwo);
        }

        [Fact]
        public void FirstTaskCompletes()
        {
            Assert.True(_tasks[0].Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
        }

        [Fact]
        public void SecondTaskProducesException()
        {
            Assert.True(_tasks[1].ContinueWith(_ => { }).Wait(TimeSpan.FromSeconds(2)), "Timed out waiting for task");
            Assert.Same(ErrorTwo, _tasks[1].Exception.InnerException);
        }

        [Fact]
        public void ThirdTaskIsNotComplete()
        {
            Assert.False(_tasks[2].IsCompleted);
        }

        [Fact]
        public void ReportsThreeArguments()
        {
            Assert.Equal(3, Source.Arguments.Count);
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

        [Fact]
        public void RecordsThirdArgument()
        {
            Assert.Same(ArgThree, Source.Arguments[2]);
        }
    }
}
