using System;
using Castle.Windsor;
using System.Collections.Generic;
using MargieBot;
using Castle.MicroKernel.Registration;
using System.Configuration;


namespace GeorgetownFoodTruckBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IResponder>().ImplementedBy<Responder>());

            var bot = new Bot();
            var responders = container.ResolveAll<IResponder>();
            foreach (var responder in responders)
            {
                bot.Responders.Add(responder);
            }
            var connect = bot.Connect(ConfigurationManager.AppSettings["GeorgetownFTBotAPIToken"]);

            while (Console.ReadLine() != "close") { }
        }
    }
}
