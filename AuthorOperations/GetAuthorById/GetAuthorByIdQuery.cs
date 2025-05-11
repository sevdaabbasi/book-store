using AutoMapper;
using WebApi.DbOperations;

namespace WebApi.AuthorOperations.GetAuthorById;

public class GetAuthorByIdQuery
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    public int AuthorId { get; set; }

    public GetAuthorByIdQuery(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public AuthorDto Handle()
    {
        var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
        if (author is null)
            throw new InvalidOperationException("Author not found!");

        return _mapper.Map<AuthorDto>(author);
    }
} 