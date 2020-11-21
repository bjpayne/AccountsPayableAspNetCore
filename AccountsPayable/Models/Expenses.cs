using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsPayable.Models
{
    public class Expenses
    {
        public Int32 id { get; set; }
        public String exp_date { get; set; }
        public String exp_description { get; set; }
        public String exp_fund { get; set; }
        public String exp_org { get; set; }
        public String exp_account { get; set; }
        public String exp_tax { get; set; }
        public String exp_amount { get; set;  }

        [ForeignKey("form_id")]
        public int form_id { get; set; }
        [ForeignKey("receipt_id")]
        public int receipt_id { get; set; }

    }
}
