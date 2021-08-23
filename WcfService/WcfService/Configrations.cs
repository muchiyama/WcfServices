using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService.Contract.Structure;

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
        internal static ConfigrationCommon Config
            => Configration;
        private static ConfigrationCommon Configration { get; set; } = new ConfigrationCommon();
        internal static TargetRunningEnvironment RunningEnv => TargetEnv == "docker" ? TargetRunningEnvironment.DockerWindowsKernel : TargetRunningEnvironment.Windows;
        internal static string TargetEnv => Environment.GetEnvironmentVariable("runninngEnv");
        public virtual TimeSpan TimeOut => new TimeSpan(0, 0, 10, 5);
        public virtual TimeSpan TimeOutForAdditionalSend => new TimeSpan(0, 0, 10, 30);
        public virtual int WaitIntervalForSendTimer => 5000;
        public virtual int WaitIntervalForAdditionalSend => 5000;
        public virtual string Ip => string.Empty;
        public virtual int Port => -1;
        public virtual int BurderingInterval => 0;
        public virtual int SendBurderingInterval => 0;
        public virtual int RecieveBurderingInterval => 0;
    }
    public class ConfigrationCCCCC : ConfigrationCommon
    {
        internal static new ConfigrationCCCCC Config
            => Configration;
        private static ConfigrationCCCCC Configration { get; set; } = new ConfigrationCCCCC();
        private ConfigrationCCCCC() { }
        public override string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.11";
        public override int Port => 6782;
    }
    public class ConfigrationRRRRR : ConfigrationCommon
    {
        internal static new ConfigrationRRRRR Config
            => Configration;
        private static ConfigrationRRRRR Configration { get; set; } = new ConfigrationRRRRR();

        private ConfigrationRRRRR() { }
        public override string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.12";
        public override int Port => 6783;
    }
    public class ConfigrationTTTTT01 : ConfigrationCommon
    {
        internal static new ConfigrationTTTTT01 Config
            => Configration;
        private static ConfigrationTTTTT01 Configration { get; set; } = new ConfigrationTTTTT01();
        private ConfigrationTTTTT01() { }
        public override string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.13";
        public override int Port => 6780;
    }
    public class ConfigrationTTTTT02 : ConfigrationCommon
    {
        internal static new ConfigrationTTTTT02 Config
            => Configration;
        private static ConfigrationTTTTT02 Configration { get; set; } = new ConfigrationTTTTT02();
        private ConfigrationTTTTT02() { }
        public override string Ip => ConfigrationCommon.RunningEnv == TargetRunningEnvironment.Windows ? "localhost" : "197.168.10.14";
        public override int Port => 6781;
    }

    public static class ConfigrationFactory
    {
        public static ConfigrationCommon GetConfig(HostType type)
        {
            switch (type)
            {
                case HostType.CCCCC:
                    return ConfigrationCCCCC.Config;
                case HostType.RRRRR:
                    return ConfigrationRRRRR.Config;
                case HostType.TTTTT01:
                    return ConfigrationTTTTT01.Config;
                case HostType.TTTTT02:
                    return ConfigrationTTTTT02.Config;
                default:
                    return null;
            }
        }
    }
}
