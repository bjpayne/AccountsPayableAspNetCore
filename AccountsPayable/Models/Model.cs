using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountsPayable.Models
{
    public class Model
    {
        public String GetConnectionString()
        {
            return "Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;MultipleActiveResultSets=true";
        }
    }
}

