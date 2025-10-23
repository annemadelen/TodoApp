using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
    // Marks this class as a Web API controller 
    [ApiController]
    // Sets the route for this controller
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        // The EF Core DbContext representing the database
        private readonly TodoContext _context;

        // Constructor receives the TodoContext vie dependency injections
        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GET api/todo
        // Returns a list of todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos(
            [FromQuery] string? query,      // comes from URL like ?query=task 
            [FromQuery] bool? completed)    // comes from URL like ?completed=true
        {
            // Starts building a query
            var todos = _context.Todos.AsQueryable();

            // Filter by search term
            if (!string.IsNullOrWhiteSpace(query))
            {
                // makes search case-insensitive and protect against t.Title being null
                todos = todos.Where(t => t.Title != null && t.Title.ToLower().Contains(query.ToLower()));
            }

            // Filter by completion status
            if (completed.HasValue)
            {
                todos = todos.Where(t => t.Completed == completed.Value);
            }

            // Execute the SQL query and return the results as a list (async)
            return await todos.ToListAsync();
        }

        // GET api/todo/{id}
        // Return a single todo by its id
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoById(string id)
        {
            // FindAsync looks for an entity by primary key
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return NotFound(); // 404 if missing
            return todo; // 200 with the Todo object
        }

        // POST api/todo
        // Create a new todo from JSON
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
            // Persist changes to the DB (INSERT SQL command)
            await _context.SaveChangesAsync();

            // Return 201 Created, include Location header pointing to GET api/todo/{id}
            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
        }

        // PUT api/todo/{id}
        // Update an existing todo
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(string id, Todo updatedTodo)
        {
            // Looks up the existing todo by id
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return NotFound(); // 404 if not found

            // Updates todo
            todo.Title = updatedTodo.Title ?? todo.Title;
            todo.Completed = updatedTodo.Completed;
            // set/update the timestamp
            todo.UpdatedAt = DateTime.UtcNow;

            // Save changes (UPDATE SQL command)
            await _context.SaveChangesAsync();
            return NoContent(); // 204 success
        }

        // DELETE api/todo/{id}
        // Remove a todo by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(string id)
        {
            // Find the todo
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
                return NotFound(); // 404 if it is not found

            // Marks the todo for deletion
            _context.Todos.Remove(todo);
            // Save changes (DELETE SQL command)
            await _context.SaveChangesAsync();
            return NoContent(); // 204
        }
    }
}