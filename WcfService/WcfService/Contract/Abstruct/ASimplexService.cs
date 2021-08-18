using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfService.Contract.Interfaces;
using WcfService.Contract.Structure;

namespace WcfService.Contract.Abstruct
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public abstract class ASimplexService : ISimplexService
    {
        public abstract void Action(DataContainer container);
    }
}
