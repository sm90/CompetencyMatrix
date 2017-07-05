using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class EmployeeMatrixApproval
    {
        public int Id { get; set; }
        public string ByWhom { get; set; }
        public DateTime When { get; set; }

        
        public int EmployeeId { get; set; }
        

        //[ForeignKey("MatrixId")]
        public Employee Employee { get; set; }

        public virtual AspNetUsers ByWhomNavigation { get; set; }
    }
}
