using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stef_David_Lab2.Data;
using Stef_David_Lab2.Models;

namespace Stef_David_Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Stef_David_Lab2.Data.Stef_David_Lab2Context _context;

        public EditModel(Stef_David_Lab2.Data.Stef_David_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Borrowing = await _context.Borrowing
                .Include(b => b.Book)
                    .ThenInclude(b => b.Author)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Borrowing == null)
            {
                return NotFound();
            }

            var books = await _context.Book
                .Include(b => b.Author)
                .Select(x => new
                {
                    x.ID,
                    BookDetails = x.Title + " - " + x.Author.LastName + " " + x.Author.FirstName
                })
                .ToListAsync();

            var members = await _context.Member
                .Select(m => new
                {
                    m.ID,
                    FullName = m.FirstName + " " + m.LastName
                })
                .ToListAsync();

            ViewData["BookID"] = new SelectList(books, "ID", "BookDetails", Borrowing.BookID);
            ViewData["MemberID"] = new SelectList(members, "ID", "FullName", Borrowing.MemberID);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowing.Any(e => e.ID == id);
        }
    }
}
