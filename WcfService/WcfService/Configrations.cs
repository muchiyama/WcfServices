using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService
{
    public enum TargetRunningEnvironment
    {
        Windows = 1,
        DockerWindowsKernel = 2,
        DockerLinuxKernel = 3,
    }
    public interface IConfigration
    {
    }
    public class ConfigrationCommon : IConfigration
    {
        public static ConfigrationCommon Config
            => Configration;
        private static ConfigrationCommon Configration { get; set; } = new ConfigrationCommon();
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 5);
        public int WaitForExecuteAsync => 5000;
        public bool BurderingFlag => true;
        internal static TargetRunningEnvironment RunningEnv => TargetEnv == "docker" ? TargetRunningEnvironment.DockerWindowsKernel : TargetRunningEnvironment.Windows;
        internal static string TargetEnv => Environment.GetEnvironmentVariable("runninngEnv");
    }
    public class ConfigrationCCCCC : IConfigration
    {
        public static ConfigrationCCCCC Config
            => Configration;
        private static ConfigrationCCCCC Configration { get; set; } = new ConfigrationCCCCC();

        private ConfigrationCCCCC() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.11";
        public int Port => 6782;
    }
    public class ConfigrationRRRRR : IConfigration
    {
        public static ConfigrationRRRRR Config
            => Configration;
        private static ConfigrationRRRRR Configration { get; set; } = new ConfigrationRRRRR();

        private ConfigrationRRRRR() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.12";
        public int Port => 6783;
    }
    public class ConfigrationTTTTT01 : IConfigration
    {
        public static ConfigrationTTTTT01 Config
            => Configration;
        private static ConfigrationTTTTT01 Configration { get; set; } = new ConfigrationTTTTT01();
        private ConfigrationTTTTT01() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.13";
        public int Port => 6780;
    }
    public class ConfigrationTTTTT02 : IConfigration
    {
        public static ConfigrationTTTTT02 Config
            => Configration;
        private static ConfigrationTTTTT02 Configration { get; set; } = new ConfigrationTTTTT02();
        private ConfigrationTTTTT02() { }
        public TimeSpan TimeOut => new TimeSpan(0, 0, 0, 10);
        public string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.14";
        public int Port => 6781;
    }
}
