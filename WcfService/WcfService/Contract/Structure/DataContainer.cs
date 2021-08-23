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
        public RequestType RequestType {  get; set; }
        public HostType SendTo { get; set; }
        public DateTime SendTime { get; set; } = DateTime.MinValue;
        public TimeSpan SendTimeOffset => DateTime.Now - SendTime;
        public DateTime RecieveTime { get; set; } = DateTime.MinValue;
        public TimeSpan RecieveTimeOffSet => DateTime.Now - RecieveTime;
        public DateTime CompletedTime { get; set; } = DateTime.MinValue;
        public TimeSpan CompletedTimeOffset => DateTime.Now - CompletedTime;
        public TimeSpan DelayIntervalBetweenSendAndRecieve => RecieveTime - SendTime;

        public DataContainer Clone()
            => (DataContainer)MemberwiseClone();
    }

    public enum CommunicationType
    {
        NONE = -1,
        REQUEST = 1,
        RESPONSE = 2,
        NOTIFY = 3,
    }

    public enum RequestType
    {
        NONE = -1,
        NORMAL = 1,
        ADDITIONAL = 2,
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
        NONE = -1,
        SENDING = 1,
        RECIEVED = 2,
        COMPLETED = 3,
    }

    public class DataContainerRepository : IDataContainerRepository
    {
        public static DataContainerRepository m_instance = new DataContainerRepository();
        public static DataContainer Create(HostType hostType, HostType sendTo, RequestType requestType)
            => ((IDataContainerRepository)m_instance).Create(hostType, sendTo, requestType);

        DataContainer IRepository<DataContainer>.Create(HostType hostType, HostType sendTo, RequestType requestType)
            => new DataContainer()
            {
                Id = Guid.NewGuid(),
                CommunicationType = CommunicationType.REQUEST,
                CommunicationStatus = CommunicationStatus.SENDING,
                HostType = hostType,
                RequestType = requestType,
                SendTo = sendTo,
                SendTime = DateTime.Now,
            };
    }

    public interface IDataContainerRepository : IRepository<DataContainer>
    {
    }

    public interface IRepository<TType>
    {
        TType Create(HostType hostType, HostType sendTo, RequestType requestType);
    }
}
