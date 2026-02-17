using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Microsoft.AspNetCore.Mvc.ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public class TodoController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly AppDbContext _db;
    public TodoController(AppDbContext db) => _db = db;

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public async Task<Microsoft.AspNetCore.Mvc.IActionResult> Get() => Ok(await _db.Todos.ToListAsync());

    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<Microsoft.AspNetCore.Mvc.IActionResult> Post(Todo item)
    {
        _db.Todos.Add(item);
        await _db.SaveChangesAsync();
        return Ok(item);
    }
}
