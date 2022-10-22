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
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> log)
        {
            _logger = log;
        }

        [FunctionName("Function1")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var token = req.Headers["Authorization"].FirstOrDefault();
            token = token.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Environment.GetEnvironmentVariable("SqlServerName");
            builder.InitialCatalog = Environment.GetEnvironmentVariable("SqlDbName");

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.AccessToken = token;
                    using (var model = new BruceDbContext(connection))
                    {
                        var users = model.ToDos.ToList();
                        return new OkObjectResult($"Results:{Environment.NewLine}{JsonConvert.SerializeObject(users)}");
                    }
                }

                catch (Exception ex)
                {
                    return new ObjectResult(ex) { StatusCode = 500 };
                }
            }
        }
    }


    public class BruceDbContext : DbContext
    {
        public BruceDbContext(SqlConnection con) : base(con, true)
        {
            Database.SetInitializer<BruceDbContext>(null);
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