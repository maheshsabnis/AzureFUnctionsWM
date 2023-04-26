using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzHttpAPI.Services
{
    public class MessagingClient
    {
        public async Task AddMessageAsync(string data)
        {
            // 1. Create a subsctiption to CloudStorahge Account
            // by copying the connection string from Azure Portal for Storage Account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=msitstorageaccount;AccountKey=HCB2Oc6LHBMA7FAiSu9JAyAorap+nFYh7lzfxPUXyL8jWcPZa84YY1eeI0hETp5fYWUGS4yXL/DA+AStyqbTDA==;EndpointSuffix=core.windows.net");
            // 2. Create the CloudQueueCLient
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            // 3. REad the Queue under the Storage Account, if not present create it
            CloudQueue queue = queueClient.GetQueueReference("myqueue-items");
            
            // 4. CReate CLoud Message Object
            CloudQueueMessage queueMessage = new CloudQueueMessage(data);
            // 5. Send Message to Queue
            await queue.AddMessageAsync(queueMessage);
        }
    }
}
