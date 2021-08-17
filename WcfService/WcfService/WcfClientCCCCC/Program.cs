using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcfService.Client;
using WcfService.Contract.Structure;
using WcfService.Server;

namespace WcfClientCCCCC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartUp();
            await ExecuteAsync(TokenSource.Token);
        }

        static async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Parallel.ForEach(Clients, c =>
                {
                    var container = new DataContainer()
                    {
                        Id = Guid.NewGuid(),
                        CommunicationType = CommunicationType.REQUEST,
                        HostType = HostType.CCCCC,
                        ExecutionTime = DateTime.Now,
                    };

                    c.Value.SendData(container);
                });

                await Task.Delay(5000);
            }
        }

        static async Task StartUp()
        {
            ((IWcfServerCCCCC)Server).Start();

            Console.WriteLine("server started");
            await Task.Delay(5000);
            Console.WriteLine("press any key to start communication");
            Console.ReadLine();

            while (true)
            {
                var client = new WcfClient();
                ((IWcfClientCCCCCToTTTTT01)client).Open();
                var result = Clients.TryAdd(typeof(IWcfClientCCCCCToTTTTT01), client);
                if (result) break;
            }
            while (true)
            {
                var client = new WcfClient();
                ((IWcfClientCCCCCToTTTTT02)client).Open();
                var result = Clients.TryAdd(typeof(IWcfClientCCCCCToTTTTT02), client);
                if (result) break;
            }

            Console.CancelKeyPress += (sender, eventArgs) => TokenSource.Cancel();
        }

        private static ConcurrentDictionary<Type, WcfClient> Clients = new ConcurrentDictionary<Type, WcfClient>();
        private static WcfServer Server = new WcfServer();
        private static CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
    }
}
