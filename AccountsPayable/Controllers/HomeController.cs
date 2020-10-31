using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AccountsPayable.Data;
using AccountsPayable.Models;
using Microsoft.Extensions.Primitives;
using System.Xml.Schema;

namespace AccountsPayable.Controllers
{
    public class HomeController : Controller
    {
        private readonly AccountsPayableContext _context;

        public HomeController(AccountsPayableContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _context.Department.ToListAsync();

            departments = departments.OrderBy(x => x.department_name).ToList();

            return View(departments);
        }

        // POST: /Create      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/HomeController/Create", Name = "Create")]
        public async Task<IActionResult> Create()
        {
            Microsoft.AspNetCore.Http.IFormCollection form = Request.Form;

            Employee employee = new Employee();

            if (form.TryGetValue("first_name", out StringValues firstName))
            {
                employee.employee_first_name = firstName;
            }

            if (form.TryGetValue("last_name", out StringValues lastName))
            {
                employee.employee_last_name = lastName;
            }

            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
