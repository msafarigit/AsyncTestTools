using System;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.NoResult
{
    public class CompletionSourceNoResultAndArgumentTestBase
    {
        protected readonly Arg ArgOne = new Arg("One");
        protected readonly Arg ArgTwo = new Arg("Two");
        protected readonly Arg ArgThree = new Arg("Three");
        protected readonly Exception ErrorOne = new Exception("One");
        protected readonly Exception ErrorTwo = new Exception("Two");

        public CompletionSourceNoResultAndArgumentTestBase()
        {
            Source = new CompletionSourceWithArgs<Arg>();
        }

        protected CompletionSourceWithArgs<Arg> Source { get; }

        protected class Arg
        {
            private readonly string _value;
            public Arg(string value)
            {
                _value = value;
            }

            public override string ToString()
            {
                return _value;
            }
        }
    }
}
