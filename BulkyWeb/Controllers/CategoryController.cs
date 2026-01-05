using Bulky.DataAccess.Data;
using Bulky.DataAccess.RepositoryFolder.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepositoryDb;

        public CategoryController(ICategoryRepository db)
        {
            this._categoryRepositoryDb = db;
        }
        public IActionResult Index()
        {
            List<Category> objCtaegoryList = _categoryRepositoryDb.GetAll().ToList();
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
                _categoryRepositoryDb.Add(obj);
                _categoryRepositoryDb.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            //Category category = _categoryRepositoryDb.Categories.Find(id);
            Category categoriesFromDb = _categoryRepositoryDb.Get(u => u.Id == id);
            if (categoriesFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _categoryRepositoryDb.Update(categoriesFromDb);
            }
            return View(categoriesFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepositoryDb.Update(obj);
                _categoryRepositoryDb.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id) 
        {
            Category? categoryFroDb = _categoryRepositoryDb.Get(u => u.Id == id);

            if (categoryFroDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _categoryRepositoryDb.Update(categoryFroDb);
            }

            return View(categoryFroDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? categoryFroDb = _categoryRepositoryDb.Get(u => u.Id == id);

            if (categoryFroDb == null)
            {
                return NotFound();
            }

            
            _categoryRepositoryDb.Remove(categoryFroDb);
            _categoryRepositoryDb.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
