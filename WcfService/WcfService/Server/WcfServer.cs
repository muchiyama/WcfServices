using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WcfService.Client;
using WcfService.Contract;
using WcfService.Contract.Abstruct;
using WcfService.Contract.Interfaces;
using WcfService.Contract.Structure;

namespace WcfService.Server
{
    public class WcfServer : IWcfServer
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

        internal static void RecieveInternal(DataContainer container, WcfServer server)
        {
            switch (container.CommunicationType)
            {
                case CommunicationType.REQUEST:
                    CreateClientsIfNotExist(container, server);
                    container = WriteResponse(container, server.m_hostType);

                    if (server.m_config.RecieveBurderingInterval > 0) Burdening.Wait(server.m_config.RecieveBurderingInterval);

                    WcfClient client = null;
                    client = server.FetchWcfClient(container.SendTo, server);

                    client.SendData(container);
                    break;
                case CommunicationType.RESPONSE:
                    container.CompletedTime = DateTime.Now;
                    container.CommunicationStatus = CommunicationStatus.COMPLETED;
                    server.m_logger.Logging(container);

                    if (DateTime.Now - container.SendTime > ConfigrationCommon.Config.TimeOut)
                        throw new InvalidAsynchronousStateException($"リクエストから5秒の間レスポンスがありませんでした。 --- {((IWcfFormatter)server.m_logger).Format(container)}");
                    break;
                default:
                    Console.WriteLine("do nothing");
                    break;
            }
        }

        internal static DataContainer WriteResponse(DataContainer container, HostType hostType)
        {
            container.CommunicationType = CommunicationType.RESPONSE;
            container.CommunicationStatus = CommunicationStatus.RECIEVED;
            container.SendTo = container.HostType;
            container.HostType = hostType;
            container.RecieveTime = DateTime.Now;

            return container;
        }

        private ConcurrentDictionary<HostType, WcfClient> m_clients = new ConcurrentDictionary<HostType, WcfClient>();
        private ServiceHost m_serviceHost = null;
        private HostType m_hostType = HostType.None;
        private ConfigrationCommon m_config;
        private readonly IWcfLogger m_logger = new WcfConsoleLogger();
        private readonly IAppLogger m_appLogger = new WcfConsoleLogger();

        public void Recieve(DataContainer container)
        {
            try
            {
                RecieveInternal(container, this);
            }
            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
        internal static void CreateClientsIfNotExist(DataContainer container, WcfServer server)
        {
            if (!server.m_clients.ContainsKey(container.HostType))
                while (true)
                {
                    var client = new WcfClient(server.m_hostType, container.HostType);
                    client.Open();
                    var isSuccecced = server.m_clients.TryAdd(container.HostType, client);
                    if (isSuccecced) break;
                }
        }
        internal WcfClient FetchWcfClient(HostType type, WcfServer server)
        {
            while (true)
            {
                WcfClient client;
                var isSucceeded = server.m_clients.TryGetValue(type, out client);
                if (isSucceeded) return client;
            }
        }
        public WcfServer(HostType hostType)
        {
            m_hostType = hostType;
            m_config = ConfigrationFactory.GetConfig(m_hostType);
        }
        public string Status()
            => m_serviceHost.State.ToString();
        public void Start()
        {
            SimplexService simplexService = new SimplexService();
            simplexService.RaiseEventHandler = Recieve;
            StartServiceInternal(ref m_serviceHost, simplexService, m_config.Ip, m_config.Port, (Action<DataContainer>)simplexService.Action);
        }
    }

    public interface IWcfServer
    {
        void Start();
    }
}
