using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public int MatrixId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [InverseProperty("Employee")]
        public virtual ICollection<Users> Users { get; set; }
        [ForeignKey("MatrixId")]
        [InverseProperty("Employee")]
        public virtual EmployeeMatrix Matrix { get; set; }
    }
}
