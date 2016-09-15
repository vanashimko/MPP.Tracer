using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Tracer
{
    public sealed class XmlTraceResultFormatter : ITraceResultFormatter
    {
        private Stream _outStream;

        public XmlTraceResultFormatter(Stream outStream)
        {
            if (outStream == null)
            {
                throw new ArgumentNullException(nameof(outStream));
            }
            _outStream = outStream;
        }

        public void Format(TraceResult traceResult)
        {
            if (traceResult == null)
            {
                throw new ArgumentNullException(nameof(traceResult));
            }

            var document = new XDocument();
            var root = new XElement("root");

            foreach (KeyValuePair<int, ThreadTraceInfo> threadTraceInfo in traceResult.ThreadsTraceInfo)
            {
                var threadElementInfo = new XElement("thread");
                threadElementInfo.Add(new XAttribute("id", threadTraceInfo.Key));
                threadElementInfo.Add(new XAttribute("time", threadTraceInfo.Value.ExecutionTime));

                foreach (MethodTraceInfo methodTraceInfo in threadTraceInfo.Value.TracedMethods)
                {
                    threadElementInfo.Add(MethodTraceInfoToXElement(methodTraceInfo));
                }

                root.Add(threadElementInfo);
            }

            document.Add(root);
            document.Save(_outStream);
        }

        // Static internals

        private static XElement MethodTraceInfoToXElement(MethodTraceInfo methodTraceInfo)
        {
            var result = new XElement("method");
            result.Add(new XAttribute("name", methodTraceInfo.Name));
            result.Add(new XAttribute("time", methodTraceInfo.ExecutionTime));
            result.Add(new XAttribute("class", methodTraceInfo.ClassName));
            result.Add(new XAttribute("params", methodTraceInfo.ParametersCount));

            foreach (MethodTraceInfo nestedMethodTraceInfo in methodTraceInfo.NestedCalls)
            {
                result.Add(MethodTraceInfoToXElement(nestedMethodTraceInfo));
            }

            return result;
        }
    }
}
