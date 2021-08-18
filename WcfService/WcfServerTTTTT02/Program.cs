﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WcfService;
using WcfService.Client;
using WcfService.Contract.Structure;
using WcfService.Server;

namespace WcfServerTTTTT02
{
    class Program
    {
        static private ILogger m_logger = new WcfConsoleLogger();
        static async Task Main(string[] args)
        {
            await StartUp();
            await ExecuteAsync(TokenSource.Token);
        }

        static async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if(ConfigrationCommon.Config.BurderingFlag) Burdening.Burden();

            while (!stoppingToken.IsCancellationRequested)
            {
                m_logger.Logging($"server running on {Server.Stauts()} status...");
                await Task.Delay(ConfigrationCommon.Config.WaitForExecuteAsync);
            }
        }

        static async Task StartUp()
        {
            ((IWcfServerTTTTT02)Server).Start();

            Console.WriteLine("server started");
            Console.WriteLine("wating for starting service.....");
            await Task.Delay(10000);
            Console.WriteLine("start to service TTTTT02");
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                TokenSource.Cancel();
                Server.Dispose();
                ShutDown();
            };
        }

        [STAThread]
        static void ShutDown()
        {
            Environment.Exit(0);
        }

        private static WcfServer Server = new WcfServer(HostType.TTTTT02);
        private static CancellationTokenSource TokenSource = new CancellationTokenSource();
    }
}