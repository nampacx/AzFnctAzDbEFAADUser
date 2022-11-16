using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace NewFunction
{
    public class GetToDos
    {
        private readonly ILogger<GetToDos> logger;
        private readonly ToDoContext context;

        public GetToDos(ILogger<GetToDos> logger, ToDoContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [FunctionName("GetToDos")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            logger.LogInformation($"Called: {nameof(GetToDos)}.{nameof(Run)}");
            try
            {
                var todos = context.ToDos.ToList();

                return new OkObjectResult(todos);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex}");
                return new ObjectResult(ex.InnerException.Message.ToString()) { StatusCode = 500 };
            }
        }
    }
}
