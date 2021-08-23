using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WcfService
{
    public static class Burdening
    {
        static private ILogger m_logger = new WcfConsoleLogger();
        public static void Burden()
        {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        Parallel.ForEach(new List<int> { 1, 2 }, async l =>
                        {
                            // 2loops
                            await Task.Delay(1000);
                        });
                        await Task.Delay(ConfigrationCommon.Config.WaitIntervalForTimer);
                    }  
                });
        }

        public static void Wait10Secound()
        {
            var limit = new TimeSpan(0, 0, 0, 10);
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            while (stopWatch.Elapsed < limit)
            {

            }
        }

        public static void BurdeningWithEndlessMultiThreading()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var list = new List<int>();
                    for (var i = 0; i < 10000000; i++)
                    {
                        list.Add(i);
                    }

                    Parallel.ForEach(list, f =>
                    {
                        var limit = new TimeSpan(0, 0, 0, 10);
                        var stopWatch = new System.Diagnostics.Stopwatch();
                        stopWatch.Start();

                        while (stopWatch.Elapsed < limit)
                        {

                        }
                    });
                }
            });
        }
    }
}
