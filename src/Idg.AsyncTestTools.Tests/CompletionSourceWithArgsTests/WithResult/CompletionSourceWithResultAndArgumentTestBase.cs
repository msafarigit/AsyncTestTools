using System;

namespace Idg.AsyncTest.Tests.CompletionSourceWithArgsTests.WithResult
{
    public class CompletionSourceWithResultAndArgumentTestBase
    {
        protected readonly Arg ArgOne = new Arg("One");
        protected readonly Arg ArgTwo = new Arg("Two");
        protected readonly Arg ArgThree = new Arg("Three");
        protected const string ResultOne = "One";
        protected const string ResultTwo = "Two";
        protected readonly Exception ErrorOne = new Exception("One");
        protected readonly Exception ErrorTwo = new Exception("Two");

        public CompletionSourceWithResultAndArgumentTestBase()
        {
            Source = new CompletionSourceWithArgs<Arg, string>();
        }

        protected CompletionSourceWithArgs<Arg, string> Source { get; }

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
