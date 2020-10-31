using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountsPayable.Models
{
    public class Department
    {
        [Key]
        public int department_id { get; set; }
        public string department_name { get; set; }
    }
}
