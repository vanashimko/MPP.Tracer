using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Tracer
{
    internal sealed class MethodTraceInfo
    {
        private readonly List<MethodTraceInfo> _nestedCalls;
        private readonly Stopwatch _stopwatch;

        // Internals
        
        internal string Name { get; }
        internal string ClassName { get; }
        internal int ParametersCount { get; }
        internal long ExecutionTime => _stopwatch.ElapsedMilliseconds;

        internal MethodTraceInfo(MethodBase method)
        {
            Name = method.Name;
            ClassName = method.DeclaringType.Name;
            ParametersCount = method.GetParameters().Length;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            _nestedCalls = new List<MethodTraceInfo>();
        }

        internal void StopMethodTrace()
        {
            _stopwatch.Stop();
        }

        internal void AddNestedCall(MethodTraceInfo methodTraceInfo)
        {
            _nestedCalls.Add(methodTraceInfo);            
        }

        internal IEnumerable<MethodTraceInfo> NestedCalls => _nestedCalls;
    }
}
