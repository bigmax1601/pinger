using Pinger.Interfaces;
using Pinger.Pingers;
using StructureMap;

namespace Pinger
{
    public class PingerRegistry : Registry
    {
        public PingerRegistry()
        {
            For<PingerBase>().Use<FrontPinger>();
            For<PingerBase>().Use<FrontRequester>();
            //For<PingerBase>().Use<FrontDirectlyRequester>();
            For<PingerBase>().Use<BackPinger>();
            For<PingerBase>().Use<BackRequester>();
            For<PingerBase>().Use<GooglePinger>();

            For<IAlarmNotifier>().Use<AlarmNotifier>();
        }
    }
}