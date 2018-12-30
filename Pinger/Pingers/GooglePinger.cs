using Pinger.Interfaces;

namespace Pinger.Pingers
{
    public class GooglePinger : PingerBase
    {
        public GooglePinger(IAlarmNotifier alarmNotifier) : base(alarmNotifier)
        {
        }

        public override string Name => "Google ping";
        public override bool Ping()
        {
            return PingGoogle();
        }

        private bool PingGoogle()
        {
            var url = "8.8.8.8";
            return PingerHelper.PingByICMP(url, Name);
        }

    }
}