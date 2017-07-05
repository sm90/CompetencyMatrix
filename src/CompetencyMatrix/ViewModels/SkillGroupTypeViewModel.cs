using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class SkillGroupTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static SkillGroupTypeViewModel FromDbModel(Models.SkillGroupType skillGroupType)
        {
            var model = new SkillGroupTypeViewModel();
            model.Id = skillGroupType.Id;
            model.Name = skillGroupType.Name;

            return model;
        }
    }
}
