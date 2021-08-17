using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService.Client;
using WcfService.Contract;
using WcfService.Contract.Structure;
using WcfService.Server;

namespace WcfService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eventArgs) => { Console.WriteLine("canceled"); };
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
