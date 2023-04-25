using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using AzHttpAPI.Services;
using AzHttpAPI.Models;

namespace AzHttpAPI
{
    public  class APIFunction
    {
        IServices<Department,int> deptServ = new DepartmentService();
         
        ResponseObject<Department> response = new ResponseObject<Department>(); 

        [FunctionName("GET")]
        public  async Task<IActionResult> GET(
            [HttpTrigger(AuthorizationLevel.Function, "get" , Route = "Departments")] HttpRequest req,
            ILogger log)
        {

            response = await deptServ.GetAsync();
            return new OkObjectResult(response);
        }

        [FunctionName("GETSingle")]
        public  async Task<IActionResult> GETSingle(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Departments/{id:int}")] HttpRequest req,int id,
            ILogger log)
        {
            response = await deptServ.GetAsync(id);
            return new OkObjectResult(response);
        }

        [FunctionName("POST")]
        public  async Task<IActionResult> POST(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = "Departments")] HttpRequest req,
            ILogger log)
        {
            var bodyData = new StreamReader(req.Body).ReadToEnd();
            var dept = JsonSerializer.Deserialize<Department>(bodyData);
            response = await deptServ.CreateAsync(dept);

            return new OkObjectResult(response);
        }

        [FunctionName("PUT")]
        public  async Task<IActionResult> PUT(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Departments/{id:int}")] HttpRequest req, int id,
           ILogger log)
        {
            //  var queryData = req.Query["id"];
            var bodyData = new StreamReader(req.Body).ReadToEnd();
            var dept = JsonSerializer.Deserialize<Department>(bodyData);
            response = await deptServ.UpdateAsync(id,dept);

            return new OkObjectResult(response);

            
        }
    }
}
