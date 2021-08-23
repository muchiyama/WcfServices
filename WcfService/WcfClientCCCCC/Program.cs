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
        static readonly internal ILogger m_logger = new WcfConsoleLogger();
        static readonly internal ConfigrationCommon m_config = ConfigrationFactory.GetConfig(HostType.CCCCC);
        static readonly internal HostType m_HostType = HostType.CCCCC;
        internal static ConcurrentDictionary<HostType, WcfClient> m_clients = new ConcurrentDictionary<HostType, WcfClient>();
        internal readonly static WcfServer m_server = new WcfServer(m_HostType);
        internal static System.Timers.Timer m_timer;
        internal static CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();
        internal static  List<IClientTaskExecuter> ServiceCollection {  get; set; } = new List<IClientTaskExecuter>();
        static async Task Main(string[] args)
        {
            await StartUp();
            ServiceCollection.ForEach(f => f.Start());

            while (!TokenSource.IsCancellationRequested)
                await Task.Delay(10000);
        }
        internal static void ExecuteAsyncWithTimer()
        {
            m_timer = new System.Timers.Timer(m_config.WaitIntervalForTimer);
            m_timer.Elapsed += (sender, e) =>
            {
                Parallel.ForEach(m_clients, c =>
                {
                    c.Value.SendData(DataContainerRepository.Create(m_HostType, RequestType.NORMAL));
                });

                m_logger.Logging($"server running on {m_server.Stauts()} status...");
                m_logger.Logging($"client{m_clients.FirstOrDefault().Key}: {m_clients.FirstOrDefault().Value.Status}");
                m_logger.Logging($"client{m_clients.LastOrDefault().Key}: {m_clients.LastOrDefault().Value.Status}");
            };

            m_timer.Start();
        }

        internal static async void SendAdditionalMessage(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Parallel.ForEach(m_clients, async c =>
                {
                    if (c.Key == HostType.TTTTT01)
                        await Task.Run(() => c.Value.SendDataWithCloneDataContainer(DataContainerRepository.Create(m_HostType, RequestType.ADDITIONAL)));
                    else
                        c.Value.SendDataWithCloneDataContainer(DataContainerRepository.Create(m_HostType, RequestType.ADDITIONAL));
                });

                m_logger.Logging($"server running on {m_server.Stauts()} status...");
                m_logger.Logging($"client{m_clients.FirstOrDefault().Key}: {m_clients.FirstOrDefault().Value.Status}");
                m_logger.Logging($"client{m_clients.LastOrDefault().Key}: {m_clients.LastOrDefault().Value.Status}");
                await Task.Delay(m_config.WaitIntervalForTimer);
            }
        }

        internal static async Task StartUp()
        {
            m_server.Start();

            Console.WriteLine("IWcfServerCCCCC server started.....");
            Console.WriteLine("wating for IWcfServerCCCCC starting service.....");
            await Task.Delay(10000);
            Console.WriteLine("start to service IWcfServerCCCCC");

            new List<HostType> { HostType.TTTTT01, HostType.TTTTT02 }.ForEach(type =>
            {
                while (true)
                {
                    var client = new WcfClient(type);
                    client.Open();
                    var result = m_clients.TryAdd(type, client);
                    if (result) break;
                }
            });

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                m_server.Dispose();
                ShutDown();
            };

            ServiceCollection.Add(new SnedSimpleMessageTimerExecuter());

            if (m_config.BurderingFlag) Burdening.Burden();
            if (m_config.BurderingEndlessMultiThreading) Burdening.BurdeningWithEndlessMultiThreading();
        }

        [STAThread]
        static void ShutDown()
        {
            Environment.Exit(0);
        }
    }

    public interface IClientTaskExecuter
    {
        void Start();
    }

    public class SnedSimpleMessageTimerExecuter : IClientTaskExecuter
    {
        public void Start()
            => Program.ExecuteAsyncWithTimer();
    }

    public class SnedSimpleMessageWorkerExecuter : IClientTaskExecuter
    {
        public void Start()
            => Program.SendAdditionalMessage(Program.TokenSource.Token);
    }

    public class SnedAdditionalMessageExecuter : IClientTaskExecuter
    {
        public void Start()
            => Program.SendAdditionalMessage(Program.TokenSource.Token);
    }
}
