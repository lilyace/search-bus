using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BusTelegramBot
{
    class Program
    {
        static private ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("1144536009:AAH_craoTzU26XLOXg_ppD4_qM-Qxp0DfB0");
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hello World! Its me: {me.Id} and my name is {me.FirstName}");
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text!=null)
            {
                Console.WriteLine($"Message: {e.Message.Text}");
            }

            await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"Your message {e.Message.Text}");
        }
    }
}
