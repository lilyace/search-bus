using BusInfo.Services;
using BusInfo.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BusTelegramBot
{
    class Program
    {
        static private ITelegramBotClient botClient;
        private static IBusInformationService _busInformationService;
        static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddTransient<IBusInformationService, BusInformationFromWeb>();
            var serviceProvider=collection.BuildServiceProvider();
            _busInformationService = serviceProvider.GetService<IBusInformationService>();
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


            if(e.Message.Chat.Type!=ChatType.Private && !(e.Message.Chat.Id == 282797907 && e.Message.Chat.Id == 408418628))
            {
                await botClient.SendTextMessageAsync(e.Message.Chat, "Пользователь не авторизован");
                return;
            }
            string routeNumber="";
            char direction = 't';
            try
            {
                MatchCollection groups;
                switch (e.Message.Text)
                {
                    case string message when !message.StartsWith("/"):
                        await SendMessage(e.Message.Chat, "Неверная команда");
                        break;

                    case string message when Regex.IsMatch(message, @"/[Оо]становки \d+"):
                        routeNumber = Regex.Match(message, @"\d+").Value;
                        var busstopList = await _busInformationService.GetBusStopList(Convert.ToInt32(routeNumber), 't');
                        if (busstopList == null)
                            await SendMessage(e.Message.Chat, "Задан неверный номер маршрута");
                        await SendMessage(e.Message.Chat, string.Join("\n", busstopList));
                        break;

                    case string message when message.StartsWith("/Автобусов на маршруте", StringComparison.OrdinalIgnoreCase):
                        var bb= Regex.IsMatch(message, @"/Автобусов на маршруте");
                        groups = Regex.Matches(message, @"/(Автобусов на маршруте) (\d+)\s*\w*");
                        routeNumber = groups[0].Groups[2].Value;
                        direction = 't';
                        if (message.Contains("обратно"))
                            direction = 'b';                        
                        var activeBusesCount = await _busInformationService.GetActiveBusesCount(Convert.ToInt32(routeNumber), direction);
                        await SendMessage(e.Message.Chat, activeBusesCount.ToString());
                        break;

                    case string message when message.StartsWith("/Когда приедет", StringComparison.OrdinalIgnoreCase):
                        groups = Regex.Matches(message, @"/([Кк]огда приедет) (\d+) (остановка) (\w+) (\w+)");
                        routeNumber = groups[0].Groups[2].Value;
                        var busstop = groups[0].Groups[4].Value;
                        direction = 't';
                        if (message.Contains("обратно"))
                            direction = 'b';
                        var timeInfo = await _busInformationService.GetBusArrivalTime(Convert.ToInt32(routeNumber), busstop, direction);
                        await SendMessage(e.Message.Chat, timeInfo);
                        break;
                }
            }
            catch
            {
                await SendMessage(e.Message.Chat, "Введена неверная команда. Проверьте ввод данных");
            }
        }

        static async Task SendMessage(Chat chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}
