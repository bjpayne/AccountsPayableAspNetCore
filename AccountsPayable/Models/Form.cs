using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;

namespace AccountsPayable.Models
{
    public class Form
    {
        [Key]
        public Int32 form_id { get; set; }

        public String form_date { get; set; }

        public String form_campus { get; set; }

        public String form_status { get; set; }

        public String employee_id { get; set; }

        public String employee_first_name { get; set; }

        public String employee_last_name { get; set; }

        public Int32 department_id { get; set; }

        public Boolean AddFormRecord(IFormCollection form)
        {
            try
            {
                return true;
            } catch (Exception e)
            {
                EventLog.WriteEntry("Form.cs", e.ToString(), EventLogEntryType.Error);

                return false;
            }
        }
    }
}