 
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzDurableChain.Models
{
    public class MessagingClient
    {
        public async Task AddMessageAsync(string data)
        {
            // 1. Create a subsctiption to CloudStorahge Account
            // by copying the connection string from Azure Portal for Storage Account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            // 2. Create the CloudQueueCLient
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            // 3. REad the Queue under the Storage Account, if not present create it
            CloudQueue queue = queueClient.GetQueueReference("myq");
            
            // 4. CReate CLoud Message Object
            CloudQueueMessage queueMessage = new CloudQueueMessage(data);
            // 5. Send Message to Queue
            await queue.AddMessageAsync(queueMessage);
        }
    }
}
