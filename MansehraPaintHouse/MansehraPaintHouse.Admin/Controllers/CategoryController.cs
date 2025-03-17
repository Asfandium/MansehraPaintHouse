using Microsoft.AspNetCore.Mvc;
using MansehraPaintHouse.Core.Entities;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using MansehraPaintHouse.Infrastructure.Data;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList(); // Fetch categories from the database
            return View("CategoryIndex", categories);
        }

        public IActionResult Create()
        {
            ViewBag.ParentCategories = _context.Categories.ToList(); // Pass parent categories for dropdown
            return View("CategoryCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category, IFormFile? Image1File, IFormFile? Image2File)
        {
            if (ModelState.IsValid)
            {
                // Save images to the specified folder and store their paths
                if (Image1File != null)
                {
                    var imagePath = Path.Combine("wwwroot/images", Image1File.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        Image1File.CopyTo(stream);
                    }
                    category.Image1 = $"/images/{Image1File.FileName}";
                }

                if (Image2File != null)
                {
                    var imagePath = Path.Combine("wwwroot/images", Image2File.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        Image2File.CopyTo(stream);
                    }
                    category.Image2 = $"/images/{Image2File.FileName}";
                }

                // Save the category to the database
                _context.Categories.Add(category);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ParentCategories = _context.Categories.ToList(); // Pass parent categories for dropdown
            return View("CategoryCreate", category); 
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.ParentCategories = _context.Categories.ToList();
            return View("CategoryEdit", category); 
        }

        [HttpPost]
        public IActionResult Edit(Category category, IFormFile? Image1File, IFormFile? Image2File)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = _context.Categories.Find(category.CategoryID);
                if (existingCategory != null)
                {
                    existingCategory.Name = category.Name;
                    existingCategory.Description = category.Description;
                    existingCategory.ParentCategoryID = category.ParentCategoryID;
                    existingCategory.IsActive = category.IsActive;

                    // Update images if new ones are uploaded
                    if (Image1File != null)
                    {
                        existingCategory.Image1 = SaveImage(Image1File);
                    }

                    if (Image2File != null)
                    {
                        existingCategory.Image2 = SaveImage(Image2File);
                    }

                    _context.SaveChanges(); // Save changes to the database
                }
                return RedirectToAction("Index");
            }

            ViewBag.ParentCategories = _context.Categories.ToList();
            return View("CategoryEdit", category); 
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View("CategoryDelete", category); // Render the confirmation view
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                category.IsActive = false; // Perform a soft delete by setting IsActive to false
                _context.SaveChanges(); // Save changes to the database
            }

            return RedirectToAction("Index");
        }

        private string SaveImage(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists
            }

            var filePath = Path.Combine(uploadsFolder, imageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }
            return $"/images/{imageFile.FileName}";
        }
    }
}