
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureStrorageLibrary.Services
{
    public class AzQueue
    {
        private readonly QueueClient _queueClient;

        public AzQueue(string queuename)
        {
            _queueClient = new QueueClient(ConnectionString.Con, queuename);
            _queueClient.CreateIfNotExistsAsync();
        }

        public async Task SendMessageAsync(string message)
        {            
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<QueueMessage> RetrieveMessageAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                QueueMessage[] retrievedMessage = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));
                if (retrievedMessage.Any())
                {
                    return retrievedMessage[0];
                }
            }
            return null;
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var response = await _queueClient.ReceiveMessagesAsync();
            var message = response.Value.FirstOrDefault();
            if (message != null)
            {
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                return message.MessageText;
            }
            return null;
        }

        public async Task<IEnumerable<string>> ReceiveMessagesAsync(int maxMessages)
        {
            var response = await _queueClient.ReceiveMessagesAsync(maxMessages);
            var messages = response.Value;
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                }
                return messages.Select(m => m.MessageText);
            }
            return null;
        }

        public async Task<IEnumerable<string>> PeekMessagesAsync(int maxMessages)
        {
            var response = await _queueClient.PeekMessagesAsync(maxMessages);
            var messages = response.Value;
            if (messages != null)
            {
                return messages.Select(m => m.MessageText);
            }
            return null;
        }

        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }

        public async Task ClearMessagesAsync()
        {
            await _queueClient.ClearMessagesAsync();
        }
    }
}
