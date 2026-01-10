using Bulky.DataAccess.Data;
using Bulky.DataAccess.RepositoryFolder;
using Bulky.DataAccess.RepositoryFolder.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
            return View(objProductList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name.");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            //Product category = _categoryRepositoryDb.Categories.Find(id);
            Product productFromDb = _unitOfWork.ProductRepository.Get(u => u.ProductId == id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(productFromDb);
            }
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            Product? categoryFroDb = _unitOfWork.ProductRepository.Get(u => u.ProductId == id);

            if (categoryFroDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(categoryFroDb);
            }

            return View(categoryFroDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? categoryFroDb = _unitOfWork.ProductRepository.Get(u => u.ProductId == id);

            if (categoryFroDb == null)
            {
                return NotFound();
            }


            _unitOfWork.ProductRepository.Remove(categoryFroDb);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");

        }
    }
}

