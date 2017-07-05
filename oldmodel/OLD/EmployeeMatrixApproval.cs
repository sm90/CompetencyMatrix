using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompetencyMatrix.Models
{
    public partial class EmployeeMatrixApproval
    {
        [Column("id")]
        public int Id { get; set; }
        public int ByWhom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime When { get; set; }
        public int MatrixId { get; set; }

        [ForeignKey("ByWhom")]
        [InverseProperty("EmployeeMatrixApproval")]
        public virtual Users ByWhomNavigation { get; set; }
        [ForeignKey("MatrixId")]
        [InverseProperty("EmployeeMatrixApproval")]
        public virtual EmployeeMatrix Matrix { get; set; }
    }
}
