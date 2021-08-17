using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService.Contract.Abstruct;
using WcfService.Contract.Structure;

namespace WcfService.Contract
{
    public class SimplexService : ASimplexService
    {
        public delegate void RaiseEvent(DataContainer container);
        public RaiseEvent RaiseEventHandler;
        public override void Action(DataContainer container)
            => RaiseEventHandler(container);
    }
}
