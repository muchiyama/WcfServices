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
    public class WcfClient : IWcfClient<DataContainer>
    {
        private static IAppLogger m_appLogger = new WcfConsoleLogger();
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

        private HostType m_hostType;
        private HostType m_sendTo;
        private IWcfLogger m_logger = new WcfConsoleLogger();
        private ConfigrationCommon m_config;
        private ConfigrationCommon m_sendToConfig;
        private IClientChannel m_Clientchannel = null;
        public WcfClient(HostType hostType, HostType sendTo)
        {
            m_hostType = hostType;
            m_sendTo = sendTo;
            m_config = ConfigrationFactory.GetConfig(hostType);
            m_sendToConfig = ConfigrationFactory.GetConfig(sendTo);
        }

        public void Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, m_sendToConfig.Ip, m_sendToConfig.Port, (Action<DataContainer>)service.Action);
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

                if (m_config.SendBurderingInterval > 0) Burdening.Wait(m_config.SendBurderingInterval);

                ISimplexService proxy = (ISimplexService)m_Clientchannel;
                SendDataInternal(proxy, proxy.Action, container, ConfigrationCommon.Config.TimeOut);
            }

            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public void SendData(DataContainer container, int miliSeconds)
        {
            try
            {
                m_logger.Logging(container);

                if (m_config.SendBurderingInterval > 0) Burdening.Wait(m_config.SendBurderingInterval);

                ISimplexService proxy = (ISimplexService)m_Clientchannel;
                SendDataInternal(proxy, proxy.Action, container, ConfigrationCommon.Config.TimeOut + new TimeSpan(0, 0, 0, 0, miliSeconds));
            }

            catch (Exception ex)
            {
                m_appLogger.Logging($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public string Status
            => m_Clientchannel.State.ToString();

        public HostType HostType => m_hostType;
        public HostType SendTo => m_sendTo;
    }
    public interface IWcfClient<TContinaer> where TContinaer : IContainer
    {
        void Open();
        void SendData(TContinaer container);
        void SendData(TContinaer container, int miliSeconds);
        HostType HostType { get; }
        HostType SendTo { get; }
    }
}
