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
        public CommunicationStatus CommunicationStatus { get; set; }
        public HostType HostType { get; set; }
        public HostType SendTo { get; set; }
        public DateTime SendTime { get; set; }
        public TimeSpan SendTimeOffset => DateTime.Now - SendTime;
        public DateTime RecieveTime { get; set; }
        public TimeSpan RecieveTimeOffSet => DateTime.Now - RecieveTime;
        public DateTime ResponseTime { get; set; }
        public TimeSpan ResponseTimeOffset => DateTime.Now - ResponseTime;
    }

    public enum CommunicationType
    {
        REQUEST = 1,
        RESPONSE = 2,
        NOTIFY = 3,
    }

    public enum HostType
    {
        None = -1,
        TTTTT01 = 1,
        TTTTT02 = 2,
        CCCCC = 3,
        RRRRR = 4,
    }

    public enum CommunicationStatus
    {
        Sending = 1,
        Recieved = 2,
        Completed = 3,
    }

    public class DataContainerRepository : IDataContainerRepository
    {
        public static DataContainerRepository m_instance = new DataContainerRepository();
        public static DataContainer Create()
            => ((IDataContainerRepository)m_instance).Create();

        DataContainer IRepository<DataContainer>.Create()
            => new DataContainer()
                {
                    Id = Guid.NewGuid(),
                    CommunicationType = CommunicationType.REQUEST,
                    HostType = HostType.CCCCC,
                    SendTime = DateTime.Now,
                };
    }

    public interface IDataContainerRepository : IRepository<DataContainer>
    {
    }

    public interface IRepository<TType>
    {
        TType Create();
    }
}
