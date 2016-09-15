using System;
using System.Collections.Generic;
using System.Linq;

namespace Tracer
{
    internal sealed class ThreadTraceInfo
    {
        private readonly Stack<MethodTraceInfo> _callStack;
        private readonly List<MethodTraceInfo> _tracedMethods;

        internal ThreadTraceInfo()
        {
            _callStack = new Stack<MethodTraceInfo>();
            _tracedMethods = new List<MethodTraceInfo>();
        }

        internal void StartMethodTrace(MethodTraceInfo methodTraceInfo)
        {
            if (_callStack.Count == 0)
            {
                _tracedMethods.Add(methodTraceInfo);
            }
            else
            {
                _callStack.Peek().AddNestedCall(methodTraceInfo);
            }
            _callStack.Push(methodTraceInfo);
        }

        internal void StopMethodTrace()
        {
            if (_callStack.Count == 0)
            {
                throw new InvalidOperationException("can't finish method trace: empty call stack");
            }
            _callStack.Peek().StopMethodTrace();
            _callStack.Pop();
        }

        internal IEnumerable<MethodTraceInfo> TracedMethods => _tracedMethods;
        internal long ExecutionTime => _tracedMethods.Select(x => x.ExecutionTime).Sum();
    }
}