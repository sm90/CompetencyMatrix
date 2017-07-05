using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using System.ComponentModel.DataAnnotations;

namespace CompetencyMatrix.ViewModels
{
    public class SkillEvaluationModelViewModel
    {
		public int? Id { get; set; }

        [Required]
		public string Name { get; set; }
		public string Description { get; set; }

		public bool Selected { get; set; }

		public string DisplayName
	    {
		    get
		    {
			    if (Id == null)
			    {
				    return Name;
			    }

			    if (Levels == null || !Levels.Any())
			    {
					return $"{Name} [no levels]";
			    }

			    var displayName = $"{Name} [{string.Join(", ", Levels.OrderBy(l=>l.Quality).Select(l => l.Name))}]";
			    return displayName;
			}
	    }

	    public List<SkillLevelModelViewModel> Levels { get; set; } = new List<SkillLevelModelViewModel>();

		public SkillLevelModelViewModel CurrentLevel { get; set; }

		public static SkillEvaluationModelViewModel FromDbModel(SkillEvaluationModel dbSkillEvaluationModel)
	    {
		    var viewModel = new SkillEvaluationModelViewModel();

		    viewModel.Id = dbSkillEvaluationModel.Id;
		    viewModel.Name = dbSkillEvaluationModel.Name;
		    viewModel.Description = dbSkillEvaluationModel.Description;

		    viewModel.Levels =
				dbSkillEvaluationModel.SkillEvaluationModelLevel.Select(seml => SkillLevelModelViewModel.FromDbModel(seml.SkillLevelModel, dbSkillEvaluationModel)).OrderBy(e=>e.Quality).ToList();

		    return viewModel;
	    }
    }

	public class SkillLevelModelViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Quality { get; set; }

		public static SkillLevelModelViewModel FromDbModel(SkillLevelModel dbSkillLevelModel, SkillEvaluationModel dbSkillEvaluationModel)
		{
			var viewModel = new SkillLevelModelViewModel();
			viewModel.Id = dbSkillLevelModel.Id;
			viewModel.Name = dbSkillLevelModel.Name;
			viewModel.Description = dbSkillLevelModel.Description;
			viewModel.Quality = dbSkillLevelModel.Quality;

			return viewModel;
		}
	}
}
