using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace AccountsPayable.Models
{
    public class Form
    {
        [Key]
        public Int32 form_id { get; set; }

        public String form_data { get; set; }

        public String form_campus { get; set; }

        public String form_status { get; set; }

        [ForeignKey("Employee")]
        public Int32 employee_id { get; set; }

        public Int32 department_id { get; set; }
    }
}