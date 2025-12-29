using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCtaegoryList = _db.Categories.ToList();
            return View(objCtaegoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name.");
            //}
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            //Category category = _db.Categories.Find(id);
            Category categoriesFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
            if (categoriesFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Update(categoriesFromDb);
            }
            return View(categoriesFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id) 
        {
            Category? categoryFroDb = _db.Categories.FirstOrDefault(u => u.Id == id);

            if (categoryFroDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Update(categoryFroDb);
            }

            return View(categoryFroDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? categoryFroDb = _db.Categories.FirstOrDefault(u => u.Id == id);

            if (categoryFroDb == null)
            {
                return NotFound();
            }

            
            _db.Categories.Remove(categoryFroDb);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
