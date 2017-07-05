using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using CompetencyMatrix.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [Authorize]
    public class SkillCriteriaController : Controller
    {
		private CompetencyMatrixContext dbContext;

		public SkillCriteriaController(CompetencyMatrixContext context)
		{
			dbContext = context;
		}


		[HttpGet("api/[controller]")]
		public IEnumerable<SkillCriteria> Get()
		{
			return dbContext.SkillCriteria.ToList();
		}

		[HttpGet("api/[controller]/{id}")]
	    public SkillCriteria Get(int id)
	    {
		    return dbContext.SkillCriteria.SingleOrDefault(c => c.Id == id);
		}

		[HttpPost("api/[controller]")]
		public SkillCriteria Post(SkillCriteria criteria)
		{
			var exists = dbContext.SkillCriteria.Any(s => s.Id == criteria.Id);
			return dbContext.Upsert(criteria, !exists);
		}
	}
}
