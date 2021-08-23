using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcfConsoleBase.ClientService;
using WcfConsoleBase.ServerService;
using WcfService;
using WcfService.Client;
using WcfService.Contract.Structure;
using WcfService.Server;

namespace WcfClientRRRRR
{
    class Program
    {
        private static IWcfConsoleApplicationServer m_server;
        private static IWcfConsoleApplicationClient m_client;
        internal static readonly HostType m_hostType = HostType.RRRRR;
        private static CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            await StartUp();
            m_client.Start();
            while (!TokenSource.IsCancellationRequested)
                await Task.Delay(10000);
        }

        static async Task StartUp()
        {
            var wcfServer = new WcfServer(m_hostType);
            var logger = new WcfConsoleLogger();
            m_server = WcfConsoleApplicationServer.Create(
                    server: wcfServer,
                    hostType: m_hostType
                );

            m_server.Start();

            await Task.Delay(10000);

            m_client = WcfConsoleApplicationClient.Create(
                    hostType: m_hostType
                ).AddClient(new WcfClient(m_hostType, HostType.TTTTT01))
                 .AddClient(new WcfClient(m_hostType, HostType.TTTTT02))
                 .AddActionExecutor(new WcfSendableWithTimer());
                 //.AddActionExecutor(new WcfSendableAdditionalWithRandomInterval());

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                //Server.Dispose();
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
