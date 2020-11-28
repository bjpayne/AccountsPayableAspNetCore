using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsPayable.Models
{
    public class Mile
    {
        [Key]
        public int id { get; set; }

        public DateTime mile_date { get; set; }
        public Decimal mile_miles { get; set; }
        public String mile_description { get; set; }
        public String mile_fund { get; set; }
        public String mile_org { get; set; }
        public String mile_account { get; set; }
        public String mile_amount { get; set; }
        public String mile_tax { get; set; }
        public String mile_purpose { get; set; }

        [ForeignKey("form_id")]
        public Int32 form_id { get; set; }


    }
}
