using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos(
            [FromQuery] string? query,
            [FromQuery] bool? completed)
        {
            var todos = _context.Todos.AsQueryable();

            // Filter by search term
            if (!string.IsNullOrWhiteSpace(query))
            {
                todos = todos.Where(t => t.Title != null && t.Title.ToLower().Contains(query.ToLower()));
            }

            // Filter by completion status
            if (completed.HasValue)
            {
                todos = todos.Where(t => t.Completed == completed.Value);
            }

            return await todos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoById(string id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return NotFound();
            return todo;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
        {
            // Validation: Title cannot be empty
            if (string.IsNullOrWhiteSpace(todo.Title))
                return BadRequest("Title cannot be empty.");

            // Generate UUID and timestamps
            todo.Id = Guid.NewGuid().ToString();
            todo.CreatedAt = DateTime.UtcNow;
            todo.UpdatedAt = null; 

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(string id, Todo updatedTodo)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return NotFound();

            todo.Title = updatedTodo.Title ?? todo.Title;
            todo.Completed = updatedTodo.Completed;
            todo.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(string id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}