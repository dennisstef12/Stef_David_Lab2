using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stef_David_Lab2.Data;
using Stef_David_Lab2.Models;

namespace Stef_David_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Stef_David_Lab2Context _context;

        public IndexModel(Stef_David_Lab2Context context)
        {
            _context = context;
        }

        // ✅ Proprietatea cerută de laborator
        public BookData BookData { get; set; } = new BookData();

        // Aceste două variabile sunt folosite dacă vrei să selectezi o carte / categorie
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id, int? categoryID)
        {
            BookData = new BookData();

            BookData.Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            if (id != null)
            {
                BookID = id.Value;
                var selectedBook = BookData.Books.FirstOrDefault(b => b.ID == id.Value);

                if (selectedBook != null)
                {
                    BookData.Categories = selectedBook.BookCategories
                        .Select(bc => bc.Category);
                }
            }
        }
    }
}
