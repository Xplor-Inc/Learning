using ExpenceManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenceManagement.Controllers
{
    public class CategoryController : Controller
    {
        ExpensesContext expensesContext = new ExpensesContext();
        public ActionResult Index()
        {
            List<Category> categories = expensesContext.Categories.ToList();
            return View(categories);
        }

        public ActionResult Details(int id)
        {
            Category categoris = expensesContext.Categories.FirstOrDefault(exp => exp.Id == id);
            return View(categoris);
        }


        [HttpPost]
        public ActionResult Create(Category categories)
        {
            var category = expensesContext.Categories.Add(categories);
            expensesContext.SaveChanges();
            var nnen = expensesContext.Categories.ToList();

            return Redirect("/Category");
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            return View();
        }

        public ActionResult Delete(int id)
        {

            var category = expensesContext.Categories.FirstOrDefault(exp => exp.Id == id);
            expensesContext.Categories.Remove(category);
            expensesContext.SaveChanges();
            var categories = expensesContext.Categories.ToList();
            return View("Index", categories);
        }

    }
}
