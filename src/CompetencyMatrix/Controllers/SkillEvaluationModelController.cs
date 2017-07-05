using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [Authorize]
    public class SkillEvaluationModelController : Controller
	{
		private CompetencyMatrixContext dbContext;

		public SkillEvaluationModelController(CompetencyMatrixContext context)
		{
			dbContext = context;
		}

		public string GetName(int id)
		{
			var evaluationModel = dbContext
				.SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel).ThenInclude(seml => seml.SkillLevelModel)
				.ToList()
				.Select(
					sem => SkillEvaluationModelViewModel.FromDbModel(sem)).ToList().Single(m => m.Id == id);

			return evaluationModel.DisplayName;
		}

		public IActionResult SaveEvaluationViewModels(List<ViewModels.SkillEvaluationModelViewModel> evaluationViewModels)
		{
			for (int index = 0; index < evaluationViewModels.Count; index++)
			{
				var skillEvaluationModelViewModel = evaluationViewModels[index];
				var updatedModel = SaveEvaluationViewModel(skillEvaluationModelViewModel);
				evaluationViewModels[index] = updatedModel;
			}

			var existingModelIds = evaluationViewModels.Select(e => e.Id).ToList();
			var deletedModels = dbContext.SkillEvaluationModel.Where(sem => !existingModelIds.Contains(sem.Id)).ToList();

			foreach (var deletedModel in deletedModels)
			{
				DeleteEvaluationModel(deletedModel.Id);
			}

			ModelState.Clear();

			return PartialView("SkillEvaluationModels", evaluationViewModels);
		}

		public SkillEvaluationModelViewModel SaveEvaluationViewModel(SkillEvaluationModelViewModel evaluationViewModel)
		{
			SkillEvaluationModelViewModel model = evaluationViewModel;
			
			var dbModel = dbContext.SkillEvaluationModel
				.Include(sem => sem.SkillEvaluationModelLevel).ThenInclude(seml => seml.SkillLevelModel)
				.SingleOrDefault(m => m.Id == model.Id);

			if (dbModel == null)
			{
				dbModel = new SkillEvaluationModel();
				dbContext.SkillEvaluationModel.Add(dbModel);
			}

			dbModel.Name = evaluationViewModel.Name;
			dbModel.Description = evaluationViewModel.Description;

			evaluationViewModel.Levels.RemoveAll(l => l.Id == 0 || l.Name == null);

			var existingDbLevels = dbModel.SkillEvaluationModelLevel.ToList();
			var deletedDbLevels = existingDbLevels.Where(c => !evaluationViewModel.Levels.Select(lvl => lvl.Id).Contains(c.SkillLevelModelId)).ToList();
			var deletedDbEvaluationModelLevels =
				dbContext.SkillEvaluationModelLevel.Where(seml => deletedDbLevels.Select(c => c.SkillLevelModelId).Contains(seml.SkillLevelModelId))
					.ToList();
			var deletedDbLevelCriteria =
				dbContext.SkillLevelCriteria.Where(slc => deletedDbLevels.Select(c => c.SkillLevelModelId).Contains(slc.SkillLevelModelId))
					.ToList();

			dbContext.RemoveRange(deletedDbLevelCriteria);
			dbContext.RemoveRange(deletedDbEvaluationModelLevels);
			dbContext.RemoveRange(deletedDbLevels);

			foreach (var levelViewModel in evaluationViewModel.Levels)
			{
				var dbLevelModel = dbContext.SkillLevelModel.SingleOrDefault(m => m.Id == levelViewModel.Id);
				if (dbLevelModel == null)
				{
					dbLevelModel = new SkillLevelModel();
					dbContext.SkillLevelModel.Add(dbLevelModel);
				}

				dbLevelModel.Name = levelViewModel.Name;
				dbLevelModel.Description = levelViewModel.Description;
				dbLevelModel.Quality = levelViewModel.Quality;

				var existing = dbModel.SkillEvaluationModelLevel.SingleOrDefault(seml => seml.SkillLevelModel.Id == dbLevelModel.Id);
				if (existing == null)
				{
					dbModel.SkillEvaluationModelLevel.Add(new SkillEvaluationModelLevel()
					{
						SkillEvaluationModel = dbModel,
						SkillLevelModel = dbLevelModel
					});
				}
			}

			dbContext.SaveChanges(true);

			evaluationViewModel = ViewModels.SkillEvaluationModelViewModel.FromDbModel(dbModel);

			ModelState.Clear();

			return evaluationViewModel;
		}

		public IActionResult AddLevel(ViewModels.SkillEvaluationModelViewModel evaluationViewModel, int Index)
		{
			var newId = -1;

			if (evaluationViewModel.Levels.Any(e => e.Id < 0))
			{
				newId = evaluationViewModel.Levels.Min(e => e.Id) - 1;
			}

			var level = new ViewModels.SkillLevelModelViewModel();
			level.Id = newId;
			level.Name = "New level";

			evaluationViewModel.Levels.ForEach(e => e.Quality = e.Quality * 10);

			if (evaluationViewModel.CurrentLevel==null || evaluationViewModel.CurrentLevel.Id == 0)
			{
				if (evaluationViewModel.Levels.Count == 0)
				{
					level.Quality = 1;
				}
				else
				{
					level.Quality = evaluationViewModel.Levels.Max(e => e.Quality) + 1;
				}
				
				evaluationViewModel.Levels.Add(level);
			}
			else
			{
				evaluationViewModel.CurrentLevel.Quality = evaluationViewModel.CurrentLevel.Quality * 10;
				
				var currentRow = evaluationViewModel.Levels.Single(e => e.Id == evaluationViewModel.CurrentLevel.Id && e.Quality == evaluationViewModel.CurrentLevel.Quality);
				level.Quality = currentRow.Quality + 1;
				evaluationViewModel.Levels.Add(level);
			}

			evaluationViewModel.Levels = evaluationViewModel.Levels.OrderBy(e => e.Quality).ToList();
			evaluationViewModel.Levels.ForEach(e => e.Quality = evaluationViewModel.Levels.IndexOf(e) + 1);

			ModelState.Clear();

			var view = PartialView("EditorTemplates/SkillEvaluationModel", evaluationViewModel);
			view.ViewData.TemplateInfo.HtmlFieldPrefix = $"[{Index}]";
			view.ViewData["Index"] = Index;

			return view;
		}

		public IActionResult GetEditorView(int id)
		{
			var evaluationModel = dbContext
				.SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel).ThenInclude(seml => seml.SkillLevelModel)
				.ToList()
				.Select(
					sem => SkillEvaluationModelViewModel.FromDbModel(sem)).ToList().SingleOrDefault(m => m.Id == id);

			if (evaluationModel != null)
			{
				return PartialView("EditorTemplates/SkillEvaluationModel", evaluationModel);
			}

			return GetNewEditorView();
		}

		public IActionResult GetNewEditorView()
		{
			var evaluationModel = new SkillEvaluationModelViewModel();
			evaluationModel.Name = "New model";
			evaluationModel.Id = -1;

			return PartialView("EditorTemplates/SkillEvaluationModel", evaluationModel);
		}

		public IActionResult GetEvaluationModelsOptions(int? selectedId)
		{
			var evaluationModels = dbContext
				.SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel).ThenInclude(seml => seml.SkillLevelModel)
				.ToList()
				.Select(
					sem => SkillEvaluationModelViewModel.FromDbModel(sem)).ToList();

			evaluationModels.Insert(0, new SkillEvaluationModelViewModel { Id = null, Name = "<No model attached>" });

			var modelList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(evaluationModels, "Id", "DisplayName", selectedId);
			
			var view = this.PartialView("EditorTemplates/SelectOptions", modelList);

			return view;
		}

		public IActionResult SkillEvaluationModels(int? selectedId)
		{
			var evaluationModels = dbContext
				.SkillEvaluationModel.Include(sem => sem.SkillEvaluationModelLevel).ThenInclude(seml => seml.SkillLevelModel)
				.ToList()
				.Select(
					sem => SkillEvaluationModelViewModel.FromDbModel(sem)).ToList();

			var selectedModel = evaluationModels.SingleOrDefault(e => e.Id == selectedId);
			if (selectedModel != null)
			{
				selectedModel.Selected = true;
			}
			
			return PartialView("SkillEvaluationModels", evaluationModels);
		}

		public IActionResult AddEvaluationModel(List<ViewModels.SkillEvaluationModelViewModel> evaluationViewModels)
		{
			var newId = -1;

			if (evaluationViewModels.Any(e => e.Id != null && e.Id < 0))
			{
				newId = evaluationViewModels.Min(e => e.Id.Value) - 1;
			}

			evaluationViewModels.ForEach(e => e.Selected = false);

			var evaluationModel = new SkillEvaluationModelViewModel();
			evaluationModel.Name = "New model";
			evaluationModel.Id = newId;
			evaluationModel.Selected = true;
			
			evaluationViewModels.Add(evaluationModel);

			return PartialView("SkillEvaluationModels", evaluationViewModels);
		}

		public IActionResult DeleteEvaluationModel(int id)
		{
			try
			{
				var deletedEvaluationModelLevels = dbContext.SkillEvaluationModelLevel.Where(seml => seml.SkillEvaluationModelId == id).ToList();
				var deletedEvaluationModel = dbContext.SkillEvaluationModel.Include(m=>m.Skill).Single(m => m.Id == id);
				var deletedLevelModels = dbContext.SkillLevelModel.Where(sem => deletedEvaluationModelLevels.Select(eml=>eml.SkillLevelModelId).Contains(sem.Id)).ToList();
				var deletedSkillLevelCriteria = dbContext.SkillLevelCriteria.Where(slc => deletedEvaluationModelLevels.Select(eml => eml.SkillLevelModelId).Contains(slc.SkillLevelModelId)).ToList();

				foreach (var skill in deletedEvaluationModel.Skill)
				{
					skill.EvaluationModel = null;
				}

				dbContext.RemoveRange(deletedEvaluationModelLevels);
				dbContext.RemoveRange(deletedEvaluationModel);
				dbContext.RemoveRange(deletedSkillLevelCriteria);
				dbContext.RemoveRange(deletedLevelModels);

				var affected = dbContext.SaveChanges(true);
				Console.WriteLine(affected);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}

			return Ok();
		}
	}
}
