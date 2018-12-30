using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Pinger.Interfaces;
using Pinger.Pingers;

namespace Pinger
{
    class Program
    {
        private const int TryErrors = 3;
        private const int TrySuccess = 5;
        private const int Interval = 10;

        private static readonly ILog _log = LogManager.GetLogger("LOG");

        static Program()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private static bool _exitFlag = false;

        static void Main(string[] args)
        {
            _log.Info("Starting pinger...");

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var pingers = ObjectFactory.Container.GetAllInstances<PingerBase>();
            var tasks = RunPingers(pingers);

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                Thread.Sleep(1000);
            }

            _log.Debug("Exiting.... _exitFlag = true");
            _exitFlag = true;

            tasks.ForEach(task => task.Wait());

            _log.Info("Pinger is finished.");
        }

        private static List<Task> RunPingers(IEnumerable<PingerBase> pingers)
        {
            var tasks = new List<Task>();

            foreach (var pinger in pingers)
            {
                tasks.Add(Task.Run(() => Go(pinger)));
                Thread.Sleep(Interval * 1000 / pingers.Count());
            }

            tasks.Add(Task.Run(() => ClearMemoryThread()));

            return tasks;
        }

        private static void Go(PingerBase pinger)
        {
            INotifier notifier = new Notifier();

            int trySuccessCounter = 0;
            int tryErrorsCounter = 0;
            bool allIsOk = true;

            do
            {
                bool pingResult = pinger.Ping();

                if (allIsOk)
                {
                    if (pingResult)
                    {
                        tryErrorsCounter = 0;
                        Thread.Sleep(Interval * 1000);
                        continue;
                    }

                    tryErrorsCounter++;

                    if (tryErrorsCounter > TryErrors)
                    {
                        notifier.Alarm();
                        pinger.Alarm();
                        allIsOk = false;
                    }
                }
                else
                {
                    if (!pingResult)
                    {
                        trySuccessCounter = 0;
                        Thread.Sleep(Interval * 1000);
                        continue;
                    }

                    trySuccessCounter++;

                    if (trySuccessCounter > TrySuccess)
                    {
                        //notifier.Notify("All Ok!");
                        pinger.Alarm("All is Ok!");
                        allIsOk = true;
                    }
                }

            } while (!_exitFlag);

            _log.Debug("Go method detected _exitFlag == true.");
        }


        private static bool ClearMemory()
        {
            const string BaseUrl = "http://mcb.1bank.com.ua:9288";
            const string UrlTotalMemory = BaseUrl + "/TotalMemory";
            const string UrlClearMemory = BaseUrl + "/ClearMemory";

            _log.Warn("Releasing memory by CG.Collect() call...");

            var result1 = PingerHelper.WebClientRequest(UrlTotalMemory, out var response, "BACK: Get total memory =>");
            var result2 = PingerHelper.WebClientRequest(UrlClearMemory, out response, "BACK: Release unused memory => ");

            return result1 && result2;
        }

        private static void ClearMemoryThread()
        {
            const int ClearMemoryPeriod = 120 * 1000; //2 minutes
            do
            {
                ClearMemory();
                Thread.Sleep(ClearMemoryPeriod);
            } while (!_exitFlag);

            _log.Debug("ClearMemoryThread method detected _exitFlag == true.");
        }
    }
}
