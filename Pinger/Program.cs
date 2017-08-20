using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Pinger.Interfaces;

namespace Pinger
{
    class Program
    {
        private const int TryErrors = 3;
        private const int TrySuccess = 5;
        private const int Interval = 10;

        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        static Program()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private static bool _exitFlag = false;

        static void Main(string[] args)
        {
            _log.Info("Starting pinger...");

            var task = Task.Run(() => Go(PingBack));

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                Thread.Sleep(1000);
            }

            _log.Debug("Exiting.... _exitFlag = true");
            _exitFlag = true;
            task.Wait();

            _log.Info("Pinger is finished.");
        }


        private delegate bool PingDelegate();
        private static void Go(PingDelegate ping)
        {
            INotifier notifier = new Notifier();

            int trySuccessCounter = 0;
            int tryErrorsCounter = 0;
            bool allIsOk = true;

            do
            {
                bool pingResult = ping();

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
                        notifier.Notify("Alarm!");
                        notifier.Alarm();
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
                        notifier.Notify("All Ok!");
                        allIsOk = true;
                    }
                }

            } while (!_exitFlag);

            _log.Debug("Go method detected _exitFlag == true.");
        }

        private static bool PingBack()
        {
            //var url = "mycredit.ua";
            //var url = "10.11.0.30";
            var url = "Mcb.1bank.com.ua";

            _log.Debug($"Pinging {url}...");

            var ping = new Ping();
            var result = ping.Send(url);

            _log.Debug($"Result {result?.Status}");

            return result?.Status == IPStatus.Success;
        }

        private static bool PingFront()
        {
            var url = "http://mycredit.ua";
            _log.Debug($"Pinging {url}...");

            bool result = CustomPing(url);
            var resMsg = result ? "succeed." : "FAILED!";

            _log.Debug($"Pinging {resMsg}");

            return result;
        }


        private static bool CustomPing(string url)
        {
            var uri = new Uri(url);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 3000;
            request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
            request.Method = "HEAD";

            try
            {
                var response = request.GetResponse();
                return true;
            }
            catch (WebException wex)
            {
                _log.Error("Ping exception:\n", wex);
                return false;
            }
        }

    }
}
