using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.Models
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Group { get; set; }
        public DateTime DateAndTime { get; set; }
        public string PCName { get; set; }
        public int Points { get; set; }
    }
    public class StudentDbContext : DbContext
    {
        public DbSet<Result> Results { get; set; }
    }
}
