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
    /// <summary>
    /// wcfサーバー用
    /// Interfaceは各起動設定用に用意
    /// </summary>
    public class WcfServer : IWcfServer, IWcfRecievable<DataContainer>, IDisposable
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
                    container.RecieveTime = DateTime.Now;

                    WcfClient client = null;
                    client = server.FetchWcfClient(container.HostType, server);
                    container = WriteResponse(container, server.m_hostType);

                    client.SendData(container);
                    break;
                case CommunicationType.RESPONSE:
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
            container.ResponseTime = DateTime.Now;

            return container;
        }

        private IWcfLogger m_logger = new WcfConsoleLogger();
        private ILogger m_appLogger = new WcfConsoleLogger();
        private ServiceHost m_serviceHost = null;
        private HostType m_hostType = HostType.None;
        private ConfigrationCommon m_config;

        internal ConcurrentDictionary<HostType, WcfClient> Clients = new ConcurrentDictionary<HostType, WcfClient>();
        internal void CreateClientsIfNotExist(DataContainer container)
        {
            if (!Clients.ContainsKey(container.HostType))
                while (true)
                {
                    var client = new WcfClient(container.HostType);
                    client.Open();
                    var isSuccecced = Clients.TryAdd(container.HostType, client);
                    if (isSuccecced) break;
                }
        }
        internal WcfClient FetchWcfClient(HostType type, WcfServer server)
        {
            while (true)
            {
                WcfClient client;
                var isSucceeded = server.Clients.TryGetValue(type, out client);
                if (isSucceeded) return client;
            }
        }
        public WcfServer(HostType hostType)
        {
            m_hostType = hostType;
            m_config = ConfigrationFactory.GetConfig(m_hostType);
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

        /// <summary>
        /// server status
        /// </summary>
        /// <returns></returns>
        public string Stauts()
        {
            return m_serviceHost.State.ToString();
        }

        public void Start()
        {
            SimplexService simplexService = new SimplexService();
            simplexService.RaiseEventHandler = Recieve;
            StartServiceInternal(ref m_serviceHost, simplexService, m_config.Ip, m_config.Port, (Action<DataContainer>)simplexService.Action);
        }

        public void Dispose()
        {
            m_serviceHost.Close();
        }
    }

    public interface IWcfServer
    {
        void Start();
    }

    public interface IWcfRecievable<TContainer> where TContainer : WcfService.Contract.Structure.IContainer
    {
        void Recieve(TContainer container);
    }
}
