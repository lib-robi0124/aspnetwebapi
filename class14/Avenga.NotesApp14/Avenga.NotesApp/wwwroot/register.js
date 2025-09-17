let regBtn = document.getElementById("regBtn");
let fn = document.getElementById("firstName");
let ln = document.getElementById("lastName");
let username1 = document.getElementById("username");
let pass = document.getElementById("pass");
let confPass = document.getElementById("confPass");
let port = "5280";

let register = async () => {
    let url = "http://localhost:" + port + "/api/users/register";
    //first get value from inputs
    let user = {
        FirstName: fn.value,
        LastName: ln.value,
        username: username1.value,
        Password: pass.value,
        ConfirmPassword: confPass.value
    }
    console.log(user);
    //afterwards we create the post with FETCH for the appropriate URL
    //then we store the token that the endpoint created in the browsers local storage
    let response = await fetch(url, {
        //we set bellow what kind of http method this function will trigger (GET/POST/PUT/DELETE)
        method: 'POST',
        //we set the headers and put only what type of content will be
        headers: {
            'Content-Type': 'application/json'
        },
        //here we set the value in the body that will be send
        //and for that purpose we stringify the model
        //or we convert the values into JSON string
        body: JSON.stringify(user)
        console.log(user);

    })
    //here we make the reponse that we get from the fetch
        .then(function (response) {
            console.log(response);
            if (response.ok) {
                alert("Registration successful! Please log in.");
                window.location.href = "http://localhost:5280/login.html"
            } else {
                response.text().then(function (text) {
                    alert("Registration failed: " + text);
                });
            }
        })
    //if there is an error we catch it here
        .catch(function (error) {
            console.log(error)
        });
}
//we add an event listener to the login button
regBtn.addEventListener("click", register);