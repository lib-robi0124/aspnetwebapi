let usernameInput = document.getElementById("username");
let passInput = document.getElementById("pass");
let loginBtn = document.getElementById("loginBtn");
let port = "5280";

let login = async () => {

    let url = "http://localhost:" + port + "/api/users/login";

//first get value from inputs
    let user = {
        Username: usernameInput.value,
        Password: passInput.value
    }
    console.log(user);

    //afterwards we create the post with FETCH for the appropriate URL
    //then we store the token that the endpoint created in the browsers local storage
    let response = await fetch(url, {
        //we set bellow what kind of http method this function will trigger (GET/POST/PUT/DELETE)
        method: "POST",
        //we set the headers and put only what type of content will be
        headers: {
            'Content-Type': 'application/json'
            
        },
        //here we set the value in the body that will be send
        //and for that purpose we stringify the model
        //or we convert the values into JSON string
        body: JSON.stringify(user)
    })
//here we make the reponse that we get from the fetch
        .then(function (response) {
            console.log(response);
            response.text()
                .then(function (text) {
            //here we save the token in the local storage in the browser
                console.log(text);
                localStorage.setItem("notesApiToken", text);
                //after we get the token we redirect to the notes.html page
                window.location.href = "http://localhost:5280/note.html";
            })
        })
//if there is an error we catch it here
        .catch(function (err) {
            console.log(err);
        });
}
//we add an event listener to the login button
loginBtn.addEventListener("click", login);
