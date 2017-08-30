/*
view model for data binding
*/
var ViewModel = function () {
    var self = this;

    self.Ranks = ko.observableArray();
    self.rank = ko.observableArray();
    var RanksUri = '/api/Ranks/';
    // get all the rank table
    function getRanks() {
        $.getJSON(RanksUri).done(function (data) {
            // sort the user ranks by wins - loses 
            data.sort(function (a, b) { return (b.GamesWon - b.GamesLost) - (a.GamesWon - a.GamesLost) });
            rank = 1;
            // add the right rank for the user
            data.forEach(function (user) {
                user.Rank = rank;
                rank = rank + 1;
            }

            );
            self.Ranks(data);
        });
    }
    getRanks();
};
ko.applyBindings(new ViewModel());
