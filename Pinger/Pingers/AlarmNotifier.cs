using System;
using System.Threading.Tasks;
using Pinger.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pinger.Pingers
{
    public class AlarmNotifier : IAlarmNotifier
    {
        public void SendAlarm(string message)
        {
            var t = new Task(async () =>
            {
                await SendAlarmAsync(message);
            });

            t.RunSynchronously();
        }

        public async Task SendAlarmAsync(string message)
        {
            var bot = new TelegramBotClient("733221706:AAGTrm1l2BkK2-1C_HKv5vSWa4EXlpCTx9k");

            var chatId = new ChatId(-1001415059840);
            var t = await bot.SendTextMessageAsync(chatId, message);
        }
    }
}