using System;
using System.Threading;
using log4net;
using Pinger.Interfaces;

namespace Pinger
{
    public class Notifier : INotifier
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Notifier));

        public void Notify(string message)
        {
            _log.Warn(message);
            Console.WriteLine(message);
        }

        public void Alarm()
        {
            SosBeep();
        }

        private void SosBeep()
        {
            Point();
            Point();
            Point();
            Pause();

            Dash();
            Dash();
            Dash();
            Pause();

            Point();
            Point();
            Point();
            Pause();
        }

        private void Pause()
        {
            Thread.Sleep(50);
        }

        private void Point()
        {
            Console.Beep(400, 150);
            Pause();
        }

        private void Dash()
        {
            Console.Beep(400, 150);
            Pause();
        }
    }
}