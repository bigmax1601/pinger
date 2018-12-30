using Pinger.Interfaces;

namespace Pinger.Pingers
{
    public class FrontRequester : PingerBase
    {
        public FrontRequester(IAlarmNotifier alarmNotifier) : base(alarmNotifier)
        {
        }

        public override string Name => "FRONT callback";

        public override bool Ping()
        {
            return RequestingFront();
        }

        private bool RequestingFront()
        {
            const string Url = "https://mycredit.ua/ru?test";

            var result = PingerHelper.WebClientRequest(Url, out var response, Name);

            return result;
        }
    }
}