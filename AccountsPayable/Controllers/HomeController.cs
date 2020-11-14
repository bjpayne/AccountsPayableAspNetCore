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
using System.IO;

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

            _context.SaveChanges();

            StoreMileage(form.form_id);

            StoreExpenses(form.form_id);

            _context.SaveChanges();

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

        private void StoreMileage(Int32 formID)
        {
            Microsoft.AspNetCore.Http.IFormCollection request = Request.Form;

            Int32 i = 0;

            foreach (String key in request.Keys)
            {
                if (key.IndexOf($"mileage_reimbursements[{i}]") != -1)
                {
                    Mile mileage = new Mile();

                    mileage.form_id = formID;

                    if (request.TryGetValue($"mileage_reimbursements[{i}][date]", out StringValues mileageReimbursementDate))
                    {

                        mileage.mile_date = mileageReimbursementDate;
                    }

                    if (request.TryGetValue($"mileage_reimbursements[{i}][destination]", out StringValues mileageReimbursementDestination))
                    {

                        mileage.mile_description = mileageReimbursementDestination;
                    }

                    if (request.TryGetValue($"mileage_reimbursements[{i}][miles]", out StringValues mileageReimbursementMiles))
                    {

                        Int32 parsedMiles = 0;

                        if (Int32.TryParse(mileageReimbursementMiles, out parsedMiles))
                        {
                            mileage.mile_miles = parsedMiles;
                        }
                    }

                    if (request.TryGetValue($"mileage_reimbursements[{i}][purpose]", out StringValues mileageReimbursementPurpose))
                    {
                        mileage.mile_purpose = mileageReimbursementPurpose;
                    }


                    if (request.TryGetValue($"mileage_reimbursements[{i}][fund]", out StringValues mileageReimbursementFund))
                    {

                        mileage.mile_fund = mileageReimbursementFund;
                    }


                    if (request.TryGetValue($"mileage_reimbursements[{i}][organization]", out StringValues mileageReimbursementOrg))
                    {

                        mileage.mile_org = mileageReimbursementOrg;
                    }


                    if (request.TryGetValue($"mileage_reimbursements[{i}][account]", out StringValues mileageReimbursementAccount))
                    {

                        mileage.mile_account = mileageReimbursementAccount;
                    }


                    if (request.TryGetValue($"mileage_reimbursements[{i}][amount]", out StringValues mileageReimbursementAmount))
                    {

                        mileage.mile_amount = mileageReimbursementAmount;
                    }

                    i++;

                    _context.Add(mileage);
                }
            }
        }

        private void StoreExpenses(Int32 formID)
        {
            Microsoft.AspNetCore.Http.IFormCollection request = Request.Form;

            Int32 i = 0;

            foreach (String key in request.Keys)
            {
                if (key.IndexOf($"other_expenses[{i}]") != -1)
                {
                    Expenses expenses = new Expenses();

                    expenses.form_id = formID;

                    if (request.TryGetValue($"other_expenses[{i}][exp_date]", out StringValues otherExpensesDate))
                    {
                        expenses.exp_date = otherExpensesDate;
                    }

                    if (request.TryGetValue($"other_expenses[{i}][exp_description]", out StringValues otherExpensesDescription))
                    {
                        expenses.exp_description = otherExpensesDescription;
                    }

                    if (request.TryGetValue($"other_expenses[{i}][exp_fund]", out StringValues otherExpensesFund))
                    {
                        expenses.exp_fund = otherExpensesFund;
                    }

                    if (request.TryGetValue($"other_expenses[{i}][exp_org]", out StringValues otherExpensesOrg))
                    {
                        expenses.exp_org = otherExpensesOrg;
                    }

                    if (request.TryGetValue($"other_expenses[{i}][exp_account]", out StringValues otherExpensesAccount))
                    {
                        expenses.exp_account = otherExpensesAccount;
                    }

                    if (request.TryGetValue($"other_expenses[{i}][exp_amount]", out StringValues otherExpensesAmount))
                    {
                        expenses.exp_amount = otherExpensesAmount;
                    }

                    i++;

                    _context.Add(expenses);
                }
            }
        }

        public interface IFormFile
        {
            int Length { get; }

            string GetFilename();

            Task CopyToAsync(FileStream stream);

        }

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (IActionResult)Content("file not selected!");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        file.GetFilename());



            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (IActionResult)RedirectToAction("Files");
        }


    }
}
