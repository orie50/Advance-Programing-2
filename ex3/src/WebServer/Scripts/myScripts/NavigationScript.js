/*

*/
$(document).ready(function () {
    async: true;
    if (sessionStorage.Connect == 1) {
        // load the logout and username text

        $("#Login").text("Log off");
        $("#Register").text("Hello " + sessionStorage.username + "!");
    }
    else {
        // load the login and register text
        $("#Login").text("Login");
        $("#Register").text("Register");
    }
});

/*
if login button pressed
*/
$("#Login").click(function () {
    var value = $("#Login").text();
    if (value == "Log off") {
        sessionStorage.Connect = 0;
        alert("You Log off");
        window.location.replace("MainPage.html");
        return;
    }
    window.location.replace("LoginPage.html");
});

/*
if register button pressed
*/
$("#Register").click(function () {
    var value = $("#Login").text();
    if (value == "Log off") {
        return;
    }
    window.location.replace("RegisterPage.html");
});

/*
if multiplayer button pressed
*/
$("#MultiPlayer_Game").click(function () {
    var value = $("#Login").text();
    if (value == "Login") {
        alert("For multiplayer game you need to login");
        window.location.replace("LoginPage.html");
        return;
    }
    window.location.replace("MultiPlayerPage.html");
});