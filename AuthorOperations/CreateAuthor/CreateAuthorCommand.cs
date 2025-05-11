using AutoMapper;
using WebApi.DbOperations;

namespace WebApi.AuthorOperations.CreateAuthor;

public class CreateAuthorCommand
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    public CreateAuthorDto Model { get; set; }

    public CreateAuthorCommand(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Handle()
    {
        var author = _mapper.Map<Author>(Model);
        _context.Authors.Add(author);
        _context.SaveChanges();
    }
} 