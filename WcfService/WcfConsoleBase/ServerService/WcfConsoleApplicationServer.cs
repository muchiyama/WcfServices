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
    public interface IWcfConsoleApplicationServer
    {
        void Start();
    }

    public class WcfConsoleApplicationServer : IWcfConsoleApplicationServer
    {
        public static WcfConsoleApplicationServer Create(IWcfServer server, HostType hostType)
            => new WcfConsoleApplicationServer(server, hostType);

        private IWcfServer m_server;
        private HostType m_hostType;
        private IAppLogger m_appLogger = new WcfConsoleLogger();
        private ConfigrationCommon m_config;

        private WcfConsoleApplicationServer(IWcfServer server, HostType hostType)
        {
            m_server = server;
            m_hostType = hostType;
            m_config = ConfigrationFactory.GetConfig(hostType);
        }

        public void Start()
        {
            if (m_config.BurderingInterval > 0) Burdening.Burden(m_config.BurderingInterval);
            m_appLogger.Logging($"wating for {m_hostType} server starting service.....");
            m_server.Start();
            m_appLogger.Logging($"start to server service {m_hostType}");
        }
    }
}
