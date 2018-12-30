using Pinger.Interfaces;

namespace Pinger.Pingers
{
    public class BackPinger : PingerBase
    {
        public BackPinger(IAlarmNotifier alarmNotifier) : base(alarmNotifier)
        {
        }

        public override string Name => "MyCredit Back site - CRM";
        public override bool Ping()
        {
            //var url = "mycredit.ua";
            //var url = "10.11.0.30";
            var url = "mcb.1bank.com.ua";

            return PingerHelper.PingByICMP(url, "MyCredit Back site - CRM");
        }
    }
}