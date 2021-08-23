using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService;
using WcfService.Contract.Structure;
using WcfService.Server;

namespace WcfConsoleBase.ServerService
{
    public interface ISercerConsoleService
    {

    }

    public class ServerConsoleService
    {
        public static ServerConsoleService Create(IWcfServer server, IWcfRecievable<DataContainer> reciever, IWcfLogger wcfLogger, ILogger appLogger, HostType hostType)
            => new ServerConsoleService(server, reciever, wcfLogger, appLogger, hostType);

        private IWcfServer m_server;
        private IWcfRecievable<DataContainer> m_reciever;
        private IWcfLogger m_wcfLogger;
        private ILogger m_appLogger;
        private ConfigrationCommon m_config;

        private ServerConsoleService(IWcfServer server, IWcfRecievable<DataContainer> reciever, IWcfLogger wcfLogger, ILogger appLogger, HostType hostType)
        {
            m_server = server;
            m_reciever = reciever;
            m_wcfLogger = wcfLogger;
            m_appLogger = appLogger;
            m_config = ConfigrationFactory.GetConfig(hostType);
        }

        private async Task StartUp()
        {
            m_server.Start();

            Console.WriteLine("IWcfServerTTTTT01 server started");
            Console.WriteLine("wating for IWcfServerTTTTT01 starting service.....");
            await Task.Delay(10000);
            Console.WriteLine("start to service IWcfServerTTTTT01");
        }
    }
}
