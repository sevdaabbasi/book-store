using AutoMapper;
using WebApi.DbOperations;

namespace WebApi.AuthorOperations.GetAuthors;

public class GetAuthorsQuery
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetAuthorsQuery(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<AuthorDto> Handle()
    {
        var authors = _context.Authors.OrderBy(x => x.Id).ToList();
        return _mapper.Map<List<AuthorDto>>(authors);
    }
} 