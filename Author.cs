using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi;

public class Author
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    
    // Navigation property for books
    public ICollection<Book> Books { get; set; }
} 