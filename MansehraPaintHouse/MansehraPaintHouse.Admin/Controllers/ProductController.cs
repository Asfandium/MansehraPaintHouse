using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            IProductService productService, 
            ICategoryService categoryService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> ProductIndex(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productService.SearchProductsAsync(searchTerm);
            
            // Include the Category navigation property
            products = products.Include(p => p.Category);
            
            var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageNumber, pageSize);
            ViewBag.SearchTerm = searchTerm;
            return View(paginatedList);
        }

        public async Task<IActionResult> ProductUpsert(int? id)
        {
            try
            {
                // Get all categories
                var allCategories = await _categoryService.GetAllCategoriesAsync();
                System.Diagnostics.Debug.WriteLine($"Retrieved {allCategories.Count()} total categories");

                // Get parent categories (where ParentCategoryID is null)
                var parentCategories = allCategories.Where(c => c.ParentCategoryID == null).ToList();
                System.Diagnostics.Debug.WriteLine($"Found {parentCategories.Count} parent categories");

                // Set up the parent categories dropdown
                ViewBag.Categories = new SelectList(parentCategories, "CategoryID", "Name");

                // Set up the subcategories dropdown (initially empty)
                ViewBag.Subcategories = new SelectList(Enumerable.Empty<Category>(), "CategoryID", "Name");

                if (id == null)
                {
                    System.Diagnostics.Debug.WriteLine("Creating new product");
                    return View(new Product
                    {
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    });
                }

                var product = await _productService.GetProductByIdAsync(id.Value);
                if (product == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Product with ID {id} not found");
                    return NotFound();
                }

                System.Diagnostics.Debug.WriteLine($"Editing product with ID {id}, CategoryID: {product.CategoryID}");

                // If editing, populate the subcategories dropdown based on the selected category
                if (product.CategoryID > 0)
                {
                    // Check if the selected category is a parent category
                    var isParentCategory = parentCategories.Any(c => c.CategoryID == product.CategoryID);
                    
                    if (isParentCategory)
                    {
                        // If it's a parent category, populate subcategories
                        var subcategories = allCategories.Where(c => c.ParentCategoryID == product.CategoryID).ToList();
                        System.Diagnostics.Debug.WriteLine($"Found {subcategories.Count} subcategories for product");
                        ViewBag.Subcategories = new SelectList(subcategories, "CategoryID", "Name");
                    }
                    else
                    {
                        // If it's a subcategory, find its parent and select it in the category dropdown
                        var parentCategory = allCategories.FirstOrDefault(c => c.CategoryID == product.CategoryID);
                        if (parentCategory != null)
                        {
                            // Get subcategories for the parent category
                            var subcategories = allCategories.Where(c => c.ParentCategoryID == parentCategory.CategoryID).ToList();
                            System.Diagnostics.Debug.WriteLine($"Found {subcategories.Count} subcategories for parent category {parentCategory.CategoryID}");
                            ViewBag.Subcategories = new SelectList(subcategories, "CategoryID", "Name");
                            
                            // Set the parent category as selected
                            ViewBag.SelectedCategoryID = parentCategory.CategoryID;
                        }
                    }
                }

                return View(product);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ProductUpsert: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return View(new Product());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductUpsert(Product product, IFormFile imageFile)
        {
            System.Diagnostics.Debug.WriteLine("ProductUpsert POST action called");
            System.Diagnostics.Debug.WriteLine($"Product ID: {product.ProductID}, Name: {product.Name}, CategoryID: {product.CategoryID}");
            
            // Remove any validation errors for the Image field since it's optional
            ModelState.Remove("Image");
            
            // Remove validation errors for the Category navigation property
            ModelState.Remove("Category");
            
            // Check if CategoryID is 0 or null
            if (product.CategoryID == 0)
            {
                ModelState.AddModelError("CategoryID", "Please select a category");
                System.Diagnostics.Debug.WriteLine("CategoryID is 0, adding validation error");
            }
            
            // Log validation errors
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Model validation failed:");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Validation error: {error.ErrorMessage}");
                    }
                }
                
                // Repopulate the dropdowns
                var allCategories = await _categoryService.GetAllCategoriesAsync();
                var parentCategories = allCategories.Where(c => c.ParentCategoryID == null).ToList();
                ViewBag.Categories = new SelectList(parentCategories, "CategoryID", "Name");
                
                // Check if the product's category is a parent or subcategory
                if (product.CategoryID > 0)
                {
                    var isParentCategory = parentCategories.Any(c => c.CategoryID == product.CategoryID);
                    
                    if (isParentCategory)
                    {
                        // If it's a parent category, populate subcategories
                        var subcategories = allCategories.Where(c => c.ParentCategoryID == product.CategoryID).ToList();
                        ViewBag.Subcategories = new SelectList(subcategories, "CategoryID", "Name");
                    }
                    else
                    {
                        // If it's a subcategory, find its parent and select it in the category dropdown
                        var parentCategory = allCategories.FirstOrDefault(c => c.CategoryID == product.CategoryID);
                        if (parentCategory != null)
                        {
                            // Get subcategories for the parent category
                            var subcategories = allCategories.Where(c => c.ParentCategoryID == parentCategory.CategoryID).ToList();
                            ViewBag.Subcategories = new SelectList(subcategories, "CategoryID", "Name");
                            
                            // Set the parent category as selected
                            ViewBag.SelectedCategoryID = parentCategory.CategoryID;
                        }
                    }
                }
                else
                {
                    ViewBag.Subcategories = new SelectList(Enumerable.Empty<Category>(), "CategoryID", "Name");
                }
                
                return View(product);
            }
            
            try
            {
                System.Diagnostics.Debug.WriteLine("Model is valid, processing...");
                
                // Handle image upload only if a new image is provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"New image provided: {imageFile.FileName}, Size: {imageFile.Length} bytes");
                    
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    product.Image = $"/images/products/{uniqueFileName}";
                    System.Diagnostics.Debug.WriteLine($"Image saved to: {product.Image}");
                }
                else if (product.ProductID > 0)
                {
                    // If editing and no new image is provided, preserve the existing image
                    var existingProduct = await _productService.GetProductByIdAsync(product.ProductID);
                    if (existingProduct != null)
                    {
                        product.Image = existingProduct.Image;
                        System.Diagnostics.Debug.WriteLine($"Preserving existing image: {product.Image}");
                    }
                }

                product.UpdatedAt = DateTime.Now;

                if (product.ProductID == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Creating new product");
                    product.CreatedAt = DateTime.Now;
                    await _productService.CreateProductAsync(product);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Updating product with ID: {product.ProductID}");
                    await _productService.UpdateProductAsync(product);
                }

                System.Diagnostics.Debug.WriteLine("Product saved successfully, redirecting to index");
                return RedirectToAction(nameof(ProductIndex));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving product: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Add error message to ModelState
                ModelState.AddModelError("", "An error occurred while saving the product. Please try again.");
                
                // Repopulate the dropdowns
                var allCategories = await _categoryService.GetAllCategoriesAsync();
                var parentCategories = allCategories.Where(c => c.ParentCategoryID == null).ToList();
                ViewBag.Categories = new SelectList(parentCategories, "CategoryID", "Name");
                
                // Check if the product's category is a parent or subcategory
                if (product.CategoryID > 0)
                {
                    var isParentCategory = parentCategories.Any(c => c.CategoryID == product.CategoryID);
                    
                    if (isParentCategory)
                    {
                        // If it's a parent category, populate subcategories
                        var subcategories = allCategories.Where(c => c.ParentCategoryID == product.CategoryID).ToList();
                        ViewBag.Subcategories = new SelectList(subcategories, "CategoryID", "Name");
                    }
                    else
                    {
                        // If it's a subcategory, find its parent and select it in the category dropdown
                        var parentCategory = allCategories.FirstOrDefault(c => c.CategoryID == product.CategoryID);
                        if (parentCategory != null)
                        {
                            // Get subcategories for the parent category
                            var subcategories = allCategories.Where(c => c.ParentCategoryID == parentCategory.CategoryID).ToList();
                            ViewBag.Subcategories = new SelectList(subcategories, "CategoryID", "Name");
                            
                            // Set the parent category as selected
                            ViewBag.SelectedCategoryID = parentCategory.CategoryID;
                        }
                    }
                }
                else
                {
                    ViewBag.Subcategories = new SelectList(Enumerable.Empty<Category>(), "CategoryID", "Name");
                }
                
                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["error"] = "Product not found.";
                    return RedirectToAction(nameof(ProductIndex));
                }

                // Soft delete by setting IsActive to false
                product.IsActive = false;
                product.UpdatedAt = DateTime.Now;
                await _productService.UpdateProductAsync(product);

                TempData["success"] = "Product deleted successfully.";
                return RedirectToAction(nameof(ProductIndex));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting product: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["error"] = "An error occurred while deleting the product.";
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleProductStatus(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["error"] = "Product not found.";
                    return RedirectToAction(nameof(ProductIndex));
                }

                // Toggle the status
                product.IsActive = !product.IsActive;
                product.UpdatedAt = DateTime.Now;
                await _productService.UpdateProductAsync(product);

                TempData["success"] = product.IsActive ? "Product activated successfully." : "Product deactivated successfully.";
                return RedirectToAction(nameof(ProductIndex));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling product status: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["error"] = "An error occurred while toggling the product status.";
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSubcategories(int categoryId)
        {
            try
            {
                // Get all categories
                var allCategories = await _categoryService.GetAllCategoriesAsync();
                
                // Get subcategories for the selected category
                var subcategories = allCategories.Where(c => c.ParentCategoryID == categoryId).ToList();
                
                // Log the count of subcategories found
                System.Diagnostics.Debug.WriteLine($"Found {subcategories.Count} subcategories for category ID {categoryId}");
                
                // Return the subcategories as a JSON result
                return Json(new SelectList(subcategories, "CategoryID", "Name"));
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error in GetSubcategories: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new List<SelectListItem>());
            }
        }
    }
}
