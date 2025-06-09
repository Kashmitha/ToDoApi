// ✅ Set to your actual API base URL (match with ASP.NET launch URL)
const API_BASE = "http://localhost:5076"; // Change if needed

// 🔐 LOGIN FUNCTION
function login() {
  const username = document.getElementById("username").value.trim();
  const password = document.getElementById("password").value.trim();

  fetch(`${API_BASE}/api/auth/login`, { // ✅ Ensure this route matches your API
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password }),
  })
    .then(res => res.json())
    .then(data => {
      if (data.token) {
        localStorage.setItem("token", data.token);
        window.location.href = "index.html";
      } else {
        document.getElementById("error").innerText = data.message || "Invalid login";
      }
    })
    .catch(() => {
      document.getElementById("error").innerText = "Server error. Please try again.";
    });
}

// 🔓 LOGOUT FUNCTION
function logout() {
  localStorage.removeItem("token");
  window.location.href = "login.html";
}

// ✅ FETCH TODOS
function getTodos() {
  fetch(`${API_BASE}/api/todos`, { // ✅ Corrected route
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  })
    .then(res => {
      if (!res.ok) throw new Error("Unauthorized");
      return res.json();
    })
    .then(data => {
      const list = document.getElementById("todo-list");
      list.innerHTML = "";
      data.forEach(todo => {
        const li = document.createElement("li");
        li.innerHTML = `
          <span>${todo.title}</span>
          <button onclick="deleteTodo(${todo.id})">❌</button>
        `;
        list.appendChild(li);
      });
    })
    .catch(err => {
      console.error(err);
      logout(); // logout and redirect on token failure
    });
}

// ➕ ADD NEW TODO
function addTodo() {
  const title = document.getElementById("new-todo").value.trim();
  if (!title) return;

  fetch(`${API_BASE}/api/todos`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
    body: JSON.stringify({ title }),
  })
    .then(res => {
      if (!res.ok) throw new Error("Failed to add todo");
      document.getElementById("new-todo").value = "";
      getTodos();
    })
    .catch(console.error);
}

// ❌ DELETE TODO
function deleteTodo(id) {
  fetch(`${API_BASE}/api/todos/${id}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  })
    .then(res => {
      if (!res.ok) throw new Error("Failed to delete todo");
      getTodos();
    })
    .catch(console.error);
}

// 🔁 INIT on index.html only
if (window.location.pathname.includes("index.html")) {
  if (!localStorage.getItem("token")) {
    window.location.href = "login.html";
  } else {
    getTodos();
  }
}
