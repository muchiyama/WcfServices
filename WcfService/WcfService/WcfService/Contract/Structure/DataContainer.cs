using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService.Contract.Structure
{
    public class DataContainer : IContainer
    {
        public Guid Id{ get; set; }
        public CommunicationType CommunicationType { get; set; }
        public HostType HostType { get; set; }
        public DateTime ExecutionTime { get; set; }
    }

    public enum CommunicationType
    {
        REQUEST = 1,
        RESPONSE = 2,
        NOTIFY = 3,
    }

    public enum HostType
    {
        TTTTT01 = 1,
        TTTTT02 = 2,
        CCCCC = 3,
        RRRRR = 4,
    }
}
