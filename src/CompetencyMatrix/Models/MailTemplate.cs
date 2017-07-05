using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Models
{
    public enum MailTemplateType
    {
        SubmitEmployeeProfile = 1,
        ApproveEmployeeProfile = 2,
        RejectEmployeeProfile = 3,

    }

    public class MailTemplate
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public MailTemplateType Type { get; set; }
    }
}
