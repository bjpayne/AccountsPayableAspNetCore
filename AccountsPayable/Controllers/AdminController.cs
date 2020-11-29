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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace AccountsPayable.Controllers
{
    public class AdminController : Controller
    {
        private readonly AccountsPayableContext _context;

        public AdminController(AccountsPayableContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Form> pendingForms = _context.Form.Where(form => form.form_status == "Pending Approval").ToList();

            List<Form> approvedForms = _context.Form.Where(form => form.form_status == "Approved").ToList();

            List<Form> deniedForms = _context.Form.Where(form => form.form_status == "Denied").ToList();

            CalculateTotalReimbursements(pendingForms);

            CalculateTotalReimbursements(approvedForms);

            CalculateTotalReimbursements(deniedForms);

            ViewData["PendingForms"] = pendingForms;

            ViewData["ApprovedForms"] = approvedForms;

            ViewData["DeniedForms"] = deniedForms;

            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Department> departments = await _context.Department.ToListAsync();

            departments = departments.OrderBy(x => x.department_name).ToList();

            ViewData["Departments"] = departments;

            Form form = await _context.Form.FindAsync(id);

            List<Mile> mileages = _context.Mile.Where(mile => mile.form_id == id).ToList();

            List<Expenses> expenses = _context.Expenses.Where(expenses => expenses.form_id == id).ToList();

            if (expenses.Count > 0)
            {
                Receipt receipt = _context.Receipt.Where(receipt => receipt.receipt_id == expenses[0].receipt_id).First();

                ViewData["receipt"] = receipt;
            }

            ViewData["form"] = form;

            ViewData["mileages"] = mileages;

            ViewData["expenses"] = expenses;

            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Form form = await _context.Form.FindAsync(id);

            IFormCollection request = Request.Form;

            if (request.TryGetValue("form_status", out StringValues formStatus))
            {
                form.form_status = formStatus.ToString();

                _context.Update(form);

                _context.SaveChanges();
            }

            List<Mile> mileages = _context.Mile.Where(mile => mile.form_id == id).ToList();

            foreach (Mile mile in mileages)
            {
                if (request.TryGetValue($"mileage_reimbursements[{mile.id}][mile_tax]", out StringValues mileTax))
                {
                    mile.mile_tax = "yes";
                }
                else
                {
                    mile.mile_tax = "no";
                }

                _context.Update(mile);

                _context.SaveChanges();
            }

            List<Expenses> expenses = _context.Expenses.Where(expense => expense.form_id == id).ToList();

            foreach (Expenses expense in expenses)
            {
                if (request.TryGetValue($"other_expenses[{expense.id}][exp_tax]", out StringValues expTax))
                {
                    expense.exp_tax = "yes";
                }
                else
                {
                    expense.exp_tax = "no";
                }

                _context.Update(expense);

                _context.SaveChanges();
            }

            if (form.form_status == "Approved")
            {
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Body"] = "Expense form approved.";
            }

            if (form.form_status == "Denied")
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Body"] = "Expense form denied.";
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void CalculateTotalReimbursements(List<Form> forms)
        {
            foreach (Form form in forms)
            {
                Decimal reimbursementTotal = 0;

                List<Mile> mileages = _context.Mile.Where(mile => mile.form_id == form.form_id).ToList();

                foreach (Mile mileage in mileages)
                {
                    Decimal mileageAmount = 0;

                    if (Decimal.TryParse(mileage.mile_amount, out mileageAmount))
                    {
                        reimbursementTotal = reimbursementTotal + mileageAmount;
                    }
                }

                List<Expenses> expenses = _context.Expenses.Where(expense => expense.form_id == form.form_id).ToList();

                foreach (Expenses expense in expenses)
                {
                    Decimal expenseAmount = 0;

                    if (Decimal.TryParse(expense.exp_amount, out expenseAmount))
                    {
                        reimbursementTotal = reimbursementTotal + expenseAmount;
                    }
                }

                form.reimbursement_total = String.Concat("$", reimbursementTotal);
            }
        }

        public FileResult Download(int id)
        {
            Receipt receipt = _context.Receipt.Find(id);

            return File(receipt.receipt_receipt, "application/pdf");
        }
    }
}
