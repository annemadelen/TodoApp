

namespace TodoApp.Api.Models
{
    // Class that defines a "Todo" item in the database
    // Each becomes a column in the table
    public class Todo
    {
        // A unique primary key for each todo
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // The title for each todo
        public string Title { get; set; } = string.Empty;

        // Whether the Todo is completed or not
        public bool Completed { get; set; } = false;

        // When the todo was first created (in UTC time)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // When the todo was last updated
        public DateTime? UpdatedAt { get; set; } = null;
    }
}