using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsPayable.Models
{
    public class ApprovalList
    {
        [Key]
        public int app_id { get; set; }
        
        public int app_fund { get; set; }
        public int app_org { get; set; }
        public int app_fund_org { get; set; }
        public char app_dept { get; set; }
        public String app_budget_mang { get; set; }
        public char app_lead { get; set; }
        public String app_add_app { get; set; }
        public String app_add_app2 { get; set; }
        public String app_add_app3 { get; set; }
        public String app_finac_mang { get; set; }
    }
}
