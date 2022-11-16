using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewFunction
{
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
