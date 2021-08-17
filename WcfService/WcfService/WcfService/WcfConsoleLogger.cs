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
        string IWcfFormatter.Format(DataContainer container)
            => $"[{System.Threading.Thread.CurrentThread.ManagedThreadId}] | Id=[{container.Id}] | CmmunicationType=[{container.CommunicationType}] | Sender=[{container.HostType}] | SendTime=[{container.SendTime} | ResponseTime=[{container.ResponseTime}] | SendOffset=[{container.SendTimeOffset} | ResponseOffset=[{container.ResponseTimeOffset}]]";
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
