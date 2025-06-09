const API_BASE = "https://localhost:7185/api/Todo"; //URL of our API

function login(){
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    fetch('${API_BASE}/auth/login', {
        method: "POST",
        headers: {"Content-Type" : "application/jason"},
        body: JSON.stringify({username, password}),
    })
        .then(res => res.json())
        .then(data => {
            if(data.token){
                localStorage.setItem("token", data.token);
                window.location.href = "index.html";
            } else {
                document.getElementById("error").innerText = "invalid login";
            }
        });
}

function logout(){
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

function getTodos(){
    fetch('${API_BASE}/todos',{
        headers: {
            Authorization: 'Bearer ${localStorage.getItem("token")}',
        }
    })
}