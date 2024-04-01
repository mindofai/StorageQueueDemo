using Azure.Storage.Queues;
using Newtonsoft.Json;
using StorageQueueDemo.Core.Models;

namespace StorageQueueDemo.Sender
{
    internal class Program
    {
        private const string connectionString = "<connection string here>";
        private const string queueName = "orders";

        static void Main(string[] args)
        {
            Console.WriteLine("Storage Queue Sender running...");

            QueueClient queueClient = new QueueClient(connectionString, queueName);

            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                Random random = new Random();
                var orderCount = random.Next(5, 20);
                for (int counter = 1; counter <= orderCount; counter++)
                {
                    var order = new Order()
                    {
                        Id = counter,
                        Address = $"{counter} Place St., Location, Wherever City, Nowhere",
                        Description = $"Handle with care please!",
                        CreatedDate = DateTime.Now,
                        DeliveryDate = DateTime.Now.AddDays(counter)
                    };

                    queueClient.SendMessage(JsonConvert.SerializeObject(order));

                    Console.WriteLine($"Delivery #{counter} sent!");
                }
            }
        }


    }

}