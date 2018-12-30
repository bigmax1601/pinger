using Pinger.Interfaces;

namespace Pinger.Pingers
{
    public class BackRequester : PingerBase
    {
        public BackRequester(IAlarmNotifier alarmNotifier) : base(alarmNotifier)
        {
        }

        public override string Name => "BACK callback";

        public override bool Ping()
        {
            return RequestingBack();
        }

        private bool RequestingBack()
        {
            const string Url = "http://mcb.1bank.com.ua:9288/IsAlive";

            var result = PingerHelper.WebClientRequest(Url, out var response, Name);

            return result;
        }
    }
}