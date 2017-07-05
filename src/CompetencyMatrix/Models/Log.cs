using System;
using System.ComponentModel.DataAnnotations;

namespace CompetencyMatrix.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public DateTime Logged { get; set; } 

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Exception { get; set; }

        public string Environment { get; set; }

        public string EventId { get; set; }
    }
}
