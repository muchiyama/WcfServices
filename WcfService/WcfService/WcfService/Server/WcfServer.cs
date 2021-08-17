using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfService.Contract;
using WcfService.Contract.Abstruct;
using WcfService.Contract.Interfaces;
using WcfService.Contract.Structure;

namespace WcfService.Server
{
    public class WcfServer : IWcfServerTTTTT01, IWcfServerTTTTT02, IWcfServerCCCCC, IWcfServerRRRRR
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

        private ServiceHost m_serviceHost = null;

        public void Recieve(DataContainer container)
        {
            try
            {
                Console.WriteLine(container.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
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
