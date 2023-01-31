using ExpenceManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenceManagement.Controllers
{
    public class AccountController : Controller
    {
        ExpensesContext expensesContext = new ExpensesContext();
        public ActionResult Index()
        {
            List<Account> accounts = expensesContext.accounts.ToList();
            return View(accounts);
        }

        public ActionResult Details( int id)
        {
            Account account = expensesContext.accounts.FirstOrDefault(act => act.Id == id);
            return View(account);
        }


        [HttpPost]
        public ActionResult  Create (Account account) 
        {
            var Account = expensesContext.accounts.Add(account);
            expensesContext.SaveChanges();

            return Redirect("/Account");
        }

        [HttpGet]
        public ActionResult Create (int id)
        {
            var Account = expensesContext.accounts.ToList();


            return View();
        }
        public ActionResult Delete(int id)
        {
            var rfg = expensesContext.accounts.FirstOrDefault(act => act.Id == id);
            expensesContext.accounts.Remove(rfg);
            expensesContext.SaveChanges();

            return Redirect("/Account");
        }
        
    }
}
