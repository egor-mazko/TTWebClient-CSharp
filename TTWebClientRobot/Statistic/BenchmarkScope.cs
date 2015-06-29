using System;
using System.Diagnostics;

namespace TTWebClientRobot.Statistic
{
    public class BenchmarkScope : IDisposable
    {
        private readonly Benchmark _benchmark;
        private readonly Stopwatch _stopwatch;

        public BenchmarkScope(Benchmark benchmark)
        {
            _benchmark = benchmark;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        #region IDisposable implementation

        // Disposed flag.
        private bool _disposed;

        // Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingManagedResources)
        {
            // The idea here is that Dispose(Boolean) knows whether it is 
            // being called to do explicit cleanup (the Boolean is true) 
            // versus being called due to a garbage collection (the Boolean 
            // is false). This distinction is useful because, when being 
            // disposed explicitly, the Dispose(Boolean) method can safely 
            // execute code using reference type fields that refer to other 
            // objects knowing for sure that these other objects have not been 
            // finalized or disposed of yet. When the Boolean is false, 
            // the Dispose(Boolean) method should not execute code that 
            // refer to reference type fields because those objects may 
            // have already been finalized."

            if (!_disposed)
            {
                if (disposingManagedResources)
                {
                    // Dispose managed resources here...
                    _stopwatch.Stop();
                    _benchmark.Update(_stopwatch.ElapsedMilliseconds);
                }

                // Dispose unmanaged resources here...

                // Set large fields to null here...

                // Mark as disposed.
                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~BenchmarkScope()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
