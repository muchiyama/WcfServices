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

namespace WcfClientRRRRR
{
    class Program
    {
        private static readonly ILogger m_logger = new WcfConsoleLogger();
        private static readonly ConfigrationCommon m_config = ConfigrationFactory.GetConfig(HostType.RRRRR);
        internal static readonly HostType m_HostType = HostType.RRRRR;
        internal static List<IClientTaskExecuter> ServiceCollection { get; set; } = new List<IClientTaskExecuter>();
        private static ConcurrentDictionary<HostType, WcfClient> Clients = new ConcurrentDictionary<HostType, WcfClient>();
        private static WcfServer Server = new WcfServer(m_HostType);
        internal static System.Timers.Timer m_timer;
        private static CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            await StartUp();
            ServiceCollection.ForEach(f => f.Start());

            while (!TokenSource.IsCancellationRequested)
                await Task.Delay(10000);
        }

        static void ExecuteAsyncWithTimer()
        {
            m_timer = new System.Timers.Timer(m_config.WaitIntervalForTimer);
            m_timer.Elapsed += (sender, e) =>
            {
                Parallel.ForEach(Clients, c =>
                {
                    c.Value.SendData(DataContainerRepository.Create(m_HostType, RequestType.NORMAL));
                });

                m_logger.Logging($"server running on {Server.Stauts()} status...");
                m_logger.Logging($"client{Clients.FirstOrDefault().Key}: {Clients.FirstOrDefault().Value.Status}");
                m_logger.Logging($"client{Clients.LastOrDefault().Key}: {Clients.LastOrDefault().Value.Status}");
            };

            m_timer.Start();
        }

        static async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Parallel.ForEach(Clients, c =>
                {
                    c.Value.SendData(DataContainerRepository.Create(m_HostType, RequestType.NORMAL));
                });

                m_logger.Logging($"server running on {Server.Stauts()} status...");
                m_logger.Logging($"client{Clients.FirstOrDefault().Key}: {Clients.FirstOrDefault().Value.Status}");
                m_logger.Logging($"client{Clients.LastOrDefault().Key}: {Clients.LastOrDefault().Value.Status}");
                await Task.Delay(m_config.WaitIntervalForTimer);
            }
        }
        static async Task StartUp()
        {
            Server.Start();

            Console.WriteLine("IWcfServerRRRRR server started");
            Console.WriteLine("wating for IWcfServerRRRRR starting service.....");
            await Task.Delay(10000);
            Console.WriteLine("start to service IWcfServerRRRRR");

            new List<HostType> { HostType.TTTTT01, HostType.TTTTT02 }.ForEach(type =>
            {
                while (true)
                {
                    var client = new WcfClient(type);
                    client.Open();
                    var result = Clients.TryAdd(type, client);
                    if (result) break;
                }
            });

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                Server.Dispose();
                ShutDown();
            };

            if (m_config.BurderingFlag) Burdening.Burden();
            if (m_config.BurderingEndlessMultiThreading) Burdening.BurdeningWithEndlessMultiThreading();
        }

        [STAThread]
        static void ShutDown()
        {
            Environment.Exit(0);
        }

        public interface IClientTaskExecuter
        {
            void Start();
        }

        public class SnedSimpleMessageTimerExecuter : IClientTaskExecuter
        {
            public void Start()
            {
                Program.ExecuteAsyncWithTimer();
            }
        }

        public class SnedSimpleMessageWorkerExecuter : IClientTaskExecuter
        {
            public async void Start()
            {
                await Program.ExecuteAsync(Program.TokenSource.Token);
            }
        }
    }
}
