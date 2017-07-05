using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class MailTemplate
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public MailTemplateType Type { get; set; }

        public static MailTemplate FromDBModel(CompetencyMatrix.Models.MailTemplate template)
        {
            var mailTemplate = new MailTemplate()
            {
                Id = template.Id,
                Body = template.Body,
                Subject = template.Subject,
                Type = template.Type
            };

            return mailTemplate;
        }
    }

    

}
