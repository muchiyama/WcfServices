using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcfConsoleBase.ServerService;
using WcfService;
using WcfService.Client;
using WcfService.Contract.Structure;
using WcfService.Server;

namespace WcfServerTTTTT01
{
    class Program
    {
        private static IWcfConsoleApplicationServer m_server;
        private static HostType m_hostType = HostType.TTTTT01;
        private static CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            StartUp();
            await ExecuteAsync(TokenSource.Token);
        }

        static async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(10000);
        }
        static void StartUp()
        {
            m_server = WcfConsoleApplicationServer.Create(
                    server: new WcfServer(m_hostType),
                    hostType:m_hostType
                );
            m_server.Start();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                ShutDown();
            };
        }

        [STAThread]
        static void ShutDown()
        {
            Environment.Exit(0);
        }
    }
}
