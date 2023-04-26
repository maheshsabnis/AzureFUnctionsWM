using AzHttpAPI.Models;
using AzHttpAPI.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Attrbute for Funtion Startup that will initialize the Dependeny COntainer

[assembly:FunctionsStartup(typeof(AzHttpAPI.Startup))]
namespace AzHttpAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // REad the COnnectionString
            string connstr = Environment.GetEnvironmentVariable("AppConnection");

            // register the CompanyCOntext class in DI Cntainer
            // One COnnection Object for All Functions in current App
            builder.Services.AddDbContext<CompanyContext>(
                  options => options.UseSqlServer(connstr)
                ) ;

            // Register the DepartmentService in DI Container
            // AddScoped: This will create one Service Object for All functions in the current applciation
            builder.Services.AddScoped<IServices<Department,int>, DepartmentService>();
        }

    }
}
