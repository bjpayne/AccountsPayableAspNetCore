using Microsoft.EntityFrameworkCore;
using AccountsPayable.Models;

namespace AccountsPayable.Data
{
    public class AccountsPayableContext : DbContext
    {
        public AccountsPayableContext(DbContextOptions<AccountsPayableContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Department { get; set; }

        public DbSet<Employee> Form { get; set; }
    }
}