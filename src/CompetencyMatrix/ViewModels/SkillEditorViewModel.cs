using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompetencyMatrix.ViewModels
{
    public class SkillEditorViewModel
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<BreadCrumb> Breadcrumbs { get; set; }

		public string TrainingMaterials { get; set; }
		public string Questionarie { get; set; }

		public int? EvaluationModelId { get; set; }

		public SelectList EvaluationModels { get; set; }

		public SkillLevelCriteriaMatrix CriteriaMatrix { get; set; }

        public bool IsEditable { get; set; }

        public static SkillEditorViewModel FromDbModel(Models.Skill skillModel)
	    {
		    var skill = new SkillEditorViewModel();
		    skill.Id = skillModel.Id;
		    skill.Name = skillModel.Name;
		    skill.Description = skillModel.Description;
		    skill.TrainingMaterials = skillModel.TrainingMaterials;
			skill.Questionarie = skillModel.Questionarie;
		    skill.EvaluationModelId = skillModel.EvaluationModelId;

		    skill.Breadcrumbs = generateBreadCrumbs(skillModel);

		    skill.CriteriaMatrix = new SkillLevelCriteriaMatrix(skillModel);

		    return skill;
	    }

	    static List<BreadCrumb> generateBreadCrumbs(Models.Skill skillModel)
	    {
		    var crumbs = new List<BreadCrumb>();

		    var skillCrumb = new BreadCrumb() {Id = skillModel.Id, Path = skillModel.Name, IsCategory = false};
			crumbs.Add(skillCrumb);

		    generateBreadCrumbs(skillModel.Category, crumbs);

			return crumbs;
	    }

	    static void generateBreadCrumbs(Models.SkillCategory skillCategory, List<BreadCrumb> items)
	    {
		    var categoryCrumb = new BreadCrumb() {Id = skillCategory.Id, Path = skillCategory.Name, IsCategory = true};
			items.Insert(0, categoryCrumb);

		    if (skillCategory.Parent != null)
		    {
			    generateBreadCrumbs(skillCategory.Parent, items);
		    }
	    }

	    public void Rebuild()
	    {
		    
	    }
    }
}
