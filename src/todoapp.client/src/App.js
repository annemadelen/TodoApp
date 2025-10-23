import React, { useEffect, useState } from "react";
import './App.css';

// Base URL for backend API
const API_URL = "http://localhost:5112/api/todo";

function App() {
  const [todos, setTodos] = useState([]);         // List of todo items
  const [newTodo, setNewTodo] = useState("");     // Text input for a new task
  const [search, setSearch] = useState("");       // Search query
  const [filter, setFilter] = useState("all");    // Filter: "all", "completed", or "incomplete"

  // Fetch todos from backend API
  const fetchTodos = async () => {
    let url = `${API_URL}?`;
    const params = [];

    // Add query params for seach and filters
    if (search.trim()) {
      params.push(`query=${encodeURIComponent(search)}`);
    }
    if (filter === "completed") {
      params.push("completed=true");
    } else if (filter === "incomplete") {
      params.push("completed=false");
    }

    url += params.join("&")

    try {
      const res = await fetch(url);     // Send GET request
      const data = await res.json();    // Convert response to JSON
      setTodos(data);                   // Store todos in React state
    } catch (err) {
      console.error("Error fetching todos:", err)
    }
  }

  // Fecth todos when "search" or "filter" changes
  useEffect(() => {
    fetchTodos();
  }, [search, filter]);

  // Add a new todo
  const addTodo = async () => {
    const title = newTodo.trim();
    if (!title) {
      alert("Title cannot be empty.");
      return;
    }

    const response = await fetch(API_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ title }),
    });

    // if successful, add new todo to the state
    if (response.ok) {
      const data = await response.json();
      setTodos([...todos, data]);
      setNewTodo("");
    } else {
      alert("Failed to add todo.");
    }
  };

  // Delete a todo by id
  const deleteTodo = async (id) => {
    await fetch(`${API_URL}/${id}`, { method: "DELETE" });
    setTodos(todos.filter(t => t.id !== id)); // Remove from state
  };

  // Toggle completion
  const toggleCompleted = async (todo) => {
    const updatedTodo = { ...todo, completed: !todo.completed};

    const response = await fetch(`${API_URL}/${todo.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(updatedTodo),
    });

    if (response.ok) {
      fetchTodos(); // Refresh list
    } else {
      alert("Error updating todo.");
    }
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
        <button onClick={addTodo} style={{ marginLeft: "0.5rem" }}>
          Add
        </button>
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
      <ul style={{ listStyle: "none", padding: 0 }}>
        {todos.map((todo) => (
          <li
            key={todo.id}
            style={{
              marginBottom: "0.75rem",
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
            }}
          >
            <div>
              <input
                type="checkbox"
                checked={todo.completed}
                onChange={() => toggleCompleted(todo)}
              />
              <span
                style={{
                  textDecoration: todo.completed ? "line-through" : "none",
                  marginLeft: "0.5rem",
                }}
              >
                {todo.title}
              </span>
              {/* Show created/updated times */}
              <div style={{ fontSize: "0.8rem", color: "gray" }}>
                Created: {new Date(todo.createdAt).toLocaleString()}
                {todo.updatedAt && (
                  <> | Updated: {new Date(todo.updatedAt).toLocaleString()}</>
                )}
              </div>
            </div>
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