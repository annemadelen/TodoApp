using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Models;

namespace TodoApp.Api.Data
{
    // Class representing the database
    // Inherits from DbContext
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        // Represents the "todos" table in the database
        // Each todo object corresponds to one row in the table
        public DbSet<Todo> Todos => Set<Todo>();
    }
}
