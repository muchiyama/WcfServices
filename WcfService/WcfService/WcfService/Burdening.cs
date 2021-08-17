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
                Parallel.ForEach(new List<int> { 1, 2 }, l =>
                {

                });

                await Task.Delay(ConfigrationCommon.Config.WaitForExecuteAsync);
            });
        }

        public static void CreateManyMultiThreadTask()
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
                        int worker;
                        int completion;
                        ThreadPool.GetAvailableThreads(out worker, out completion);
                        m_logger.Logging($"[{Thread.CurrentThread.ManagedThreadId}] | worker : {worker} | completion : {completion}");
                        Thread.Sleep(10000);
                    });
                }
            });
        }
    }
}
