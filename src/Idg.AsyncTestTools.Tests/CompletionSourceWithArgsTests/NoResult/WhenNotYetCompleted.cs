using System.Threading.Tasks;
using Xunit;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class WhenNotYetCompleted : CompletionSourceNoResultAndArgumentTestBase
    {
        private Task[] _tasks;

        public WhenNotYetCompleted()
        {
            _tasks = new[]
            {
                Source.GetTask(ArgOne),
                Source.GetTask(ArgTwo)
            };
        }

        [Fact]
        public void FirstTaskIsNotComplete()
        {
            Assert.False(_tasks[0].IsCompleted);
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
