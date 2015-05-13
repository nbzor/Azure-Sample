using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Threading.Tasks;
using T5.Common.Models;

namespace T5.Common
{
    public class ManagerQueue<T>
    {
        private CloudQueue queue = StorageUtils.QueueClient.GetQueueReference((string)Properties.Settings.Default["QueueStorageString"]);

        public async Task SendMessageAsync(T task)
        {
            await queue.CreateIfNotExistsAsync();
            var taskJson = JsonConvert.SerializeObject(task);

            CloudQueueMessage message = new CloudQueueMessage(taskJson);

            await queue.AddMessageAsync(message);
        }

        public async Task<T> GetMessageAsync()
        {
            await queue.CreateIfNotExistsAsync();
            CloudQueueMessage message = await queue.GetMessageAsync();
            if(message != null)
            {
                var entity = JsonConvert.DeserializeObject<T>(message.AsString);
                await queue.DeleteMessageAsync(message);
                return entity;
            }
            return default(T);
        }
    }
}
