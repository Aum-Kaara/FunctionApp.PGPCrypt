using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp.PGPCrypt.Interface;

namespace FunctionApp.PGPCrypt
{
    public class Decryption
    {
        private readonly ICryption _cryption;

        public Decryption(ICryption cryption)
        {
            _cryption = cryption;
        }

        [FunctionName("Decryption")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var encryptedData = await _cryption.DecryptAsync(requestBody);
            return requestBody != null
                ? (ActionResult)new OkObjectResult(encryptedData)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

    }
}
