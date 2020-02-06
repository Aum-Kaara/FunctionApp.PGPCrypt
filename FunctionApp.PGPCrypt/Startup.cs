using FunctionApp.PGPCrypt;
using FunctionApp.PGPCrypt.Interface;
using FunctionApp.PGPCrypt.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionApp.PGPCrypt
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ICryption,Cryption>();
        }
    }
}
