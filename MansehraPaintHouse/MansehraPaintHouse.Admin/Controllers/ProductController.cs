////using MansehraPaintHouse.Core.Entities;
////using MansehraPaintHouse.Core.Interfaces.IServices;
////using Microsoft.AspNetCore.Mvc;

////namespace MansehraPaintHouse.Admin.Controllers
////{
////    public class ProductController : Controller
////    {
////        private readonly IProductService _productService;

////        public ProductController(IProductService productService)
////        {
////            _productService = productService;
////        }

////        public async Task<IActionResult> ProductIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
////        {
////            var products = await _productService.SearchProductsAsync(searchTerm);
////            var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageNumber, pageSize);
////            ViewBag.SearchTerm = searchTerm;
////            return View(paginatedList);
////        }

////        public async Task<IActionResult> ProductUpsert(int? id)
////        {
////            if (id == null)
////            {
////                return View(new Product());
////            }

////            var product = await _productService.GetProductByIdAsync(id.Value);
////            if (product == null)
////            {
////                return NotFound();
////            }

////            return View(product);
////        }

////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> ProductUpsert(Product product)
////        {
////            if (ModelState.IsValid)
////            {
////                if (product.ProductID == 0)
////                {
////                    await _productService.CreateProductAsync(product);
////                }
////                else
////                {
////                    await _productService.UpdateProductAsync(product);
////                }

////                return RedirectToAction(nameof(ProductIndex));
////            }

////            return View(product);
////        }

////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> DeleteProduct(int id)
////        {
////            await _productService.DeleteProductAsync(id);
////            return RedirectToAction(nameof(ProductIndex));
////        }

////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> ToggleProductStatus(int id)
////        {
////            await _productService.ToggleProductStatusAsync(id);
////            return RedirectToAction(nameof(ProductIndex));
////        }
////    }
////}


//using MansehraPaintHouse.Core.Entities;
//using MansehraPaintHouse.Core.Interfaces.IServices;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace MansehraPaintHouse.Admin.Controllers
//{
//    public class ProductController : Controller
//    {
//        private readonly IProductService _productService;
//        private readonly ICategoryService _categoryService;

//        public ProductController(IProductService productService, ICategoryService categoryService)
//        {
//            _productService = productService;
//            _categoryService = categoryService;
//        }

//        public async Task<IActionResult> ProductIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
//        {
//            var products = await _productService.SearchProductsAsync(searchTerm);
//            var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageNumber, pageSize);
//            ViewBag.SearchTerm = searchTerm;
//            return View(paginatedList);
//        }

//        public async Task<IActionResult> ProductUpsert(int? id)
//        {
//            var categories = await _categoryService.GetAllCategoriesAsync();
//            ViewBag.Categories = new SelectList(categories.Where(c => c.ParentCategoryID == null), "CategoryID", "Name");

//            if (id == null)
//            {
//                return View(new Product());
//            }

//            var product = await _productService.GetProductByIdAsync(id.Value);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ProductUpsert(Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                if (product.ProductID == 0)
//                {
//                    await _productService.CreateProductAsync(product);
//                }
//                else
//                {
//                    await _productService.UpdateProductAsync(product);
//                }

//                return RedirectToAction(nameof(ProductIndex));
//            }

//            var categories = await _categoryService.GetAllCategoriesAsync();
//            ViewBag.Categories = new SelectList(categories.Where(c => c.ParentCategoryID == null), "CategoryID", "Name");

//            return View(product);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteProduct(int id)
//        {
//            await _productService.DeleteProductAsync(id);
//            return RedirectToAction(nameof(ProductIndex));
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ToggleProductStatus(int id)
//        {
//            await _productService.ToggleProductStatusAsync(id);
//            return RedirectToAction(nameof(ProductIndex));
//        }
//    }
//}




using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> ProductIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productService.SearchProductsAsync(searchTerm);
            var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageNumber, pageSize);
            ViewBag.SearchTerm = searchTerm;
            return View(paginatedList);
        }

        //public async Task<IActionResult> ProductUpsert(int? id)
        //{
        //    var categories = await _categoryService.GetAllCategoriesAsync();
        //    ViewBag.Categories = new SelectList(categories.Where(c => c.ParentCategoryID == null), "CategoryID", "Name");

        //    if (id == null)
        //    {
        //        return View(new Product());
        //    }

        //    var product = await _productService.GetProductByIdAsync(id.Value);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        public async Task<IActionResult> ProductUpsert(int? id)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories.Where(c => c.ParentCategoryID == null), "CategoryID", "Name");

            if (id == null)
            {
                return View(new Product());
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductUpsert(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.Image = "/images/" + fileName;
                }

                if (product.ProductID == 0)
                {
                    await _productService.CreateProductAsync(product);
                }
                else
                {
                    await _productService.UpdateProductAsync(product);
                }

                return RedirectToAction(nameof(ProductIndex));
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories.Where(c => c.ParentCategoryID == null), "CategoryID", "Name");

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleProductStatus(int id)
        {
            await _productService.ToggleProductStatusAsync(id);
            return RedirectToAction(nameof(ProductIndex));
        }
    }
}
