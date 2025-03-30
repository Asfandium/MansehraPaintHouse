using Microsoft.AspNetCore.Mvc;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductAttributeService _attributeService;
        private readonly IAttributeValueService _attributeValueService;
        private readonly IProductVariationService _productVariationService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IProductAttributeService attributeService,
            IAttributeValueService attributeValueService,
            IProductVariationService productVariationService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _attributeService = attributeService;
            _attributeValueService = attributeValueService;
            _productVariationService = productVariationService;
        }

        public async Task<IActionResult> ProductIndex(int? pageNumber, int? pageSize, string searchTerm)
        {
            int defaultPageSize = 8;
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? defaultPageSize;

            IQueryable<Product> query;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = await _productService.SearchProductsAsync(searchTerm);
            }
            else
            {
                query = await _productService.GetAllProductsQueryableAsync();
            }

            query = query.OrderByDescending(p => p.ProductID);
            var paginatedProducts = await PaginatedList<Product>.CreateAsync(query, currentPageNumber, currentPageSize);

            ViewData["CurrentFilter"] = searchTerm;
            return View(paginatedProducts);
        }

        public async Task<IActionResult> ProductUpsert(int? id)
        {
            var product = id == null || id == 0
                ? new Product()
                : await _productService.GetProductByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Attributes = await _attributeService.GetAllAttributesAsync();
            return PartialView("_ProductUpsert", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductUpsert(Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    product.Image = await SaveImageAsync(imageFile);
                }

                if (product.ProductID == 0)
                {
                    await _productService.CreateProductAsync(product);
                }
                else
                {
                    var existingProduct = await _productService.GetProductByIdAsync(product.ProductID);
                    if (existingProduct != null)
                    {
                        existingProduct.Name = product.Name;
                        existingProduct.Description = product.Description;
                        existingProduct.CategoryID = product.CategoryID;
                        existingProduct.SKU = product.SKU;
                        existingProduct.Price = product.Price;
                        existingProduct.StockQuantity = product.StockQuantity;
                        existingProduct.MinimumStockLevel = product.MinimumStockLevel;
                        existingProduct.IsActive = product.IsActive;
                        existingProduct.IsVariationProduct = product.IsVariationProduct;
                        existingProduct.Image = product.Image ?? existingProduct.Image;
                        await _productService.UpdateProductAsync(existingProduct);
                    }
                }

                return RedirectToAction("ProductIndex");
            }

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Attributes = await _attributeService.GetAllAttributesAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                await _productService.ToggleProductStatusAsync(id);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSubcategories(int categoryId)
        {
            var subcategories = await _categoryService.GetAllCategoriesAsync();
            var filteredSubcategories = subcategories.Where(c => c.ParentCategoryID == categoryId);
            return Json(filteredSubcategories);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributeValues(int attributeId)
        {
            var attributeValues = await _attributeValueService.GetAttributeValuesByAttributeAsync(attributeId);
            return Json(attributeValues);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, imageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            return $"/images/{imageFile.FileName}";
        }
    }
} 