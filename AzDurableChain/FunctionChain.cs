using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AzDurableChain.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AzDurableChain
{
    public static class FunctionChain
    {

        static MessagingClient messaging = new MessagingClient();

        /// <summary>
        /// IDurableOrchestrationContext: Set the Context for Execution of All Fuctions inside the Context
        /// CallActivityAsync<T>(P1, P2), the method that invokes the Function.
        /// P1: The FUntion NAme, but this funtion name must have 'ActivityTrigger' so that it can start executio inside the context
        /// P2: the STate passed to the FUnction from inside the COntext
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [FunctionName("Function1")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            ToDo task = new ToDo() 
            {
              Task = "Define The Software Atchitecture Digram"
            };

            // 1. Call to the AddToDb Funcation in the Context
            // CallActivityAsync<T>("FUNCTION-NAME", T)
            // T is the type of data received from the function

            // The 'result' is the State that is returned from the function
            var result = await context.CallActivityAsync<List<ToDo>>(nameof(AddToDb), task);
            // COonditional Logic
            // Passing the Result of the FIrst Function to the Second FUnction
            await context.CallActivityAsync(nameof(AddToQueue), result);


        }

        /// <summary>
        /// Function1: Used to Add Data in Database and get Records from it 
        /// </summary>
        /// <param name="toDo"></param>
        /// <returns></returns>
        [FunctionName(nameof(AddToDb))]
        public static List<ToDo> AddToDb([ActivityTrigger] ToDo toDo)
        {
            List<ToDo> toDos = new List<ToDo>();

            try
            {
                SqlConnection Conn = new SqlConnection("Data Source=.;Initial Catalog=Company;Integrated Security=SSPI;Encrypt=Yes;TrustServerCertificate=Yes");
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = $"Insert into ToDo Values('{toDo.Task}')";
                Cmd.ExecuteNonQuery();

                // REad The Inserted Record
                Cmd.CommandText = "Select * from ToDo";
                SqlDataReader Reader = Cmd.ExecuteReader();
                while (Reader.Read()) 
                {
                    toDos.Add(new ToDo() { Id = Convert.ToInt32(Reader["Id"]), Task = Reader["task"].ToString() });
                }
                Reader.Close();
            }
            catch (System.Exception)
            {
                 
            }
            return toDos;
        }


        [FunctionName(nameof(AddToQueue))]
        public static async Task<string> AddToQueue([ActivityTrigger]List<ToDo> todos)
        {
            // Call method to add data in the Queue
            await messaging.AddMessageAsync(JsonSerializer.Serialize(todos));
            return "Data is added in Queue";
        }


        /// <summary>
        /// THe Http Trigger Funtion thatnis Async HTTP API
        /// THis will Start the 'Function1'
        /// IDurableOrchestrationClient: A COntract that will Create a Started to STart the  
        /// OrchaStrator Function to Execute based on Http Get, Post Request
        /// </summary>
        /// <param name="req"></param>
        /// <param name="starter"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            // Parameter 1: The FUnction NAme that has the "OrchestrationTrigger"
            string instanceId = await starter.StartNewAsync("Function1", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);
            // Return the RequestId and the COntext objet that is specifying the Execution Metatadata 
            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}