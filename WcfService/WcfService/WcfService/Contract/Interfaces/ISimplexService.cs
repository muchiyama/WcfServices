using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfService.Contract.Structure;

namespace WcfService.Contract.Interfaces
{
    [ServiceContract(Namespace = "http://sample.wcf.service")]
    public interface ISimplexService
    {
        [OperationContract]
        void Action(DataContainer container);
    }
}
