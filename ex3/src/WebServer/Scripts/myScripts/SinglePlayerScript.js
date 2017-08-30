//load defualt settings and define buttons events
$(document).ready(function () {
    loadSettings();
    $("#start")[0].onclick = startGame;
    $("#solve")[0].onclick = SolveGame;
});

//load defualt settings
function loadSettings() {
    $('#Name').val("name");
    $('#Rows').val(localStorage.Rows);
    $('#Cols').val(localStorage.Cols);
    $('#Algorithm').val(localStorage.Algorithm);
}

var board;
//load game pictures
exit = new Image();
player_left = new Image();
player_right = new Image();
wall = new Image();
exit.src = "../images/goal.png";
player_left.src = '../images/dave_left.png';
player_right.src = '../images/dave_right.png';
wall.src = '../images/wall.png';

// defines what happens when the player clicks the start game button
function startGame() {
    $("#mazeCanvas").hide();
    $(".loader").show();
    var name = $("#Name").val();
    var rows = $("#Rows").val();
    var cols = $("#Cols").val();
    var apiUrl = '/SinglePlayer/' + name + "/" + rows + "/" + cols;
    $.ajax({
        method: "GET",
        url: apiUrl
    }).done(function (data) {
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
        board = $("#mazeCanvas").mazeBoard(data["Name"], maze,
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
        document.onkeydown = function (e) {
            switch (e.which) {
            case 37:
                board.makeMove("Left");
                break;
            case 38:
                board.makeMove("Up");
                break;
            case 39:
                board.makeMove("Right");
                break;
            case 40:
                board.makeMove("Down");
                break;
            default:
                break;
            }
            //check if the player reached the exit
            if (board.gameFinished() == true) {
                alert("You won!");
                window.location.replace("/Pages/MainPage.html");
            }
        }
        board.drawMaze();
        $(".loader").hide();
        $("#mazeCanvas").show();
        document.title = name;
    });
}
// defines what happens when the player clicks the solve game button
function SolveGame() {
    var alg = $("#Algorithm").val();
    var apiUrl = '/SinglePlayer/' + board._name + "/" + alg;
    $.ajax({
        method: "GET",
        url: apiUrl
    }).done(function (data) {
        var solSrl = data["Solution"];
        var solution = [];
        var i = solSrl.length - 1;
        //convert the list to integers
        for (var c of solSrl) {
            solution[i] = parseInt(c);
            i--;
        }
        board.reset();
        board.solve(solution);
    });
}

