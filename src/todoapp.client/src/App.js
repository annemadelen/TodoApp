import React, { useEffect, useState } from "react";

const API_URL = "http://localhost:5112/api/todo";

function App() {
  const [todos, setTodos] = useState([]);
  const [newTodo, setNewTodo] = useState("");
  const [search, setSearch] = useState("");
  const [filter, setFilter] = useState("all");

  // Fetch todos
  const fetchTodos = async () => {
    let url = `${API_URL}?`;
    const params = [];

    if (search.trim()) {
      params.push(`query=${encodeURIComponent(search)}`);
    }
    if (filter === "completed") {
      params.push("completed=true");
    } else if (filter === "incompleted") {
      params.push("completed=false");
    }

    url += params.join("&")

    try {
      const res = await fetch(url);
      const data = await res.json();
      setTodos(data);
    } catch (err) {
      console.error("Error fetching todos:", err)
    }
  }

  useEffect(() => {
    fetchTodos();
  }, [search, filter]);

  // Add a new todo
  const addTodo = async () => {
    if (!newTodo.trim()) return;

    const response = await fetch(API_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ title: newTodo, isComplete: false }),
    });

    const data = await response.json();
    setTodos([...todos, data]);
    setNewTodo("");
  };

  // Delete a todo
  const deleteTodo = async (id) => {
    await fetch(`${API_URL}/${id}`, { method: "DELETE" });
    setTodos(todos.filter(t => t.id !== id));
  };

  return (
    <div style={{ padding: "2rem", maxWidth: "600px", margin: "0 auto" }}>
      <h1>Todo List</h1>

      {/* Add New Todo */}
      <div style={{ marginBottom: "1rem" }}>
        <input
          type="text"
          value={newTodo}
          onChange={(e) => setNewTodo(e.target.value)}
          placeholder="Add a task..."
        />
        <button onClick={addTodo}>Add</button>
      </div>

      {/* Search + Filter */}
      <div style={{ marginBottom: "1rem" }}>
        <input
          type="text"
          placeholder="Search tasks..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <select
          value={filter}
          onChange={(e) => setFilter(e.target.value)}
          style={{ marginLeft: "0.5rem" }}
        >
          <option value="all">All</option>
          <option value="completed">Completed</option>
          <option value="incomplete">Incomplete</option>
        </select>
      </div>

      {/* Todo List */}
      <ul>
        {todos.map(todo => (
          <li key={todo.id}>
            {todo.title} {todo.completed ? "✅" : ""}
            <button
              onClick={() => deleteTodo(todo.id)}
              style={{ marginLeft: "1rem" }}
            >
              Remove
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
