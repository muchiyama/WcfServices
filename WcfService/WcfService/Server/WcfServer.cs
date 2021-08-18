using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfService.Client;
using WcfService.Contract;
using WcfService.Contract.Abstruct;
using WcfService.Contract.Interfaces;
using WcfService.Contract.Structure;

namespace WcfService.Server
{
    public class WcfServer : IWcfServerTTTTT01, IWcfServerTTTTT02, IWcfServerCCCCC, IWcfServerRRRRR, IDisposable
    {
        internal static void StartServiceInternal<TType>(ref ServiceHost serviceHost, ASimplexService service, string ip, int port, Action<TType> action)
        {
            serviceHost = new ServiceHost(service);
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            serviceHost.AddServiceEndpoint(typeof(ISimplexService), binding, $"net.tcp://{ip}:{port}/sample.wcf.service/{action.Method.Name}");
            serviceHost.Faulted += (sender, args) => Console.WriteLine("faulted");
            serviceHost.Open();
        }

        internal void CreateClientsIfNotExist(DataContainer container)
        {
            switch (container.HostType)
            {
                case HostType.CCCCC:
                    if (!Clients.ContainsKey(typeof(IWcfClientToCCCCC)))
                        while (true)
                        {
                            var client = new WcfClient();
                            ((IWcfClientToCCCCC)client).Open();
                            var isSuccecced = Clients.TryAdd(typeof(IWcfClientToCCCCC), client);
                            if (isSuccecced) break;
                        }
                    break;
                case HostType.RRRRR:
                    if (!Clients.ContainsKey(typeof(IWcfClientToRRRRR)))
                        while (true)
                        {
                            var client = new WcfClient();
                            ((IWcfClientToRRRRR)client).Open();
                            var isSuccecced = Clients.TryAdd(typeof(IWcfClientToRRRRR), client);
                            if (isSuccecced) break;
                        }
                    break;
                case HostType.TTTTT01:
                    if (!Clients.ContainsKey(typeof(IWcfClientToTTTTT01)))
                        while (true)
                        {
                            var client = new WcfClient();
                            ((IWcfClientToTTTTT01)client).Open();
                            var isSuccecced = Clients.TryAdd(typeof(IWcfClientToTTTTT01), client);
                            if (isSuccecced) break;
                        }
                    break;
                case HostType.TTTTT02:
                    if (!Clients.ContainsKey(typeof(IWcfClientToTTTTT02)))
                        while (true)
                        {
                            var client = new WcfClient();
                            ((IWcfClientToTTTTT02)client).Open();
                            var isSuccecced = Clients.TryAdd(typeof(IWcfClientToTTTTT02), client);
                            if (isSuccecced) break;
                        }
                    break;
            }
        }

        internal static void RecieveInternal(DataContainer container, WcfServer server)
        {
            switch (container.CommunicationType)
            {
                case CommunicationType.REQUEST:
                    WcfClient client = null;
                    if (container.HostType == HostType.CCCCC) client = FetchWcfClient(typeof(IWcfClientToCCCCC), server);
                    else if (container.HostType == HostType.RRRRR) client = FetchWcfClient(typeof(IWcfClientToRRRRR), server);
                    else if (container.HostType == HostType.TTTTT01) client = FetchWcfClient(typeof(IWcfClientToTTTTT01), server);
                    else if (container.HostType == HostType.TTTTT01) client = FetchWcfClient(typeof(IWcfClientToTTTTT02), server);

                    container.CommunicationType = CommunicationType.RESPONSE;
                    container.SendTo = container.HostType;
                    container.HostType = server.m_hostType;
                    container.ResponseTime = DateTime.Now;

                    client.SendData(container);
                    break;
                case CommunicationType.RESPONSE:
                    if (DateTime.Now - container.SendTime > ConfigrationCommon.Config.TimeOut)
                        throw new InvalidAsynchronousStateException($"リクエストから5秒の間レスポンスがありませんでした。 --- {((IWcfFormatter)server.m_logger).Format(container)}");
                    break;
                default:
                    Console.WriteLine("do nothing");
                    break;
            }
        }

        internal static WcfClient FetchWcfClient(Type type, WcfServer server)
        {
            while (true)
            {
                WcfClient client;
                var isSucceeded = server.Clients.TryGetValue(type, out client);
                if (isSucceeded) return client;
            }
        }

        private IWcfLogger m_logger = new WcfConsoleLogger();
        private ILogger m_appLogger = new WcfConsoleLogger();
        private ServiceHost m_serviceHost = null;
        private HostType m_hostType = HostType.None;
        internal ConcurrentDictionary<Type, WcfClient> Clients = new ConcurrentDictionary<Type, WcfClient>();

        public WcfServer(HostType hostType)
        {
            m_hostType = hostType;
        }

        public void Recieve(DataContainer container)
        {
            try
            {
                m_logger.Logging(container);

                CreateClientsIfNotExist(container);
                RecieveInternal(container, this);
            }
            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public string Stauts()
        {
            return m_serviceHost.State.ToString();
        }

        void IWcfServerTTTTT01.Start()
        {
            SimplexService simplexService = new SimplexService();
            simplexService.RaiseEventHandler = Recieve;
            StartServiceInternal(ref m_serviceHost, simplexService, ConfigrationTTTTT01.Config.Ip, ConfigrationTTTTT01.Config.Port, (Action<DataContainer>)simplexService.Action);
        }

        void IWcfServerTTTTT02.Start()
        {
            SimplexService simplexService = new SimplexService();
            simplexService.RaiseEventHandler = Recieve;
            StartServiceInternal(ref m_serviceHost, simplexService, ConfigrationTTTTT02.Config.Ip, ConfigrationTTTTT02.Config.Port, (Action<DataContainer>)simplexService.Action);
        }

        void IWcfServerCCCCC.Start()
        {
            SimplexService simplexService = new SimplexService();
            simplexService.RaiseEventHandler = Recieve;
            StartServiceInternal(ref m_serviceHost, simplexService, ConfigrationCCCCC.Config.Ip, ConfigrationCCCCC.Config.Port, (Action<DataContainer>)simplexService.Action);
        }
        void IWcfServerRRRRR.Start()
        {
            SimplexService simplexService = new SimplexService();
            simplexService.RaiseEventHandler = Recieve;
            StartServiceInternal(ref m_serviceHost, simplexService, ConfigrationRRRRR.Config.Ip, ConfigrationRRRRR.Config.Port, (Action<DataContainer>)simplexService.Action);
        }

        public void Dispose()
        {
            m_serviceHost.Close();
        }
    }

    public interface IWcfServerTTTTT01
    {
        void Start();
    }

    public interface IWcfServerTTTTT02
    {
        void Start();
    }

    public interface IWcfServerCCCCC
    {
        void Start();
    }

    public interface IWcfServerRRRRR
    {
        void Start();
    }
}
