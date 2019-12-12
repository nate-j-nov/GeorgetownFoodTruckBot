using MargieBot;
using System.Text;
using System;

namespace GeorgetownFoodTruckBot
{
    public class Responder : IResponder
    {
        static string file = "C:\\Users\\natej\\Documents\\C#\\Food Truck Bot\\Lottery Results\\2019\\Dec 2019 - MRV Lottery Results.pdf";
        static DataCleaner cleaner = new DataCleaner(file);
        TruckDisplayer ftTruckDisplayer = new TruckDisplayer(cleaner.FoodTruckList);
        public bool CanRespond(ResponseContext context)
        {
            return context.Message.MentionsBot
                  && !context.BotHasResponded
                  &&
                  (
                  context.Message.Text.ToLower().Contains("hello")
                  || context.Message.Text.ToLower().Contains("hi")
                  || context.Message.Text.ToLower().Contains("what's for lunch")
                  || context.Message.Text.ToLower().Contains("whats for lunch")
                  || context.Message.Text.ToLower().Contains("i'm hungry")
                  || context.Message.Text.ToLower().Contains("im hungry")
                  );
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var builder = new StringBuilder();
            builder.Append("Hello, ").Append(context.Message.User.FormattedUserID +  ", here's today's menu:" + "\n");
            foreach (var ft in ftTruckDisplayer.TrucksInGeorgetown)
            {
                builder.Append(ft.BusinessName + "\n");
            }
            return new BotMessage { Text = builder.ToString() };
        }
    }
}

