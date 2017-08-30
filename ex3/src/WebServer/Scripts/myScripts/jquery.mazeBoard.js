// JQuery Plugin with maze board functionallity
(function($) {
    $.fn.mazeBoard = function(name, mazeData,
        startRow,
        startCol,
        exitRow,
        exitCol,
        playerRightImg,
        playeLeftImg,
        exitImage,
        wallImg,
        isEnabled,
        callback) {

        class mazeBoard {
            constructor(name, mazeData, startRow, startCol, exitRow, exitCol, playerRightImg, playerLeftImg, exitImage, wallImg, isEnabled, callback, canvas) {
                this._name = name;
                this._data = mazeData;
                this._startRow = startRow;
                this._startCol = startCol;
                this._exitRow = exitRow;
                this._exitCol = exitCol;
                this._playerRow = startRow;
                this._playerCol = startCol;
                this._playerLeft = playerLeftImg;
                this._playerRight = playerRightImg;
                this._player = playerRightImg;
                this._exitImg = exitImage;
                this._wallImg = wallImg;
                this._isEnabled = isEnabled;
                this._onmove = callback;
                this._canvas = canvas;
            }
            //reset the maze. draw the player at the start point
            reset() {
                this._playerRow = this._startRow;
                this._playerCol = this._startCol;
                this.drawMaze();
            }
            //draw the current state on the canvas
            drawMaze() {
                var ctx = this._canvas.getContext("2d");
                ctx.strokeStyle = "black";
                var rows = this._data.length;
                var cols = this._data[0].length;
                var width = this._canvas.width / cols;
                var height = this._canvas.height / rows;
                for (var i = 0; i < rows; i++) {
                    for (var j = 0; j < cols; j++) {
                        if (i == this._startRow && j == this._startCol) {
                            ctx.drawImage(this._playerRight, width * j, height * i, width, height);
                        } else if (i == this._exitRow && j == this._exitCol) {
                            ctx.drawImage(this._exitImg, width * j, height * i, width, height);
                        } else if (this._data[i][j] == 1) {
                            ctx.drawImage(this._wallImg, width * j, height * i, width, height);
                        } else if (this._data[i][j] == 0) {
                            ctx.fillRect(width * j, height * i, width, height);
                        }
                    }
                }
            }
            //validate the mive direction
            isValidMove(direction) {
                try
                {
                    switch (direction)
                    {
                    case "Up":
                        return this._data[this._playerRow - 1][this._playerCol] == 0;
                    case "Down":
                        return this._data[this._playerRow + 1][this._playerCol] == 0;
                    case "Right":
                        return this._data[this._playerRow][this._playerCol + 1] == 0;
                    case"Left":
                        return this._data[this._playerRow][this._playerCol - 1] == 0;
                    default:
                        var e = new Error();
                        e.message = "wrong argument in isValidMove";
                        throw e;
                    }
                }
                //in case the movement is outside the maze
                catch (e)
                {
                    return false;
                }  
            }

            //check if the player has reached the exit
            gameFinished() {
                return this._playerCol == this._exitCol && this._playerRow == this._exitRow;
            }

            //public method - move the player in a certain direction
            makeMove(direction) {
                if (this._isEnabled == true) {
                    this.move(direction);
                }
            }
            //private method -move the player in a certain direction
            move(direction) {
                var ctx = this._canvas.getContext("2d");
                var rows = this._data.length;
                var cols = this._data[0].length;
                var width = this._canvas.width / cols;
                var height = this._canvas.height / rows;
                switch (direction) {
                case "Right":
                    this._player = this._playerRight;
                    break;
                case "Left":
                    this._player = this._playerLeft;
                    break;
               default:
                    break;
                }
                /*
                *if the move is valid, clean the last place the player was at
                *and update the player location
                */
                if (this.isValidMove(direction)) {
                    switch (direction) {
                    case "Up":
                        ctx.fillRect(width * this._playerCol, height * this._playerRow, width, height);
                        this._playerRow -= 1;
                        break;
                    case "Down":
                        ctx.fillRect(width * this._playerCol, height * this._playerRow, width, height);
                        this._playerRow += 1;
                        break;
                    case "Right":
                        ctx.fillRect(width * this._playerCol, height * this._playerRow, width, height);
                        this._playerCol += 1;
                        break;
                    case "Left":
                        ctx.fillRect(width * this._playerCol, height * this._playerRow, width, height);
                        this._playerCol -= 1;
                        break;
                    default:
                        var e = new Error();
                        e.message = "wrong argument in move";
                        throw e;
                    }
                    //call the call back function
                    if (self._onmove != null) {
                        self._onmove(this._playerRow, this._playerCol, direction);
                    }
                    //redraw the exit image
                    if (this._playerRow != this._exitRow || this._playerCol != this._exitCol) {
                        ctx.drawImage(this._exitImg,
                            width * this._exitCol,
                            height * this._exitRow,
                            width,
                            height);
                    }
                }
                //draw the player image
                ctx.drawImage(this._player,
                    width * this._playerCol,
                    height * this._playerRow,
                    width,
                    height);
            }

            //create the solve animation
            solve(solution) {
                var timer;
                this._isEnabled = false;
                var board = this;
                var i = 0;
                 function stage() {
                    switch (solution[i]) {
                        case 0: //left 
                            board.move("Left");
                            break;
                        case 1:
                            board.move("Right");
                            break;
                        case 2:
                            board.move("Up");
                            break;
                        case 3:
                            board.move("Down");
                            break;
                    }
                    if (i < solution.length) {
                        i++;
                    } else {
                        board._isEnabled = true;
                        clearInterval(timer);
                    }
                }
                timer = setInterval(stage, 500);
            }
        }
        //return the maze class
        return new mazeBoard(name, mazeData,
            startRow,
            startCol,
            exitRow,
            exitCol,
            playerRightImg,
            playeLeftImg,
            exitImage,
            wallImg,
            isEnabled,
            callback,
            $(this)[0]);
    }
})(jQuery);