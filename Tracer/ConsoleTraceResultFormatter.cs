using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public sealed class ConsoleTraceResultFormatter : ITraceResultFormatter
    {
        public void Format(TraceResult traceResult)
        {
            if (traceResult == null)
            {
                throw new ArgumentNullException(nameof(traceResult));
            }

            foreach (KeyValuePair<int, ThreadTraceInfo> threadTraceInfo in traceResult.ThreadsTraceInfo)
            {
                Console.WriteLine($"Thread id: {threadTraceInfo.Key}, execution time: {threadTraceInfo.Value.ExecutionTime}");
                foreach (MethodTraceInfo tracedMethod in threadTraceInfo.Value.TracedMethods)
                {
                    PrintMethodTraceInfo(tracedMethod);
                }
            }
        }

        private void PrintMethodTraceInfo(MethodTraceInfo methodTraceInfo, int nestingLevel = 0)
        {
            int indentLength = nestingLevel*4 + 1;
            string indent = string.Format($"{{0, {indentLength}}}", string.Empty);

            Console.WriteLine($"{indent}Method: {methodTraceInfo.Name}");
            Console.WriteLine($"{indent}Class: {methodTraceInfo.ClassName}");
            Console.WriteLine($"{indent}Tracing time: {methodTraceInfo.ExecutionTime} ms");
            Console.WriteLine($"{indent}Parameters count: {methodTraceInfo.ParametersCount}");

            foreach (MethodTraceInfo nestedMethodTraceInfo in methodTraceInfo.NestedCalls)
            {
                PrintMethodTraceInfo(nestedMethodTraceInfo, nestingLevel + 1);
            }
        }
    }
}
