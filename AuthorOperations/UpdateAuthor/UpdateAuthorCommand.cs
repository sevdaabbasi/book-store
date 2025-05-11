using AutoMapper;
using WebApi.DbOperations;

namespace WebApi.AuthorOperations.UpdateAuthor;

public class UpdateAuthorCommand
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    public int AuthorId { get; set; }
    public UpdateAuthorDto Model { get; set; }

    public UpdateAuthorCommand(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Handle()
    {
        var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
        if (author is null)
            throw new InvalidOperationException("Author not found!");

        _mapper.Map(Model, author);
        _context.SaveChanges();
    }
} 