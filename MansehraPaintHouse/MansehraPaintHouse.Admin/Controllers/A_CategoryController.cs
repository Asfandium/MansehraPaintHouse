using Microsoft.AspNetCore.Mvc;
using MansehraPaintHouse.Core.Entities;
using MansehraPaintHouse.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MansehraPaintHouse.Admin.Controllers
{
    public class A_CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public A_CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult A_CategoryIndex()
        {
            var categories = _context.Categories.ToList();
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                category.IsActive = false; // Perform a soft delete by setting IsActive to false
                _context.SaveChanges(); // Save changes to the database
            }
            return RedirectToAction("A_CategoryIndex");
        }
    }
}