using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer
{
    public sealed class Tracer : ITracer
    {
        private readonly TraceResult _traceResult;

        private static volatile Tracer _tracerInstance;
        private static readonly object SyncRoot = new object();

        private Tracer()
        {
            _traceResult = new TraceResult();
        }

        // Public

        public void StartTrace()
        {
            var stackTrace = new StackTrace(1);
            StackFrame currentFrame = stackTrace.GetFrame(0);
            MethodBase currentMethod = currentFrame.GetMethod();

            _traceResult.StartMethodTrace(Thread.CurrentThread.ManagedThreadId, currentMethod);
        }

        public void StopTrace()
        {
            _traceResult.StopMethodTrace(Thread.CurrentThread.ManagedThreadId);
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }

        // Static

        public static Tracer GetInstance()
        {
            if (_tracerInstance == null)
            {
                lock (SyncRoot)
                {
                    if (_tracerInstance == null)
                    {
                        _tracerInstance = new Tracer();
                    }
                }
            }

            return _tracerInstance;
        }
    }
}
