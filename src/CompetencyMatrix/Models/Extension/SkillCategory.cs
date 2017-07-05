using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Core.v3;

namespace CompetencyMatrix.Models
{
    partial class SkillCategory
    {
	    public SkillCategory Clone()
	    {
			var newCategory = new SkillCategory();

			newCategory.ParentId = ParentId;
			newCategory.Name = $"{Name}_copy";
			newCategory.Description = $"{Description}_copy";

		    return newCategory;
	    }
	}
}
