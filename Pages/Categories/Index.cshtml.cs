using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stef_David_Lab2.Data;
using Stef_David_Lab2.Models;

namespace Stef_David_Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Stef_David_Lab2Context _context;

        public IndexModel(Stef_David_Lab2Context context)
        {
            _context = context;
        }

        // ✅ Lista de categorii (pentru afișare principală)
        public IList<Category> Categories { get; set; } = new List<Category>();

        // ✅ Obiectul de tip ViewModel (CategoryData)
        public CategoryData CategoryData { get; set; } = new CategoryData();

        // ✅ ID-ul categoriei selectate
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id)
        {
            // încarcă toate categoriile + cărțile aferente
            CategoryData.Categories = await _context.Category
                .Include(c => c.BookCategories)
                    .ThenInclude(bc => bc.Book)
                        .ThenInclude(b => b.Author)
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            if (id != null)
            {
                CategoryID = id.Value;
                var selectedCategory = CategoryData.Categories
                    .FirstOrDefault(c => c.ID == id.Value);

                if (selectedCategory != null)
                {
                    // selectăm cărțile din categoria aleasă
                    CategoryData.Books = selectedCategory.BookCategories
                        .Select(bc => bc.Book);
                }
            }
        }
    }
}
