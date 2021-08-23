using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService.Contract.Structure;

namespace WcfService
{
    public class WcfConsoleLogger : IWcfLogger, IWcfFormatter, ILogger
    {
        private IWcfFormatter formatter = new WcfFormatter();
        public string Format(DataContainer container)
            =>  formatter.Format(container);

        void IWcfLogger.Logging(DataContainer container)
        {
            Console.WriteLine(formatter.Format(container));
        }

        void ILogger.Logging(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class WcfFormatter : IWcfFormatter
    {
        private static System.Diagnostics.Stopwatch m_stopWatch = null;
        public WcfFormatter()
        {
            if(m_stopWatch == null)
            {
                m_stopWatch = new System.Diagnostics.Stopwatch();
                m_stopWatch.Start();
            }
        }

        string IWcfFormatter.Format(DataContainer container)
            => $"[{System.Threading.Thread.CurrentThread.ManagedThreadId}],Id=[{container.Id}],CmmunicationType=[{container.CommunicationType}],CmmunicationStatu=[{container.CommunicationStatus}],Sender=[{container.HostType}],SendTo=[{container.SendTo}],SendTime=[{container.SendTime.ToString("hh:mm:ss.ffff")}],SendOffset=[{container.SendTimeOffset}],RecieveTime=[{container.RecieveTime.ToString("hh:mm:ss.ffff")}],RecieveTimeOffSet=[{container.RecieveTimeOffSet}],ResponseTime=[{container.ResponseTime.ToString("hh:mm:ss.ffff")}],ResponseOffset=[{container.ResponseTimeOffset}],elapsedTime=[{m_stopWatch.Elapsed}]";
    }

    public interface ILogger
    {
        void Logging(string message);
    }

    public interface IWcfLogger
    {
        void Logging(DataContainer container);
    }

    public interface IWcfFormatter
    {
        string Format(DataContainer container);
    }
}
