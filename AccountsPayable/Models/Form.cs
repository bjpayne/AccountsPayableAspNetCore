using System;
using System.ComponentModel.DataAnnotations;

namespace AccountsPayable.Models
{
    public class Form
    {
        public String reimbursement_total;

        [Key]
        public Int32 form_id { get; set; }

        public DateTime form_date { get; set; }

        public String form_campus { get; set; }

        public String form_status { get; set; }

        [Required]
        public String employee_id { get; set; }

        public String employee_first_name { get; set; }

        public String employee_last_name { get; set; }

        public Int32 department_id { get; set; }
    }
}