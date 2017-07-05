using CompetencyMatrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ICompetencyMatrixContext _dbContext;
        public TemplateService(ICompetencyMatrixContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MailTemplate> GetMailTemplates()
        {
           return _dbContext.MailTemplate.ToList();
        }
        public MailTemplate GetMailTemplate(MailTemplateType type)
        {
            return _dbContext.MailTemplate.First(x => x.Type == type);
        }

        public void UpdateTemplate(ViewModels.MailTemplate template)
        {
            var model = _dbContext.MailTemplate.First(x => x.Id == template.Id);

            model.Subject = template.Subject;
            model.Body = template.Body;

            _dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _dbContext.SaveChanges();
        }


        public MailTemplate GetInitializedTemplate(MailTemplateType type, Employee employee, Employee manager)
        {
            var template = GetMailTemplate(type);

            template.Body = template.Body.Replace("{{Employee}}", employee.Name).Replace("{{Manager}}", manager.Name);

            return template;
        }

    }
}
