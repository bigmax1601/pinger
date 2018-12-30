using System;
using Pinger.Interfaces;

namespace Pinger.Pingers
{
    public abstract class PingerBase
    {
        private readonly IAlarmNotifier _alarmNotifier;

        protected PingerBase(IAlarmNotifier alarmNotifier)
        {
            _alarmNotifier = alarmNotifier;
        }

        public abstract string Name { get; }
        public abstract bool Ping();

        public void Alarm(string message = null)
        {
            _alarmNotifier.SendAlarm($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} ALARM from '{Name}'{(string.IsNullOrEmpty(message) ? "" : $": {message}")}");
        }
    }
}