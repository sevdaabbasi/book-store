using WebApi.DbOperations;

namespace WebApi.AuthorOperations.DeleteAuthor;

public class DeleteAuthorCommand
{
    private readonly BookStoreDbContext _context;
    public int AuthorId { get; set; }

    public DeleteAuthorCommand(BookStoreDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
        if (author is null)
            throw new InvalidOperationException("Author not found!");

        // Check if author has any books
        var hasBooks = _context.Books.Any(x => x.AuthorId == AuthorId);
        if (hasBooks)
            throw new InvalidOperationException("Cannot delete author with existing books. Please delete the books first.");

        _context.Authors.Remove(author);
        _context.SaveChanges();
    }
} 