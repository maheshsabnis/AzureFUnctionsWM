using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DurableTask.Core.History;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzFanInOut
{
    public static class AzFanoutIn
    {
        [FunctionName("AzFanoutIn")]
        public static async Task<long> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            // Call to FanOut Function
            // to read File List
            string[] files = await context.CallActivityAsync<string[]>(
          "GetFileList",
          "D:\\home\\LogFiles");

            // Creating Tasks (Seperate THreads on which Fan IN will work)

            var tasks = new Task<long>[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                // Dedicate the Function Execution on Seperate thread
                tasks[i] = context.CallActivityAsync<long>(
                    "CopyFile",
                    files[i]);
            }
            // THis will make sure thate all tasks are completing their job
            // The Caller THread aka IDurableOrchestrationContext object
            // will retuen only when all taks are done 

            await Task.WhenAll(tasks);
            // Calculate to total byte size of Fies
            long totalBytes = tasks.Sum(t => t.Result);
            return totalBytes;
        }
        /// <summary>
        /// FanOut Function that will retune data as List of Files in a specific Folder
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="log"></param>
        /// <returns></returns>

        [FunctionName("GetFileList")]
        public static string[] GetFileList(
    [ActivityTrigger] string rootDirectory,
    ILogger log)
        {
            log.LogInformation($"Searching for files under '{rootDirectory}'...");
            string[] files = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
            log.LogInformation($"Found {files.Length} file(s) under {rootDirectory}.");

            return files;
        }

        /// <summary>
        /// FanIn Funtion that will read each file
        /// Copy that into the New Folder
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="binder"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("CopyFile")]
        public static async Task<long> CopyFile(
    [ActivityTrigger] string filePath,
    Binder binder,
    ILogger log)
        {
            long byteCount = new FileInfo(filePath).Length;

            
            string outputLocation = $"D:\\home\\Copied";

            string blobPath = Path.GetFileName(filePath);



            log.LogInformation($"Copying '{filePath}' to '{outputLocation}'. Total bytes = {byteCount}.");

            // copy the file contents into a blob
            using (Stream source = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream destination = new FileStream($"{outputLocation}\\{blobPath}", FileMode.CreateNew))
            {
                if (File.Exists($"{outputLocation}\\{blobPath}"))
                {
                    log.LogInformation($"File  '{filePath}' is already exist at '{outputLocation}'. Total bytes = {byteCount}.");
                }
                else
                {
                    await source.CopyToAsync(destination);
                }
            }

            return byteCount;
        }


        [FunctionName("AzFanoutIn_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("AzFanoutIn", null);
            
            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}