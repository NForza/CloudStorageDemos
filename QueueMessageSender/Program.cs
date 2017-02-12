using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueMessageSender
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

            var msg = new CloudQueueMessage("Hello");
            queue.AddMessage(msg);
        }
    }
}
