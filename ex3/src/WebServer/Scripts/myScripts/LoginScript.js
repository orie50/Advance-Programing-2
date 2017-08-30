/*
view model for data binding
*/
var ViewModel = function () {

    var self = this;
    // users api
    var UsersUri = '/api/Users/';

    // check if the user and the password exist
    self.checkUser = function () {
        var username = $('#Name').val();
        var password = $('#Password').val();
        // send to server username + password
        $.getJSON(UsersUri + username + "/" + password).done(function (data) {

            // if the login secceeded
            var user = data;
            
            alert("welcome " + user["Id"]);
            // add to the username session storage
            sessionStorage.Connect = 1;
            sessionStorage.username = user["Id"];
            window.location.replace("MainPage.html");
        })
        .fail(function (jqXHR, status, errorThrown) {
            // if the page not found
            if (errorThrown == "Not Found") {
                alert('Wrong username or password');
            }
            else {
                alert('Failed send request to server');
            }
        })
    }
};
ko.applyBindings(new ViewModel());
