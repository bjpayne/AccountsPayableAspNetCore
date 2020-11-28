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

            foreach (Form pendingForm in pendingForms)
            {
                Decimal reimbursementTotal = 0;

                List<Mile> mileages = _context.Mile.Where(mile => mile.form_id == pendingForm.form_id).ToList();

                foreach (Mile mileage in mileages)
                {
                    Decimal mileageAmount = 0;

                    if (Decimal.TryParse(mileage.mile_amount, out mileageAmount))
                    {
                        reimbursementTotal = reimbursementTotal + mileageAmount;
                    }
                }

                List<Expenses> expenses = _context.Expenses.Where(expense => expense.form_id == pendingForm.form_id).ToList();

                foreach (Expenses expense in expenses)
                {
                    Decimal expenseAmount = 0;

                    if (Decimal.TryParse(expense.exp_amount, out expenseAmount))
                    {
                        reimbursementTotal = reimbursementTotal + expenseAmount;
                    }
                }

                pendingForm.reimbursement_total = String.Concat("$", reimbursementTotal);
            }

            List<Form> approvedForms = _context.Form.Where(form => form.form_status == "Approved").ToList();

            List<Form> deniedForms = _context.Form.Where(form => form.form_status == "Denied").ToList();

            ViewData["PendingForms"] = pendingForms;

            ViewData["ApprovedForms"] = approvedForms;

            ViewData["DeniedForms"] = deniedForms;

            return View();
        }

        // POST: /Edit      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Admin/Edit", Name = "Edit")]
        public async Task<IActionResult> Create()
        {

            /*form.form_status = "Pending Approval";

            _context.Add(form);

            _context.SaveChanges();

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));*/

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
