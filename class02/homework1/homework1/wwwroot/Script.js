const apiBase = "/api/users";

function getAllUsers() {
    fetch(apiBase)
        .then(response => {
            if (!response.ok) throw new Error(`Error: ${response.status}`);
            return response.json();
        })
        .then(data => {
            document.getElementById("result").textContent = JSON.stringify(data, null, 2);
        })
        .catch(err => {
            document.getElementById("result").textContent = err;
        });
}

function getUserByIndex() {
    const index = document.getElementById("userIndex").value;
    fetch(`${apiBase}/${index}`)
        .then(response => {
            if (!response.ok) throw new Error(`Error: ${response.status}`);
            return response.json();
        })
        .then(data => {
            document.getElementById("result").textContent = JSON.stringify(data, null, 2);
        })
        .catch(err => {
            document.getElementById("result").textContent = err;
        });
}
