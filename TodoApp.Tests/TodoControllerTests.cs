using Xunit;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Controllers;
using TodoApp.Api.Data;
using TodoApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TodoApp.Tests
{
    // Unit tests for the TodoController
    public class TodoControllerTests
    {
        // Helper method that creates a TodoController that uses a temporary DB
        private TodoController GetControllerWithInMemoryDb()
        {
            // Use a unique database name each time to prevent leftover data
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new TodoContext(options);
            return new TodoController(context);
        }

        // Test to see if a todo is added correctly
        [Fact]
        public async Task CreateTodo_ShouldAddNewTodo()
        {
            // Arrange - create a controller and a sample todo
            var controller = GetControllerWithInMemoryDb();
            var todo = new Todo { Title = "Test Task" };

            // Act - call the CreateTodo method
            var result = await controller.CreateTodo(todo);

            // Assert - check that the result was created successfully
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdTodo = Assert.IsType<Todo>(createdResult.Value);

            Assert.Equal("Test Task", createdTodo.Title);
            Assert.False(createdTodo.Completed);
        }

        // Test to see if getting all todos return the correct number
        [Fact]
        public async Task GetTodos_ShouldReturnAllTodos()
        {
            // Arrange - create a controller and add two todos
            var controller = GetControllerWithInMemoryDb();
            await controller.CreateTodo(new Todo { Title = "Task 1" });
            await controller.CreateTodo(new Todo { Title = "Task 2" });

            // Act - get all todos
            var result = await controller.GetTodos(null, null);

            // Assert - confirm there are 2 todos
            var todos = Assert.IsType<List<Todo>>(result.Value);
            Assert.Equal(2, todos.Count);
        }
    }
}
