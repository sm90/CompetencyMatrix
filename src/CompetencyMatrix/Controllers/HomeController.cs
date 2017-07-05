using System.Linq;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetencyMatrix.Controllers
{
    [SessionExpireFilter]
	public class HomeController : CompetencyMatrixBaseController
    {
        public HomeController(CompetencyMatrixContext context, IServerVariables serverVariables) : base(context, serverVariables)
        {
        }

        public IActionResult Index()
		{
			return View();
		}

        public IActionResult Skills()
		{
            ClearSessionAfterUseEmployeeSkill();
            return View();
		}


        [Authorize]
        public IActionResult PositionMatrices(int? id)
        {
                ClearSessionAfterUseEmployeeSkill();
            var positionMatrixList =
                PositionMatrixList.FromDbModel(DbContext.PositionMatrix.Where(x => x.IsDeleted == false).OrderBy(x => x.Name));
            
            var selectedMarix = positionMatrixList.SingleOrDefault(x => x.Id == id) ??
                                positionMatrixList.FirstOrDefault();
            if (selectedMarix != null)
            {
                selectedMarix.Selected = true;
                ViewData["SelectedItemId"] = selectedMarix.Id;
            }

            return View("PositionMatrices", positionMatrixList);
        }

        public IActionResult Employees()
        {
            return RedirectToRoute(new
            {
                controller = "Employees",
                action = "Index"
            });
        }

    }
}
