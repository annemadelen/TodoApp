
const API_URL = "http://localhost:5112/api/todo";

export async function getTodos() {
    const res = await fetch(API_URL);
    return res.json();
}

export async function addTodo(todo) {
    await fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        bode: JSON.stringify(todo),
    });
}

export async function deleteTodo(id) {
    await fetch(`${API_URL}/${id}`, { method: "DELETE" });
}

export async function toggleTodo(todo) {
    await fetch(`${API_URL}/${todo.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ ...todo, isCompleted: !todo.isCompleted }),
    });
}