using System;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.WithResult
{
    public class CompletionSourceWithResultTestBase
    {
        protected const string ResultOne = "One";
        protected const string ResultTwo = "Two";
        protected readonly Exception ErrorOne = new Exception("One");
        protected readonly Exception ErrorTwo = new Exception("Two");

        public CompletionSourceWithResultTestBase()
        {
            Source = new CompletionSource<string>();
        }

        protected CompletionSource<string> Source { get; }
    }
}
