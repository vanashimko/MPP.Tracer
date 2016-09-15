using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Tracer
{
    public sealed class TraceResult
    {
        private readonly ConcurrentDictionary<int, ThreadTraceInfo> _threadsTraceInfo;

        internal TraceResult()
        {
            _threadsTraceInfo = new ConcurrentDictionary<int, ThreadTraceInfo>();
        }

        internal void StartMethodTrace(int threadId, MethodBase method)
        {
            ThreadTraceInfo threadsTraceInfo = _threadsTraceInfo.GetOrAdd(threadId, new ThreadTraceInfo());
            threadsTraceInfo.StartMethodTrace(new MethodTraceInfo(method));
        }

        internal void StopMethodTrace(int threadId)
        {
            ThreadTraceInfo threadsTraceInfo;
            if (!_threadsTraceInfo.TryGetValue(threadId, out threadsTraceInfo))
            {
                throw new ArgumentException("invalid thread id");
            }
            threadsTraceInfo.StopMethodTrace();
        }

        internal IEnumerable<KeyValuePair<int, ThreadTraceInfo>> ThreadsTraceInfo => _threadsTraceInfo;
    }
}
