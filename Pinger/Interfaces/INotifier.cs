namespace Pinger.Interfaces
{
    public interface INotifier
    {
        void Notify(string message);
        void Alarm();
    }
}