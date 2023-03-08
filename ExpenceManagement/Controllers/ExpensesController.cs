using ExpenceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Collections.Generic;

namespace ExpenceManagement.Controllers
{
    public class ExpensesController : Controller
    {
        ExpensesContext expensesContext = new ExpensesContext();
        public ActionResult Index(string Searchby, string search)
        {
            var expenses = expensesContext.Expenses.Include("Account").Include("Category").ToList();

            if (Searchby == "Details")
            {
                var model = expensesContext.Expenses.Where(exp => exp.Details == search || search == null).ToList();
                return View(model);
            }
            else
            {
                var model = expensesContext.Expenses.Where(exp => exp.Remarks.StartsWith(search) || search == null).ToList();
                return View(model);
            }
            return View(expenses);
        }

        public ActionResult Details(int id)
        {
            Expenses expenses = expensesContext.Expenses.Include("Account").Include("Category").FirstOrDefault(exp => exp.Id == id);
            return View(expenses);
           

        }
        [HttpPost]
        public ActionResult Create(Expenses expenses)
        {
            expensesContext.Expenses.Add(expenses);
            expensesContext.SaveChanges();

            var account = expensesContext.accounts.FirstOrDefault(e => e.Id == expenses.AccountId);
            if(account != null)
            {
                account.Amount = account.Amount - expenses.Amount;
                expensesContext.SaveChanges();
            }
            return Redirect("/Expenses");
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            var categories = expensesContext.Categories.ToList();
            var accounts = expensesContext.accounts.ToList();
            ExpenseModel model = new ExpenseModel()
            {
                Categories = categories,
                Accounts = accounts,
                Expenses = new Expenses()
            };
            return  View(model);
        }


        [HttpPost]
        public ActionResult Edit(Expenses expenses)
        { 
            var old = expensesContext.Expenses.FirstOrDefault(exp => exp.Id == expenses.Id);
            
            var account = expensesContext.accounts.FirstOrDefault(e => e.Id == old.AccountId);
            if (account != null)
            {
                account.Amount = account.Amount + old.Amount - expenses.Amount;
            }
            old.Remarks = expenses.Remarks;
            old.Details = expenses.Details;
            old.Amount = expenses.Amount;
            old.Date = expenses.Date;

            expensesContext.SaveChanges();
            return Redirect("/Expenses");

        }
        [HttpGet]
        public ActionResult Edit (int id) 
        {
            var raw = expensesContext.Expenses.FirstOrDefault(exp => exp.Id == id);
            return View(raw);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            var std = expensesContext.Expenses.FirstOrDefault(exp => exp.Id == id);
            if(std == null)
            {
              //  return View("Error");
            }
            var dft = expensesContext.Expenses.Remove(std);
            expensesContext.SaveChanges();
            
             var account = expensesContext.accounts.FirstOrDefault(e => e.Id == std.AccountId);
            if (account != null)
            {
                account.Amount = account.Amount + std.Amount;
                expensesContext.SaveChanges();
            }
            return Redirect("/Expenses");
        }
                
    }
}
