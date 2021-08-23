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
        public static void Burden(int milisecound)
        {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        Parallel.ForEach(new List<int> { 1, 2 }, async l =>
                        {
                            // 2loops
                            await Task.Delay(milisecound);
                        });
                        await Task.Delay(ConfigrationCommon.Config.WaitIntervalForSendTimer);
                    }  
                });
        }

        public static void Wait(int milisecound)
        {
            var limit = new TimeSpan(0, 0, 0, 0, milisecound);
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            while (stopWatch.Elapsed < limit)
            {

            }
        }
    }
}
