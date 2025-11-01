using System.Collections.Generic;

namespace Stef_David_Lab2.Models
{
    public class CategoryData
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<BookCategory> BookCategories { get; set; }
    }
}
