using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Tracer;

namespace TracerTest
{
    class Program
    {
        private static readonly ITracer _tracer = Tracer.Tracer.GetInstance();

        static void Main(string[] args)
        {
            _tracer.StartTrace();

            var threads = new List<Thread>();

            for (int i = 0; i < 5; i++)
            {
                var thread = i%2 == 0 ? new Thread(LongCalculations) : new Thread(LongRecursiveCalculations);
                
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            _tracer.StopTrace();
            WriteResult();
        }

        static void LongRecursiveCalculations(object recursionLevelObj)
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            int recursionLevel = (int)(recursionLevelObj ?? 0);
            if (recursionLevel < 5)
            {
                LongRecursiveCalculations(recursionLevel + 1);
            }
            LongCalculations(10);

            _tracer.StopTrace();
        }

        static void LongCalculations(object o)
        {
            _tracer.StartTrace();

            for (int i = 0; i < 6; i++)
            {
                ShortCalculations();
            }

            Thread.Sleep(100);
            _tracer.StopTrace();
        }

        static void ShortCalculations()
        {
            _tracer.StartTrace();
            Thread.Sleep(10);

            var threads = new List<Thread>();

            for (int i = 0; i < 5; i++)
            {
                var thread = new Thread(ShortCalculations2);
                threads.Add(thread);
                thread.Start();
            }

            for (int i = 0; i < 5; i++)
            {
                threads[i].Join();
            }

            _tracer.StopTrace();
        }

        static void ShortCalculations2()
        {
            _tracer.StartTrace();
            Thread.Sleep(10);
            _tracer.StopTrace();
        }

        static void WriteResult()
        {
            ITraceResultFormatter formatter;
            TraceResult traceResult = _tracer.GetTraceResult();

            formatter = new ConsoleTraceResultFormatter();
            formatter.Format(_tracer.GetTraceResult());

            using (Stream fileStream = File.Create("TraceResult.xml"))
            {
                formatter = new XmlTraceResultFormatter(fileStream);
                formatter.Format(_tracer.GetTraceResult());
            }
        }
    }
}
