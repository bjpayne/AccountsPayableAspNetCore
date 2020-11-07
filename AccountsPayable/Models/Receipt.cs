﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsPayable.Models
{
    public class Receipt
    {
        [Key]
        public int receipt_id { get; set; }

        public String receipt_receipt { get; set; }

    }
}