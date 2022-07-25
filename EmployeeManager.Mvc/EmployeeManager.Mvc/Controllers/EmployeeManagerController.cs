using EmployeeManager.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManager.Mvc.Controllers
{
    public class EmployeeManagerController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private AppDbContext db;
        public EmployeeManagerController(AppDbContext _db)
        {
            this.db = _db;
        }
        private void FillCountries() 
        {
            List<SelectListItem> countries = 
                (from c in db.Countries
                 orderby c.Name ascending
                 select new SelectListItem()
                 { Text=c.Name, Value=c.Name}).ToList();
            ViewBag.Countries = countries;
        }
        public IActionResult List()
        {
            List<Employee> model = (from e in db.Employees
                                    orderby e.EmployeeID
                                    select e ).ToList();
            return View(model);
        }
        public IActionResult Insert()
        {
            FillCountries();
            return View();

        }
        [HttpPost]
        public IActionResult Insert(Employee model)
        {
            FillCountries();
            if(ModelState.IsValid)
            {
                db.Employees.Add(model);
                db.SaveChanges();
                ViewBag.Message = "Employee Inserted Successfully";
            }
            return View();

        }
        public IActionResult Update(int id)
        {
            FillCountries();
            Employee model = db.Employees.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Employee model)
        {
            FillCountries();
            if(ModelState.IsValid)
            {
                db.Employees.Update(model);
                db.SaveChanges();
                ViewBag.Message = "Employeee Updated Successfully";
            }
            return View(model);
        }

        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            Employee model = db.Employees.Find(id);
            return View(model);
        }

        [HttpPost] 
        public IActionResult Delete(int employeeID) 
        { 
            Employee model = db.Employees.Find(employeeID); 
            db.Employees.Remove (model); 
            db.SaveChanges(); 
            TempData["Message"] = "Employee deleted successfully"; 
            return RedirectToAction("List"); }
    }
}
