using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FunctionApp1
{
    public class GetToDos
    {
        //private readonly ILogger<GetToDos> _logger;
        //private readonly ToDoContext context;

        //public GetToDos(ILogger<GetToDos> log, ToDoContext context)
        //{
        //    _logger = log;
        //    this.context = context;
        //}

        [FunctionName(nameof(GetToDos))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            //_logger.LogInformation($"Called: {nameof(GetToDos)}.{nameof(Run)}");
            try
            {
                //var todos = context.ToDos.ToList();

                //return new OkObjectResult(todos);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error: {ex}");
                return new ObjectResult(ex.InnerException.Message.ToString()) { StatusCode = 500 };
            }

            return null;
        }
    }


    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions con) : base(con)
        {
           
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