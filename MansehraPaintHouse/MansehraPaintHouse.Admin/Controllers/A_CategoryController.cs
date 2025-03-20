using Microsoft.AspNetCore.Mvc;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class A_CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public A_CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> A_CategoryIndex(int? pageNumber, int? pageSize)
        {
            int defaultPageSize = 8;  // Default items per page
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? defaultPageSize;

            var query = _context.Categories
                .OrderByDescending(c => c.CategoryID)
                .AsNoTracking();

            var categories = await PaginatedList<Category>.CreateAsync(query, currentPageNumber, currentPageSize);
            return View(categories);
        }

        public IActionResult A_CategoryUpsert(int? id)
        {
            var category = id == null || id == 0
                ? new Category()
                : _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.ParentCategories = _context.Categories.ToList();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult A_CategoryUpsert(Category category, IFormFile? Image1File, IFormFile? Image2File)
        {
            if (ModelState.IsValid)
            {
                if (Image1File != null)
                {
                    category.Image1 = SaveImage(Image1File);
                }

                if (Image2File != null)
                {
                    category.Image2 = SaveImage(Image2File);
                }

                if (category.CategoryID == 0)
                {
                    _context.Categories.Add(category);
                }
                else
                {
                    var existingCategory = _context.Categories.Find(category.CategoryID);
                    if (existingCategory != null)
                    {
                        existingCategory.Name = category.Name;
                        existingCategory.Description = category.Description;
                        existingCategory.ParentCategoryID = category.ParentCategoryID;
                        existingCategory.IsActive = category.IsActive;
                        existingCategory.Image1 = category.Image1 ?? existingCategory.Image1;
                        existingCategory.Image2 = category.Image2 ?? existingCategory.Image2;
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("A_CategoryIndex");
            }

            ViewBag.ParentCategories = _context.Categories.ToList();
            return View(category);
        }

        private string SaveImage(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, imageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }
            return $"/images/{imageFile.FileName}";
        }

        //Method to activate category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                category.IsActive = !category.IsActive;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }


















        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ToggleStatus(int id)
        //{
        //    try
        //    {
        //        var category = await _context.Categories.FindAsync(id);

        //        if (category == null)
        //        {
        //            return NotFound();
        //        }

        //        // Toggle the IsActive status
        //        category.IsActive = !category.IsActive;

        //        // Update the ModifiedDate if you have one
        //        // category.ModifiedDate = DateTime.UtcNow;

        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if you have logging configured
        //        return StatusCode(500, "An error occurred while updating the category status.");
        //    }
        //}
    }
}