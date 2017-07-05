using System.Collections.Generic;
using System.Linq;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [Authorize]
    public class SkillCategoryController : Controller
	{
		private CompetencyMatrixContext dbContext;

		public SkillCategoryController(CompetencyMatrixContext context)
		{
			dbContext = context;
		}

		#region CRUD

		[HttpGet("api/[controller]")]
		public IEnumerable<SkillCategory> Get()
		{
			var categories = dbContext.SkillCategory.ToList();
			var skills = dbContext.Skill.ToList();

			foreach (var category in categories)
			{
				//category.ChildCategories = categories.Where(c => c.ParentId == category.Id).ToList();
				//category.Skill = skills.Where(s => s.CategoryId == category.Id).ToList();
			}

			return categories;
		}

		[HttpGet("api/[controller]/{id}")]
		public SkillCategory Get(int id)
		{
			return Get().Single(c => c.Id == id);
		}

		[HttpGet("api/[controller]/root")]
		public IEnumerable<SkillCategory> GetRoot(int id)
		{
			return Get().Where(c => c.ParentId == null);
		}

		[HttpPost("api/[controller]")]
		public SkillCategory Post(SkillCategory skillCategory)
		{
            var addedCategory = dbContext.Add(skillCategory);
			var affected = dbContext.SaveChanges(true);
			return addedCategory.Entity;
		}

		[HttpPut("api/[controller]/{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		[HttpDelete("api/[controller]/{id}")]
		public JsonResult Delete(int id)
		{
            var categoryToDelete = dbContext.SkillCategory.Include(c => c.Skill).ThenInclude(s => s.SkillCriteria)
                .ThenInclude(c => c.SkillLevelCriteria).Single(c => c.Id == id);

            if (CheckCategoryForUsedSkills(categoryToDelete))
                return Json(new
                {
                    deleted = false,
                    error = "Selected item cannot be deleted."
                });
            else
            {
                RemoveSkillCategory(categoryToDelete);
                dbContext.SaveChanges(true);
                return Json(new
                {
                    deleted = true,
                });
            }
        }

        private bool CheckCategoryForUsedSkills(SkillCategory skillCategory)
        {
            foreach (var skill in skillCategory.Skill)
            {
                var existsInMatrix = dbContext.PositionMatrixSkill.Any(p => p.SkillId == skill.Id);
                var existsInEmployeeMatrixSkill = dbContext.EmployeeMatrixSkill.Any(emp => emp.SkillId == skill.Id);
                if (existsInMatrix || existsInEmployeeMatrixSkill)
                {
                    return true;
                }
            }

            var childCats = dbContext.SkillCategory.Include(c => c.Skill).Where(c => c.ParentId == skillCategory.Id).ToList();
            bool result = false;
            foreach (var cat in childCats)
            {
                result = CheckCategoryForUsedSkills(cat);
                if (result)
                    return true;
            }
            return result;
        }

        private void RemoveSkillCategory(SkillCategory skillCategory)
        {
            var childCats = dbContext.SkillCategory.Include(c => c.Skill).ThenInclude(s => s.SkillCriteria)
                .ThenInclude(c => c.SkillLevelCriteria).Where(c => c.ParentId == skillCategory.Id).ToList();
            foreach (var cat in childCats)
            {
                RemoveSkillCategory(cat);
            }
            foreach (var skill in skillCategory.Skill)
            {
                var skillCriterias = skill.SkillCriteria.ToList();
                skillCriterias.ForEach(c => dbContext.SkillLevelCriteria.RemoveRange(c.SkillLevelCriteria));
                dbContext.SkillCriteria.RemoveRange(skillCriterias);
                dbContext.Skill.Remove(skill);
            }
            dbContext.SkillCategory.Remove(skillCategory);
        }
		#endregion

		#region FancyTree

		[HttpPost("api/[controller]/move")]
		public SkillCategory ChangeParent([FromBody] SkillCategory value)
		{
			var category = dbContext.SkillCategory.Single(c => c.Id == value.Id);
			category.ParentId = value.ParentId;
			var updated = dbContext.Update(category);
			var affected = dbContext.SaveChanges(true);
			return value;
		}

		/// <summary>
		/// Deep clones skill category and all its skills and subcategories
		/// Pass Id and new ParentId to attach to
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpPost("api/[controller]/clone")]
		public SkillCategory Clone([FromBody] SkillCategory value)
		{
			var category = dbContext.SkillCategory.Include(c=>c.Skill).ToList().Single(c => c.Id == value.Id);
			var newCategory = Clone(category, value.ParentId);

			return newCategory;
		}

		public SkillCategory Clone(SkillCategory originalCategory, int? parentId)
		{
			var newCategory = originalCategory.Clone();
			newCategory.ParentId = parentId;
			var addedCategory = dbContext.Add(newCategory);
			var affected = dbContext.SaveChanges(true);
			
			if (originalCategory.InverseParent != null)
			{
				foreach (var childCategory in originalCategory.InverseParent)
				{
					Clone(childCategory, addedCategory.Entity.Id);
				}
			}

			if (originalCategory.Skill != null)
			{
				foreach (var skill in originalCategory.Skill)
				{
					var newSkill = skill.Clone();
					newSkill.CategoryId = addedCategory.Entity.Id;
					var addedSkill = dbContext.Add(newSkill);
				}
			}

			affected = dbContext.SaveChanges(true);

			return addedCategory.Entity;
		}

		[HttpPost("api/[controller]/rename")]
		public SkillCategory ChangeName([FromBody] SkillCategory value)
		{
			var category = dbContext.SkillCategory.Single(c => c.Id == value.Id);
			category.Name = value.Name;
			var updated = dbContext.Update(category);
			var affected = dbContext.SaveChanges(true);
			return value;
		}

		public ViewModels.SkillCategoryEditorViewModel SaveSkillCategoryDetails(ViewModels.SkillCategoryEditorViewModel value)
		{
			var skillCat = dbContext.SkillCategory.Single(s => s.Id == value.Id);

			skillCat.Name = value.Name;
			skillCat.Description = value.Description;

			var affectedSkillCat = dbContext.SaveChanges(true);

			return value;
		}

		public IActionResult GetSkillCategoryDetailsView(int id, bool isEditable = true)
		{
			var skillCategories = dbContext.SkillCategory.ToList();

			var skillCategory = dbContext.SkillCategory.Include(c => c.Parent).Single(c => c.Id == id);

			var viewModel = ViewModels.SkillCategoryEditorViewModel.FromDbModel(skillCategory);
		    viewModel.IsEditable = isEditable;

            return PartialView("SkillCategoryDetails", viewModel);
		}


		#endregion
	}
}
