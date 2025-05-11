using Microsoft.AspNetCore.Mvc;
using WebApi.AuthorOperations.CreateAuthor;
using WebApi.AuthorOperations.DeleteAuthor;
using WebApi.AuthorOperations.GetAuthorById;
using WebApi.AuthorOperations.GetAuthors;
using WebApi.AuthorOperations.UpdateAuthor;
using WebApi.DbOperations;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public AuthorController(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAuthors()
    {
        GetAuthorsQuery query = new GetAuthorsQuery(_context, _mapper);
        var result = query.Handle();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetAuthorById(int id)
    {
        GetAuthorByIdQuery query = new GetAuthorByIdQuery(_context, _mapper);
        query.AuthorId = id;
        var result = query.Handle();
        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddAuthor([FromBody] CreateAuthorDto newAuthor)
    {
        CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
        command.Model = newAuthor;
        command.Handle();
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAuthor(int id, [FromBody] UpdateAuthorDto updatedAuthor)
    {
        UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
        command.AuthorId = id;
        command.Model = updatedAuthor;
        command.Handle();
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAuthor(int id)
    {
        DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
        command.AuthorId = id;
        command.Handle();
        return Ok();
    }
} 