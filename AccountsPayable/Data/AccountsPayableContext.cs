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

        public DbSet<Form> Form { get; set; }

        public DbSet<Mile> Mile { get; set; }

        public DbSet<Expenses> Expenses { get; set; }

        public DbSet<Receipt> Receipt { get; set; }
    }
}