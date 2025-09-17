let getAllNotesBtn = document.getElementById("btn1");
let addNoteBtn = document.getElementById("btn3");
let logoutBtn = document.getElementById("logoutBtn");
let addNoteTextInput = document.getElementById("noteText");
let addNotePriorityInput = document.getElementById("notePriority");
let addNoteTagInput = document.getElementById("noteTag");
let addNoteUserInput = document.getElementById("noteUserId");

let url = "http://localhost:5280/api/notes";

let getAllNotes = async () => {
//getting the token from the local storage
    let token = localStorage.getItem("notesApiToken");
    console.log("Token: " + token);
    let response = await fetch(url, {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    if (response.ok) {
        let notes = await response.json();
        console.log(notes);
        displayNotes(notes);
    } else {
        console.log("Error: " + response.status);
        if (response.status === 401) {
            alert("Unauthorized! Please log in again.");
            window.location.href = "http://localhost:5280/login.html";
        }
    }

    let addNote = async () => {
        //getting the token from the local storage
        let token = localStorage.getItem("notesApiToken");

        let note = {
            Text: addNoteTextInput.value,
            Priority: parseInt(addNotePriorityInput.value),
            Tag: parseInt(addNoteTagInput.value),
            UserId: parseInt(addNoteUserInput.value)
        }
        let response = await fetch(url + "/addNote", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(note)
        })
            .then(function (response) {
                console.log(response);
            }).catch(function (error) {
                console.log(error);
            })
    }
    let displayNotes = (notes) => {
        //creating a table with elements
        let table = document.createElement("table");
        let thead = document.createElement("thead");
        let tbody = document.createElement("tbody");
        //creating the header row
        let headerRow = document.createElement("tr");
        let headers = ["Id", "Text", "Priority", "Tag", "UserId"];
        headers.forEach(headerText => {
            let header = document.createElement("th");
            header.textContent = headerText;
            headerRow.appendChild(th);
        });
        thead.appendChild(headerRow);

        //creating the body rows
        notes.forEach(note => {
            let row = document.createElement("tr");
            Object.values(note).forEach(value => {
                let td = document.createElement("td");
                td.textContent = value;
                row.appendChild(td);
            });
            tbody.appendChild(row);
        })
        table.appendChild(thead);
        table.appendChild(tbody);

        let tableContainer = document.getElementById("tableContainer");
        tableContainer.innerHTML = "";
        tableContainer.appendChild(table);
    }
    let logout = () => {
        localStorage.removeItem("notesApiToken"); //removing the token from the local storage
        window.location.href = "http://localhost:5280/login.html";
    }
    getAllNotesBtn.addEventListener("click", getAllNotes);
    addNoteBtn.addEventListener("click", addNote);
    logoutBtn.addEventListener("click", logout);