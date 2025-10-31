using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stef_David_Lab2.Data;
using Stef_David_Lab2.Models;

namespace Stef_David_Lab2.Pages.Books
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Stef_David_Lab2Context _context;

        public CreateModel(Stef_David_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public IActionResult OnGet()
        {
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");
            ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "LastName");

            // inițial cartea e goală, dar populăm toate categoriile
            var book = new Book();
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            var newBook = new Book();

            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();
                foreach (var category in selectedCategories)
                {
                    var categoryToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(category)
                    };
                    newBook.BookCategories.Add(categoryToAdd);
                }
            }

            if (await TryUpdateModelAsync<Book>(
                newBook,
                "Book",
                b => b.Title, b => b.Price, b => b.PublishingDate, b => b.AuthorID, b => b.PublisherID))
            {
                _context.Book.Add(newBook);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateAssignedCategoryData(_context, newBook);
            return Page();
        }
    }
}

