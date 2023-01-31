
using System.Data.Entity;

namespace ExpenceManagement.Models
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext() : base("Server=XPLOR-INC;Database=Expenses;user Id=sa;password=sa;")
        {

        }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Account> accounts { get; set; }
    }
}
