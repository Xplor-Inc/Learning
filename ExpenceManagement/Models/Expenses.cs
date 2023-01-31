
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace ExpenceManagement.Models
{
    public class Expenses
    {
        public int Id { get; set; }
        public DateTime Date  { get; set; }
        public string Details { get; set; }
        public string Remarks { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int AccountId { get; set; }

        public Category Category { get; set; }
        public Account Account { get; set; }

    }


public class ExpenseModel
    {
        public Expenses Expenses { get; set; }
        public List<Category> Categories { get; set; }
        public List<Account> Accounts { get; set; }
    }
}


