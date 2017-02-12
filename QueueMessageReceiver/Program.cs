using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueMessageReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var cloudConnectionString = ConfigurationManager.ConnectionStrings["CloudStorage"].ConnectionString;
            var account = CloudStorageAccount.Parse(cloudConnectionString);
            var client = account.CreateCloudQueueClient();
            var queue = client.GetQueueReference("queue");
            queue.CreateIfNotExists();

            var nextMessage = queue.GetMessage(TimeSpan.FromSeconds(10));
            while (true)
            {               
                if (nextMessage==null)
                    Console.WriteLine("No message in q");
                else
                    Console.WriteLine($"Data from q: {nextMessage.AsString}");

                nextMessage = queue.GetMessage(TimeSpan.FromSeconds(10));
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}
