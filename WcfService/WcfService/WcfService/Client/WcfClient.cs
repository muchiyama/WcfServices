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
    public class WcfClient : IWcfClientTTTTTToCCCCC, IWcfClientTTTTTToRRRRR, IWcfClientCCCCCToTTTTT01, IWcfClientCCCCCToTTTTT02, IWcfClientRRRRRToTTTTT01, IWcfClientRRRRRToTTTTT02
    {
        private IClientChannel m_Clientchannel = null;
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
                ic.OperationTimeout = ConfigrationCCCCC.Config.TimeOut;
                ic.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        internal void SendDataInternal<TType>(ISimplexService proxy, Action<TType> action, TType param, TimeSpan timeout)
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
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        public WcfClient()
        {
        }

        void IWcfClientTTTTTToCCCCC.Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, ConfigrationCCCCC.Config.Ip, ConfigrationCCCCC.Config.Port, (Action<DataContainer>)service.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        void IWcfClientTTTTTToRRRRR.Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, ConfigrationRRRRR.Config.Ip, ConfigrationRRRRR.Config.Port, (Action<DataContainer>)service.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        void IWcfClientCCCCCToTTTTT01.Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, ConfigrationTTTTT01.Config.Ip, ConfigrationTTTTT01.Config.Port, (Action<DataContainer>)service.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
        void IWcfClientCCCCCToTTTTT02.Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, ConfigrationTTTTT02.Config.Ip, ConfigrationTTTTT02.Config.Port, (Action<DataContainer>)service.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        void IWcfClientRRRRRToTTTTT01.Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, ConfigrationTTTTT01.Config.Ip, ConfigrationTTTTT01.Config.Port, (Action<DataContainer>)service.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
        void IWcfClientRRRRRToTTTTT02.Open()
        {
            try
            {
                ISimplexService service = new SimplexService();
                OpenClientChannelInternal(ref m_Clientchannel, ConfigrationTTTTT02.Config.Ip, ConfigrationTTTTT02.Config.Port, (Action<DataContainer>)service.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
        public void SendData(DataContainer container)
        {
            try
            {
                ISimplexService proxy = (ISimplexService)m_Clientchannel;
                SendDataInternal(proxy, proxy.Action, container, ConfigrationCommon.Config.TimeOut);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
    }

    public interface IWcfClientTTTTTToCCCCC
    {
        void Open();
    }
    public interface IWcfClientTTTTTToRRRRR
    {
        void Open();
    }
    public interface IWcfClientCCCCCToTTTTT01
    {
        void Open();
    }
    public interface IWcfClientCCCCCToTTTTT02
    {
        void Open();
    }
    public interface IWcfClientRRRRRToTTTTT01
    {
        void Open();
    }
    public interface IWcfClientRRRRRToTTTTT02
    {
        void Open();
    }
}
