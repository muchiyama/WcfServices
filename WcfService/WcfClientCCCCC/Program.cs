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

namespace WcfClientCCCCC
{
    class Program
    {
        static private ILogger m_logger = new WcfConsoleLogger();
        static async Task Main(string[] args)
        {
            await StartUp();
            await ExecuteAsync(TokenSource.Token);
        }

        static async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (ConfigrationCommon.Config.BurderingFlag) Burdening.Burden();

            while (!stoppingToken.IsCancellationRequested)
            {
                Parallel.ForEach(Clients, c =>
                {
                    var container = new DataContainer()
                    {
                        Id = Guid.NewGuid(),
                        CommunicationType = CommunicationType.REQUEST,
                        HostType = HostType.CCCCC,
                        SendTo = c.Key == typeof(IWcfClientToTTTTT01) ? HostType.TTTTT01 : HostType.TTTTT02,
                        SendTime = DateTime.Now,
                    };

                    c.Value.SendData(container);
                });

                m_logger.Logging($"server running on {Server.Stauts()} status...");
                m_logger.Logging($"client{Clients.FirstOrDefault().Key}: {Clients.FirstOrDefault().Value.Status}");
                m_logger.Logging($"client{Clients.LastOrDefault().Key}: {Clients.LastOrDefault().Value.Status}");
                await Task.Delay(ConfigrationCommon.Config.WaitForExecuteAsync);
            }
        }

        static async Task StartUp()
        {
            ((IWcfServerCCCCC)Server).Start();

            Console.WriteLine("server started.....");
            Console.WriteLine("wating for starting service.....");
            await Task.Delay(10000);
            Console.WriteLine("start to service CCCCC");

            while (true)
            {
                var client = new WcfClient();
                ((IWcfClientToTTTTT01)client).Open();
                var result = Clients.TryAdd(typeof(IWcfClientToTTTTT01), client);
                if (result) break;
            }
            while (true)
            {
                var client = new WcfClient();
                ((IWcfClientToTTTTT02)client).Open();
                var result = Clients.TryAdd(typeof(IWcfClientToTTTTT02), client);
                if (result) break;
            }
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                Server.Dispose();
                ShutDown();
            };
        }

        [STAThread]
        static void ShutDown()
        {
            Environment.Exit(0);
        }

        private static ConcurrentDictionary<Type, WcfClient> Clients = new ConcurrentDictionary<Type, WcfClient>();
        private static WcfServer Server = new WcfServer(HostType.CCCCC);
        private static CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
    }
}
