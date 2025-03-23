using Microsoft.AspNetCore.Mvc;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> CategoryIndex(int? pageNumber, int? pageSize, string searchTerm)
        {
            int defaultPageSize = 8;  // Default items per page
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? defaultPageSize;

            IQueryable<Category> query;

            // Perform search on the database
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = await _categoryService.SearchCategoriesAsync(searchTerm);
            }
            else
            {
                query = await _categoryService.GetAllCategoriesQueryableAsync();
            }

            // Apply ordering and pagination
            query = query.OrderByDescending(c => c.CategoryID);
            var paginatedCategories = await PaginatedList<Category>.CreateAsync(query, currentPageNumber, currentPageSize);

            ViewBag.SearchTerm = searchTerm;
            return View(paginatedCategories);
        }

        public async Task<IActionResult> CategoryUpsert(int? id)
        {
            var category = id == null || id == 0
                ? new Category()
                : await _categoryService.GetCategoryByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.ParentCategories = await _categoryService.GetAllCategoriesAsync();
            return PartialView("_CategoryUpsert", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryUpsert(Category category, IFormFile? Image1File, IFormFile? Image2File)
        {
            if (ModelState.IsValid)
            {
                if (Image1File != null)
                {
                    category.Image1 = await SaveImageAsync(Image1File);
                }

                if (Image2File != null)
                {
                    category.Image2 = await SaveImageAsync(Image2File);
                }

                if (category.CategoryID == 0)
                {
                    await _categoryService.CreateCategoryAsync(category);
                }
                else
                {
                    var existingCategory = await _categoryService.GetCategoryByIdAsync(category.CategoryID);
                    if (existingCategory != null)
                    {
                        existingCategory.Name = category.Name;
                        existingCategory.Description = category.Description;
                        existingCategory.ParentCategoryID = category.ParentCategoryID;
                        existingCategory.IsActive = category.IsActive;
                        existingCategory.Image1 = category.Image1 ?? existingCategory.Image1;
                        existingCategory.Image2 = category.Image2 ?? existingCategory.Image2;
                        await _categoryService.UpdateCategoryAsync(existingCategory);
                    }
                }

                return RedirectToAction("CategoryIndex");
            }

            ViewBag.ParentCategories = await _categoryService.GetAllCategoriesAsync();
            return View(category);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                await _categoryService.ToggleCategoryStatusAsync(id);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}