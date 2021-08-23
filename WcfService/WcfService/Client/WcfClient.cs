using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfService.Contract;
using WcfService.Contract.Interfaces;
using WcfService.Contract.Structure;

namespace WcfService.Client
{
    public class WcfClient : IWcfClient, IWcfSendable<DataContainer>, IWcfSendableWithCloneDataContainer<DataContainer>
    {
        private static ILogger m_appLogger = new WcfConsoleLogger();
        internal static void OpenClientChannelInternal<TType>(ref IClientChannel ic, string ip, int port, Action<TType> service)
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;
                ISimplexService proxy = new ChannelFactory<ISimplexService>(binding).CreateChannel(
                        new EndpointAddress($"net.tcp://{ip}:{port}/sample.wcf.service/{service.Method.Name}")
                    );
                ic = proxy as IClientChannel;
                ic.OperationTimeout = ConfigrationCommon.Config.TimeOut;
                ic.Open();
            }
            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        internal static void SendDataInternal<TType>(ISimplexService proxy, Action<TType> action, TType param, TimeSpan timeout)
        {
            try
            {
                IClientChannel ic = proxy as IClientChannel;
                ic.OperationTimeout = timeout;
                var proxyType = proxy.GetType();
                var mi = proxyType.GetMethod(action.Method.Name);
                mi.Invoke(proxy, new object[] { param });
            }
            catch (Exception ex)
            {
                m_appLogger.Logging(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private IWcfLogger m_logger = new WcfConsoleLogger();
        private ConfigrationCommon m_config;
        private IClientChannel m_Clientchannel = null;
        public WcfClient(HostType sendTo)
        {
            m_config = ConfigrationFactory.GetConfig(sendTo);
        }

        public void Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, m_config.Ip, m_config.Port, (Action<DataContainer>)service.Action);
            }
            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public void SendData(DataContainer container)
        {
            try
            {
                m_logger.Logging(container);
                ISimplexService proxy = (ISimplexService)m_Clientchannel;
                SendDataInternal(proxy, proxy.Action, container, ConfigrationCommon.Config.TimeOut);
            }

            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public void SendDataWithCloneDataContainer(DataContainer container)
        {
            try
            {
                var cpContainer = container.Clone();

                m_logger.Logging(cpContainer);
                ISimplexService proxy = (ISimplexService)m_Clientchannel;
                SendDataInternal(proxy, proxy.Action, cpContainer, m_config.TimeOut + m_config.AdditionalTimeOutWhenSendWithClone);
            }

            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public string Status
            => m_Clientchannel.State.ToString();
    }
    public interface IWcfClient
    {
        void Open();
    }
    public interface IWcfSendable<TContinaer> where TContinaer : IContainer
    {
        void SendData(TContinaer container);
    }

    public interface IWcfSendableWithCloneDataContainer<TContinaer> where TContinaer : IContainer
    {
        void SendDataWithCloneDataContainer(TContinaer container);
    }
}
