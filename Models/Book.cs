using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stef_David_Lab2.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Titlul cărții este obligatoriu.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Titlul trebuie să aibă între 3 și 150 de caractere.")]
        [Display(Name = "Book Title")]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "decimal(6, 2)")]
        [Range(0.01, 500)]
        public decimal Price { get; set; }
        public int? AuthorID { get; set; }
        public Author? Author { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime PublishingDate { get; set; }

        public int? PublisherID { get; set; }
        public Publisher? Publisher { get; set; }
        public ICollection<BookCategory>? BookCategories { get; set; }
        public ICollection<Borrowing>? Borrowings { get; set; }
    }
}
