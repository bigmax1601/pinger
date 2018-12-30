using Pinger.Interfaces;

namespace Pinger.Pingers
{
    public class FrontDirectlyRequester : PingerBase
    {
        public FrontDirectlyRequester(IAlarmNotifier alarmNotifier) : base(alarmNotifier)
        {
        }

        public override string Name => "FRONT callback DIRECTLY";

        public override bool Ping()
        {
            return RequestingFrontDirectly();
        }

        private bool RequestingFrontDirectly()
        {
            const string Url = "https://78.140.143.142/ru?test";

            var result = PingerHelper.WebClientRequest(Url, out var response, Name);

            return result;
        }
    }
}