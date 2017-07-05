using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Services
{
    public interface ITemplateService
    {
        MailTemplate GetMailTemplate(MailTemplateType type);

        void UpdateTemplate(ViewModels.MailTemplate template);

        List<MailTemplate> GetMailTemplates();

        MailTemplate GetInitializedTemplate(MailTemplateType type, Employee employee, Employee manager);
    }
}
