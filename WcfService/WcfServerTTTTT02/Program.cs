using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcfService;
using WcfService.Client;
using WcfService.Contract.Structure;
using WcfService.Server;

namespace WcfServerTTTTT02
{
    class Program
    {
        static private ILogger m_logger = new WcfConsoleLogger();
        static private ConfigrationCommon m_config = ConfigrationFactory.GetConfig(HostType.TTTTT02);
        static async Task Main(string[] args)
        {
            await StartUp();
            await ExecuteAsync(TokenSource.Token);
        }

        static async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                m_logger.Logging($"server running on {Server.Stauts()} status...");
                await Task.Delay(m_config.WaitIntervalForTimer);
            }
        }

        static async Task StartUp()
        {
            Server.Start();

            Console.WriteLine("IWcfServerTTTTT02 server started");
            Console.WriteLine("wating for IWcfServerTTTTT02 starting service.....");
            await Task.Delay(10000);
            Console.WriteLine("start to service IWcfServerTTTTT02");
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                Server.Dispose();
                ShutDown();
            };

            if(m_config.BurderingFlag) Burdening.Burden();
            if (m_config.BurderingEndlessMultiThreading) Burdening.BurdeningWithEndlessMultiThreading();
        }

        [STAThread]
        static void ShutDown()
        {
            Environment.Exit(0);
        }

        private static WcfServer Server = new WcfServer(HostType.TTTTT02);
        private static CancellationTokenSource TokenSource = new CancellationTokenSource();
    }
}
