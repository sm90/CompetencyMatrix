using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class SkillCategoryEditorViewModel
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<BreadCrumb> Breadcrumbs { get; set; }

        public bool IsEditable { get; set; }

        public static SkillCategoryEditorViewModel FromDbModel(Models.SkillCategory skillCategoryModel)
		{
			var skillCategory = new SkillCategoryEditorViewModel();
			skillCategory.Id = skillCategoryModel.Id;
			skillCategory.Name = skillCategoryModel.Name;
			skillCategory.Description = skillCategoryModel.Description;

			skillCategory.Breadcrumbs = generateBreadCrumbs(skillCategoryModel);

			return skillCategory;
		}

		static List<BreadCrumb> generateBreadCrumbs(Models.SkillCategory skillCategoryModel)
		{
			var crumbs = new List<BreadCrumb>();
			var categoryCrumb = new BreadCrumb() { Id = skillCategoryModel.Id, Path = skillCategoryModel.Name, IsCategory = true };

			crumbs.Add(categoryCrumb);

			if (skillCategoryModel.Parent != null)
			{
				generateBreadCrumbs(skillCategoryModel.Parent, crumbs);
			}

			return crumbs;
		}

		static void generateBreadCrumbs(Models.SkillCategory skillCategory, List<BreadCrumb> items)
		{
			var categoryCrumb = new BreadCrumb() { Id = skillCategory.Id, Path = skillCategory.Name, IsCategory = true };
			items.Insert(0, categoryCrumb);

			if (skillCategory.Parent != null)
			{
				generateBreadCrumbs(skillCategory.Parent, items);
			}
		}

	}
}
