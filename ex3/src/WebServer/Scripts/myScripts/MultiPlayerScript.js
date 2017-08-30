//load defualt settings and define buttons events
$(document).ready(function () {
    loadSettings();
    $("#join")[0].onclick = JoinGame;
    $("#start")[0].onclick = StartGame;
});
//load defualt settings
function loadSettings() {
    $('#Name').val("name");
    $('#Rows').val(localStorage.Rows);
    $('#Cols').val(localStorage.Cols);
}

var board, otherBoard;
//load game pictures
exit = new Image();
player_left = new Image();
player_right = new Image();
wall = new Image();
exit.src = "../images/goal.png";
player_left.src = '../images/dave_left.png';
player_right.src = '../images/dave_right.png';
wall.src = '../images/wall.png';
//client id (0 for client who started a game, 0 otherwise)
var id;
var game = $.connection.multiplayerHub;

//function for the server to call when a new move is avaliable
game.client.newState = function (move) {
    //check who made the new move, and draw on the relevant board accordingly
    if (id == move["Id"]) {
        board.makeMove(move["Direction"]);
    } else {
        otherBoard.makeMove(move["Direction"]);
    }
    if (board.gameFinished() == true) {
        game.server.finishGame(board._name);
    }
};

//function for the server to call when the game has ended
game.client.finishGame = function (msg) {
    alert(msg["msg"]);
    window.location.replace("/Pages/MainPage.html");
};

//view model to control the game list in the select menu
function MultiplayerVM() {
    var self = this; // make 'this' available to subfunctions or closures
    self.Games = ko.observableArray(); // enables data binding
};
var vm = new MultiplayerVM();
//function for the server to call when the games list has been updated
game.client.list = function (data) {
    vm.Games(data["games"]);
};

//start the connection
$.connection.hub.start().done(function () {
    ko.applyBindings(vm);
    // Fetch the initial data
    game.server.createList();
});

// defines what happens when the player clicks the start game button
function StartGame() {
    id = 0;
    $("#mazeCanvas").hide();
    $(".loader").show();
    var name = $("#Name").val();
    var rows = $("#Rows").val();
    var cols = $("#Cols").val();
    var username = sessionStorage.username;
    game.server.startGame(name, rows, cols, username);
};

// defines what happens when the player clicks the join game button
function JoinGame() {
    var name = $("#select").val();
    var username = sessionStorage.username;
    if (name == undefined) {
        alert("You need to choose game");
        return;
    } 
    if (board != undefined && name == board._name) {
        alert("can't join a game you started");
        return;
    } 
    id = 1;
    $("#mazeCanvas").hide();
    $(".loader").show();
    game.server.joinGame(name, username);
}

// defines what happens when the player clicks on the keyboard
function OnKeyPress(e) {
    switch (e.which) {
    case 37:
        game.server.addMove(board._name, "Left");
        break;
    case 38:
        game.server.addMove(board._name, "Up");
        break;
    case 39:
        game.server.addMove(board._name, "Right");
        break;
    case 40:
        game.server.addMove(board._name, "Down");
        break;
    default:
        break;
    }
}

/*
*function for the server to call when the game is ready to start
*the function initializes the maze boards
*/
game.client.initGame = function (data) {
    if (data["msg"] != undefined) {
        alert(data["msg"]);
        $(".loader").hide();
        return;
    }
    var cols = data["Cols"];
    var maze = [];
    var row = [];
    var i = 0;
    var mazeSrl = data["Maze"];
    for (var c of mazeSrl) {
        if (i < cols) {
            row[i] = parseInt(c);
            i++;
        } else {
            maze.push(row);
            row = [parseInt(c)];
            i = 1;
        }
    };
    maze.push(row);
    board = $("#mazeCanvas").mazeBoard(data["Name"],
        maze,
        data["Start"]["Row"],
        data["Start"]["Col"],
        data["End"]["Row"],
        data["End"]["Col"],
        player_right,
        player_left,
        exit,
        wall,
        true,
        null);
    otherBoard = $("#otherMazeCanvas").mazeBoard(data["Name"],
        maze,
        data["Start"]["Row"],
        data["Start"]["Col"],
        data["End"]["Row"],
        data["End"]["Col"],
        player_right,
        player_left,
        exit,
        wall,
        true,
        null);
    if (id == 0) {
        alert("waiting for second player");
    }
    if (id == 1) {
        document.onkeydown = OnKeyPress;
        board.drawMaze();
        otherBoard.drawMaze();
        $(".loader").hide();
        $("#mazeCanvas").show();
        $("#otherMazeCanvas").show();
        document.title = name;
    }
};
/*
*function for the server to call when the second player has connected.
*the function draws the boards.
*/
game.client.startGame = function() {
    document.onkeydown = OnKeyPress;
    board.drawMaze();
    otherBoard.drawMaze();
    $(".loader").hide();
    $("#mazeCanvas").show();
    $("#otherMazeCanvas").show();
    document.title = name;
}