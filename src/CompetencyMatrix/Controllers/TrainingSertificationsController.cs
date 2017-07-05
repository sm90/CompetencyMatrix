using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompetencyMatrix.Models;
using CompetencyMatrix.ViewModels;
using CompetencyMatrix.Infrastructure;

namespace CompetencyMatrix.Controllers
{
    [SessionExpireFilter]
    public class TrainingSertificationsController : Controller
    {
        private readonly CompetencyMatrixContext _context;

        public TrainingSertificationsController(CompetencyMatrixContext context)
        {
            _context = context;    
        }

        // GET: TrainingSertifications
        public IActionResult Index(int employeeId)
        {
            return View(List(employeeId));
        }

        public List<TrainingSertificationModel> List(int employeeId)
        {
            var data = _context.TrainingSertification
                .Where( x => x.EmployeeId == employeeId)
                .Include(x => x.Type);

            var result = data.Select(x => TrainingSertificationModel.FromDbModel(x))
                .OrderByDescending(x => x.When)
                .ToList();

            return result;
        }


        // GET: TrainingSertifications/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var trainingSertification = await _context.TrainingSertification.SingleOrDefaultAsync(m => m.Id == id);
        //    if (trainingSertification == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(trainingSertification);
        //}

        // GET: TrainingSertifications/Create
        public IActionResult Update(int id, int employeeId)
        {
            var model = new TrainingSertificationModel();

            if (id > 0)
            {
                var trainingSertification = _context.TrainingSertification.SingleOrDefault(m => m.Id == id);
                if (trainingSertification == null)
                {
                    return NotFound();
                }

                model = TrainingSertificationModel.FromDbModel(trainingSertification);
            }
            else
            {
                model.When = DateTime.Now;
                model.EmployeeId = employeeId;
            }            

            //ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.Set<Models.TrainingSertificationType>(), "Id", "Name");
            return PartialView("Update", model);
        }

        // POST: TrainingSertifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(TrainingSertificationModel trainingSertification)
        {
            if (ModelState.IsValid)
            {
                if (trainingSertification.Id == 0)
                {
                    _context.TrainingSertification.Add(TrainingSertification.FromModel(trainingSertification));
                }
                else
                {
                    _context.TrainingSertification.Update(TrainingSertification.FromModel(trainingSertification));
                }
                
                _context.SaveChanges();

                return RedirectToAction("Index", new { employeeId = trainingSertification.EmployeeId });
            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name", trainingSertification.EmployeeId);
            //ViewData["TypeId"] = new SelectList(_context.Set<Models.TrainingSertificationType>(), "Id", "Id", trainingSertification.TypeId);
            return PartialView(trainingSertification);
        }

        // GET: TrainingSertifications/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var trainingSertification = await _context.TrainingSertification.SingleOrDefaultAsync(m => m.Id == id);
        //    if (trainingSertification == null)
        //    {
        //        return NotFound();
        //    }
        //    //ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name", trainingSertification.EmployeeId);
        //    ViewData["TypeId"] = new SelectList(_context.Set<Models.TrainingSertificationType>(), "Id", "Id", trainingSertification.TypeId);
        //    return View("Update", trainingSertification);
        //}

        // POST: TrainingSertifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,Name,TypeId,When")] TrainingSertification trainingSertification)
        //{
        //    if (id != trainingSertification.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(trainingSertification);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TrainingSertificationExists(trainingSertification.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name", trainingSertification.EmployeeId);
        //    ViewData["TypeId"] = new SelectList(_context.Set<Models.TrainingSertificationType>(), "Id", "Id", trainingSertification.TypeId);
        //    return View(trainingSertification);
        //}


        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> Delete(int id)
        {
            var trainingSertification = await _context.TrainingSertification.SingleOrDefaultAsync(m => m.Id == id);
            _context.TrainingSertification.Remove(trainingSertification);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //private bool TrainingSertificationExists(int id)
        //{
        //    return _context.TrainingSertification.Any(e => e.Id == id);
        //}
    }
}
