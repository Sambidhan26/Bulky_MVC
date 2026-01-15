using Bulky.DataAccess.Data;
using Bulky.DataAccess.RepositoryFolder;
using Bulky.DataAccess.RepositoryFolder.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this._unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //included navigation properties
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            
            return View(objProductList);
        }
        public IActionResult UpdateCreate(int? id)
        {

            ProductViewModel productViewModel = new()
            {
                CategorySelectList = _unitOfWork.CategoryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Product = new Product()
            };

            if(id == 0 ||id == null)
            {
                //update
                return View(productViewModel);
            }
            else
            {
                //create
                productViewModel.Product = _unitOfWork.ProductRepository.Get(u => u.ProductId == id);
                return View(productViewModel);
            }

            //ViewBag.CategorySegmentList = categorySelectList;
            //ViewData["CategorySegmentList"] = categorySelectList;
        }
        [HttpPost]
        public IActionResult UpdateCreate(ProductViewModel productViewModel, IFormFile? file)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name.");
            //}

            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string producePath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(producePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productViewModel.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productViewModel.Product.ProductId == 0)
                {
                    _unitOfWork.ProductRepository.Add(productViewModel.Product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productViewModel.Product);
                }

                //_unitOfWork.ProductRepository.Add(productViewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productViewModel.CategorySelectList = _unitOfWork.CategoryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productViewModel);
            }

        }

        //public IActionResult Edit(int? id)
        //{
        //    //Product category = _categoryRepositoryDb.Categories.Find(id);
        //    Product productFromDb = _unitOfWork.ProductRepository.Get(u => u.ProductId == id);
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.ProductRepository.Update(productFromDb);
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.ProductRepository.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}

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
        #region API Calls
        [HttpGet]
        public IActionResult GetAllAPI()
        {
            //included navigation properties
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();

            return Json(new {data = objProductList});
        }
        #endregion
    }


}

