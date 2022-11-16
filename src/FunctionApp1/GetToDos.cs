using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Configuration;

namespace FunctionApp1
{
    public class GetToDos
    {
        private readonly ILogger<GetToDos> _logger;

        public GetToDos(ILogger<GetToDos> log)
        {
            _logger = log;
        }

        [FunctionName(nameof(GetToDos))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation($"Called: {nameof(GetToDos)}.{nameof(Run)}");
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            _logger.LogInformation($"ConnectionString: {connectionString}");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (var model = new ToDoContext(connection))
                    {
                        var todos = model.ToDos.ToList();


                        return new OkObjectResult(todos);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex}");
                    return new ObjectResult(ex.InnerException.Message.ToString()) { StatusCode = 500 };
                }
            }
        }
    }


    public class ToDoContext : DbContext
    {
        public ToDoContext(SqlConnection con) : base(con, true)
        {
            Database.SetInitializer<ToDoContext>(null);
        }

        public virtual DbSet<ToDo> ToDos { get; set; }
    }
    [Table("ToDo")]
    public class ToDo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(200)]
        public string title { get; set; }

        public int order { get; set; }
        [StringLength(200)]
        public string url { get; set; }

        public bool completed { get; set; }
    }
}