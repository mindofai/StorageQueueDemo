using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using StorageQueueDemo.Core.Models;

namespace StorageQueueDemo.Receiver
{
    internal class Program
    {
        private const string connectionString = "<connection string here>";
        private const string queueName = "orders";

        static void Main(string[] args)
        {
            Console.WriteLine("Storage Queue Receiver running...");

            QueueClient queueClient = new QueueClient(connectionString, queueName);

            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                PeekedMessage[] peekedMessage = queueClient.PeekMessages();
                
                Console.WriteLine($"Peeked message: '{peekedMessage[0].Body}'");

                QueueProperties properties = queueClient.GetProperties();

                QueueMessage[] retrievedMessages = queueClient.ReceiveMessages(properties.ApproximateMessagesCount);

                foreach (var retrievedMessage in retrievedMessages)
                {
                    var order = JsonConvert.DeserializeObject<Order>(retrievedMessage.Body.ToString());
                    Console.WriteLine($"Delivery Id: '{order.Id}'\nDelivery Description: '{order.Description}'\nDelivery Addresss: '{order.Address}'\nDelivery Date: '{order.DeliveryDate}'");

                    queueClient.DeleteMessage(retrievedMessage.MessageId, retrievedMessage.PopReceipt);

                }
            }
        }


    }
}