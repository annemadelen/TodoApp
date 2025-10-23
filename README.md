# TodoApp

A simple full-stack **Todo List** application built with **ASP.NET Core Web API** (backend) and **React** (frontend).

It allows users to:
- Add, update, delete and filter todo items
- Mark tasks as completed
- Search for tasks by title
- View todo-list and timestamps

Includes unit tests written with **xUnit** for backend controller logic

---

## Tech Stack

**Backend:**
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- InMemoryDatabase (for testing)

**Frontend:**
- React 
- Fetch API for HTTP requests
- Basic CSS styling

**Testing:**
- xUnit (.NET)

---

## Setup Instructions

### Prerequisites

Installed before starting:

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js (v18+)](https://nodejs.org/) and npm

---

### Backend (ASP.NET Core API)
```bash
cd TodoApp.Api
dotnet restore
dotnet run
```
API will start at: http://localhost:5112

---

### Frontend (React)
```bash
cd todoapp.client
npm install
npm start

```
visit: http://localhost:3000

---

### Test
```bash
cd TodoApp.Tests
dotnet test
```

---
