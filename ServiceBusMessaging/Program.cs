using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating topic & subscriptions");
            // Create the topic if it does not exist already.
            string connectionString =
                CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.TopicExists("MyTopic"))
            {
                namespaceManager.CreateTopic("MyTopic");
            }
            
            if (!namespaceManager.SubscriptionExists("MyTopic", "MySubscription"))
            {
                SqlFilter highMessagesFilter =
                   new SqlFilter("MessageNumber > 3");

                namespaceManager.CreateSubscription("MyTopic",
                   "HighMessages",
                   highMessagesFilter);

                namespaceManager.CreateSubscription("MyTopic",
                    "MySubscription");
            }

            Console.WriteLine("Sending a message");
            var client = TopicClient.CreateFromConnectionString(connectionString, "MyTopic");
            
            for (int i=0;i<5;i++)
            {
                var msg = new BrokeredMessage("some message");
                msg.Properties["MessageNumber"] = i;
                client.Send(msg);
            }
                

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
