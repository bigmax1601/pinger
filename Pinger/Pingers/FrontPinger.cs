using Pinger.Interfaces;

namespace Pinger.Pingers
{
    public class FrontPinger : PingerBase
    {
        public FrontPinger(IAlarmNotifier alarmNotifier) : base(alarmNotifier)
        {
        }

        public override string Name => "MyCredit Front site - php";
        public override bool Ping()
        {   //var url = "http://mycredit.ua";
            //return CustomPing(url);

            //var url = "srv.mycredit.ua";
            var url = "front.mycredit.ua";
            return PingerHelper.PingByICMP(url, Name);
        }
    }
}