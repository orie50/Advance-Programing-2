/*
view model for data binding
*/
var ViewModel = function () {

    var self = this;
    self.users = ko.observableArray();
    var UsersUri = '/api/Users/';

    // add new user to data base
    self.addUser = function () {
        var username = $('#Name').val();
        var password = $('#Password').val();
        var verifyPassword = $('#VerifyPassword').val();
        var email = $('#Email').val();
        // check the password and verify password
        if (password != verifyPassword) {
            alert("password don't match to verify password");
            return;
        }

        // create user
        var user = {
            Id: username,
            Password: password,
            Email: email
        };
        $.post(UsersUri, user).done(function (item) {
            // add new user
            self.users.push(item);
            alert("User registered successfully");
            window.location.replace("MainPage.html");
        })
        .fail(function (jqXHR, status, errorThrown) {
            // if wrong arguments
            if (errorThrown == "BadRequest") {
                alert('Wrong details');
            }
                // if the user already exist
            else if (errorThrown == "Conflict") {
                alert('Username already exist');
            }
            else {
                alert('Failed send request to server');
            }
        })
    }
};
ko.applyBindings(new ViewModel());