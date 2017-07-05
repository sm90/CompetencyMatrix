using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CompetencyMatrix.Models
{
    partial class Skill
    {
	    public Skill Clone()
	    {
			var newSkill = new Skill();
		    
			newSkill.CategoryId = CategoryId;
			newSkill.Name = $"{Name}_copy";
			newSkill.Description = $"{Description}_copy";
			newSkill.TrainingMaterials = TrainingMaterials;
		    newSkill.EvaluationModelId = EvaluationModelId;

			return newSkill;
		}
	}
}
