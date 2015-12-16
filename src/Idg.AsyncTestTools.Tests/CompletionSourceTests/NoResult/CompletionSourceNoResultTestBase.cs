using System;

namespace Idg.AsyncTest.Tests.CompletionSourceTests.NoResult
{
    public class CompletionSourceNoResultTestBase
    {
        protected readonly Exception ErrorOne = new Exception("One");
        protected readonly Exception ErrorTwo = new Exception("Two");

        public CompletionSourceNoResultTestBase()
        {
            Source = new CompletionSource();
        }

        protected CompletionSource Source { get; }
    }
}
