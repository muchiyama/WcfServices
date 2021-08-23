using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfConsoleBase.Interfaces
{
    public interface IClientTaskExecuter
    {
        void Start();
    }

    public interface IServerTaskExecuter
    {
        void Start();
    }
}
