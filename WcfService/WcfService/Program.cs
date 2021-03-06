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
    /// <summary>
    /// 動作確認用のエントリポイント
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new WcfConsoleLogger();
            var serverCCCCC = new WcfServer(HostType.CCCCC);
            serverCCCCC.Start();

            var serverTTTTT01 = new WcfServer(HostType.TTTTT01);
            serverTTTTT01.Start();

            var clientCCCCC = new WcfClient(HostType.CCCCC, HostType.TTTTT01);
            clientCCCCC.Open();
            var container = new DataContainer()
            {
                Id = Guid.NewGuid(),
                CommunicationType = CommunicationType.REQUEST,
                HostType = HostType.CCCCC,
                SendTime = DateTime.Now,
            };

            clientCCCCC.SendData(container);

            var list = new List<int>();
            for(var i = 0; i < 1000; i++)
                list.Add(i);

            Parallel.ForEach(list,l =>
            {
                Console.WriteLine($"{l} | {DateTime.Now.ToString("hh:mm:ss.ffff")}");
                System.Threading.Thread.Sleep(10000);
            });

            Console.WriteLine("end");
            Console.ReadLine();
        }
    }
}
