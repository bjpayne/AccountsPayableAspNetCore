using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsPayable.Data;
using AccountsPayable.Models;
using Microsoft.Extensions.Primitives;

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
            Microsoft.AspNetCore.Http.IFormCollection request = Request.Form;

            Form form = new Form();

            if (request.TryGetValue("employee_id", out StringValues employeeId))
            {
                form.employee_id = employeeId;
            }

            if (request.TryGetValue("first_name", out StringValues employeeFirstName))
            {
                form.employee_first_name = employeeFirstName;
            }

            if (request.TryGetValue("last_name", out StringValues employeeLastName))
            {
                form.employee_last_name = employeeLastName;
            }

            if (request.TryGetValue("date", out StringValues formDate))
            {
                form.form_date = formDate;
            }

            if (request.TryGetValue("department", out StringValues departmentId))
            {
                form.department_id = Int32.Parse(departmentId);
            }

            if (request.TryGetValue("home_campus", out StringValues formCampus))
            {
                form.form_campus = formCampus;
            }

            form.form_status = "Pending Approval";

            _context.Add(form);

            await _context.SaveChangesAsync();

            StoreMileage();

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

        private void StoreMileage()
        {
            Microsoft.AspNetCore.Http.IFormCollection request = Request.Form;

            // String[] mileages = Request.Form.TryGetValue("")

            if (request.TryGetValue("mileage_reimbursements_date", out StringValues milageReimburesements))
            {
                Mile mileage = new Mile();

                mileage.mile_date = milageReimburesements[0];
            }            
        }
    }
}
