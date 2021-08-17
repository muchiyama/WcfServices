using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService
{
    public interface IConfigration
    {
    }

    public class ConfigrationCommon : IConfigration
    {
        public static ConfigrationCommon Config
            => m_configration;
        private static ConfigrationCommon m_configration { get; set; } = new ConfigrationCommon();
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 5);
    }

    public class ConfigrationTTTTT01 : IConfigration
    {
        public static ConfigrationTTTTT01 Config
            => m_configration;
        private static ConfigrationTTTTT01 m_configration { get; set; } = new ConfigrationTTTTT01();
        private ConfigrationTTTTT01() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => "localhost";
        public int Port => 6780;
    }
    public class ConfigrationTTTTT02 : IConfigration
    {
        public static ConfigrationTTTTT02 Config
            => m_configration;
        private static ConfigrationTTTTT02 m_configration { get; set; } = new ConfigrationTTTTT02();
        private ConfigrationTTTTT02() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => "localhost";
        public int Port => 6781;
    }
    public class ConfigrationCCCCC : IConfigration
    {
        public static ConfigrationCCCCC Config
            => m_configration;
        private static ConfigrationCCCCC m_configration { get; set; } = new ConfigrationCCCCC();

        private ConfigrationCCCCC() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => "localhost";
        public int Port => 6782;
    }
    public class ConfigrationRRRRR : IConfigration
    {
        public static ConfigrationRRRRR Config
            => m_configration;
        private static ConfigrationRRRRR m_configration { get; set; } = new ConfigrationRRRRR();

        private ConfigrationRRRRR() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => "localhost";
        public int Port => 6783;
    }
}
