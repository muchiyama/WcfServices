using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcfService;
using WcfService.Client;
using WcfService.Contract.Interfaces;
using WcfService.Contract.Structure;

namespace WcfConsoleBase.ClientService
{
    public interface IWcfConsoleApplicationClient
    {
        void Start();
    }
    public class WcfConsoleApplicationClient : IWcfConsoleApplicationClient
    {
        public static WcfConsoleApplicationClient Create(HostType hostType)
            => new WcfConsoleApplicationClient(hostType);

        private HostType m_hostType;
        private ConfigrationCommon m_config;
        IWcfLogger m_wcfLogger = new WcfConsoleLogger();
        IAppLogger m_Applogger = new WcfConsoleLogger();
        private ConcurrentDictionary<HostType, IWcfClient<DataContainer>> m_clients = new ConcurrentDictionary<HostType, IWcfClient<DataContainer>>();
        private List<IWcfClientActioExecutor> m_actinExecutors = new List<IWcfClientActioExecutor>();

        private WcfConsoleApplicationClient(HostType hostType)
        {
            m_hostType = hostType;
            m_config = ConfigrationFactory.GetConfig(m_hostType);
        }

        public WcfConsoleApplicationClient AddClient(IWcfClient<DataContainer> client)
        {
            while (true)
            {
                client.Open();
                var result = m_clients.TryAdd(client.SendTo, client);
                if (result) break;
            }

            return this;
        }

        public WcfConsoleApplicationClient AddActionExecutor(IWcfClientActioExecutor executor)
        {
            m_actinExecutors.Add(executor);
            return this;
        }

        public void Start()
        {
            if (m_config.SendBurderingInterval > 0) Burdening.Wait(m_config.SendBurderingInterval);
            m_actinExecutors.ForEach(f =>
            {
                f.SendStart(m_clients, m_config.WaitIntervalForSendTimer);
            });
        }
    }

    public interface IWcfClientActioExecutor
    {
        void SendStart(ConcurrentDictionary<HostType, IWcfClient<DataContainer>> clients, int intervalMilisecounds);
    }

    public class WcfSendableWithTimer : IWcfClientActioExecutor
    {
        private static System.Timers.Timer StartTimerIfNotStarted(System.Timers.Timer timer, int intervalMilisecounds)
        {
            if (timer == null)
                timer = new System.Timers.Timer(intervalMilisecounds);

            return timer;
        }

        private System.Timers.Timer m_timer = null;
        public void SendStart(ConcurrentDictionary<HostType, IWcfClient<DataContainer>> clients, int intervalMilisecounds)
        {
            m_timer = StartTimerIfNotStarted(m_timer, intervalMilisecounds);
            m_timer.Elapsed += (sender, e) =>
            {
                Parallel.ForEach(clients, c =>
                {
                    c.Value.SendData(DataContainerRepository.Create(c.Value.HostType, c.Key, RequestType.NORMAL));
                });
            };
            m_timer.Start();
        }
    }

    public class WcfSendableAdditionalWithRandomInterval : IWcfClientActioExecutor
    {
        private Random m_random = new Random();
        public async void SendStart(ConcurrentDictionary<HostType, IWcfClient<DataContainer>> clients, int intervalMilisecounds)
        {
            while (true)
            {
                Parallel.ForEach(clients, async c =>
                {
                    if (c.Key == HostType.TTTTT01)
                        await Task.Run(() => c.Value.SendData(DataContainerRepository.Create(c.Value.HostType, c.Key, RequestType.ADDITIONAL), m_random.Next(0, 15000)));
                    else
                        c.Value.SendData(DataContainerRepository.Create(c.Value.HostType, c.Key, RequestType.ADDITIONAL), m_random.Next(0, 15000));
                });

                await Task.Delay(intervalMilisecounds + m_random.Next(0, 15000));
            }
        }
    }
}
