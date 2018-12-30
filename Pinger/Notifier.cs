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
            //Send email or sms, etc
        }

        public void Alarm()
        {
            Notify("Alarm!");
            _log.Fatal("Alarm!");

            SosBeep();
        }

        private void SosBeep()
        {
            Dot();
            Dot();
            Dot();
            Pause();

            Dash();
            Dash();
            Dash();
            Pause();

            Dot();
            Dot();
            Dot();
            Pause();
        }

        const int ToneFreq = 1000; //Hz
        const int PauseInterval = 50; //ms
        const int DotInterval = 200; //ms
        const int DashInterval = 2 * DotInterval; //ms

        private void Pause()
        {
            Thread.Sleep(PauseInterval);
        }

        private void Dot()
        {
            Console.Beep(ToneFreq, DotInterval);
            Pause();
        }

        private void Dash()
        {
            Console.Beep(ToneFreq, DashInterval);
            Pause();
        }
    }
}