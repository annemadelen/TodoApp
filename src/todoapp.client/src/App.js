import React, { useEffect, useState } from "react";

const API_URL = "http://localhost:5112/api/todo";

function App() {
  const [todos, setTodos] = useState([]);
  const [newTodo, setNewTodo] = useState("");

  // Fetch all todos when the app loads
  useEffect(() => {
    fetch(API_URL)
      .then(res => res.json())
      .then(data => setTodos(data))
      .catch(err => console.error("Error fetching todos:", err));
  }, []);

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
    <div style={{ padding: "2rem" }}>
      <h1>Todo List</h1>
      <input
        type="text"
        value={newTodo}
        onChange={(e) => setNewTodo(e.target.value)}
        placeholder="Add a task..."
      />
      <button onClick={addTodo}>Add</button>

      <ul>
        {todos.map(todo => (
          <li key={todo.id}>
            {todo.title}
            <button onClick={() => deleteTodo(todo.id)}>Remove</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
