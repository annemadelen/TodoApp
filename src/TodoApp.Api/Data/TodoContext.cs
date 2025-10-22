using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using TodoApp.Api.Models;

namespace TodoApp.Api.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
