var board = {};

boardDiv = document.querySelector("#board");
again = document.querySelector("#again");
scoreSpan = document.querySelector("#score");
lossText = document.querySelector("#lost");
decreaseBtn = document.querySelector("#decrease-size");
increaseBtn = document.querySelector("#increase-size");

var score = 0;
var boardSize = 4;
const TWO_CHANCE = 0.8;

var playing = true;

//saved game states
localStorage = window.localStorage;

//button assignments
again.onclick = resetBoard;
document.onkeydown = keyInput;
decreaseBtn.onclick = decreaseSize;
increaseBtn.onclick = increaseSize;

document.addEventListener('touchstart', handleTouchStart, false);
document.addEventListener('touchmove', handleTouchMove, false);

var xDown = null;
var yDown = null;

function getTouches(evt) {
    return evt.touches ||             // browser API
        evt.originalEvent.touches; // jQuery
}

function handleTouchStart(evt) {
    const firstTouch = getTouches(evt)[0];
    xDown = firstTouch.clientX;
    yDown = firstTouch.clientY;
};

function handleTouchMove(evt) {
    if (!xDown || !yDown) {
        return;
    }

    var xUp = evt.touches[0].clientX;
    var yUp = evt.touches[0].clientY;

    var xDiff = xDown - xUp;
    var yDiff = yDown - yUp;

    if (Math.abs(xDiff) > Math.abs(yDiff)) {/*most significant*/
        if (xDiff > 0) {
            /* left swipe */
            moveLeft();
        } else {
            /* right swipe */
            moveRight();
        }
    } else {
        if (yDiff > 0) {
            /* up swipe */
            moveUp();
        } else {
            /* down swipe */
            moveDown();
        }
    }
    /* reset values */
    xDown = null;
    yDown = null;
};

//run game
board = JSON.parse(localStorage.getItem("board"));
if (board != null) {
    createBoard();
}
else {
    resetBoard();
}
updateBoard();

//Create board structure
function createBoard() {
    // Remove old board if it exists
    boardDiv.innerHTML = "";

    for (var r = 0; r < boardSize; r++) {
        var row = document.createElement("div");
        row.classList.add("row");
        boardDiv.appendChild(row);
        for (var t = 0; t < boardSize; t++) {
            var tile = document.createElement("div");
            tile.classList.add("tile");
            tile.id = tileKey(t, r);
            row.appendChild(tile);
        }
    }
}

function resetBoard() {

    createBoard();
    lossText.style.display = "none";

    board = {};
    score = 0;
    playing = true;

    //generate first 2 tiles
    addNewTile();
    addNewTile();
    updateBoard();

}

function increaseSize() {
    boardSize++;
    boardSize = Math.min(boardSize, 8);
    resetBoard();
}

function decreaseSize() {
    boardSize--;
    boardSize = Math.max(boardSize, 2);
    resetBoard();
}

//generate tilekey syntax
function tileKey(col, row) {
    return "tile" + col + "-" + row;
}

//respond to keypresses
function keyInput(key) {
    var changed = false;

    //copy board state
    var oldBoard = Object.assign({}, board);

    switch (key.keyCode) {
        //up
        case 38:
        case 87:
            key.preventDefault();
            for (var col = 0; col < boardSize; col++) {
                var newCol = combine(getCol(col, false), true);

                deleteCol(col);

                for (var row = 0; row < newCol.length; row++) {
                    var newKey = tileKey(col, row);
                    board[newKey] = newCol[row];
                }
            }

            break;
        //down
        case 40:
        case 83:
            key.preventDefault();
            for (var col = 0; col < boardSize; col++) {
                var newCol = combine(getCol(col, true), true);

                deleteCol(col);

                for (var row = (boardSize - 1); row > (boardSize - 1) - newCol.length; row--) {
                    var newKey = tileKey(col, row);
                    board[newKey] = newCol[(boardSize - 1) - row];
                }
            }

            break;
        //left
        case 37:
        case 65:
            key.preventDefault();
            for (var row = 0; row < boardSize; row++) {
                var newRow = combine(getRow(row, false), true);

                deleteRow(row);

                for (var col = 0; col < newRow.length; col++) {
                    var newKey = tileKey(col, row);
                    board[newKey] = newRow[col];
                }
            }

            break;
        //right
        case 39:
        case 68:
            key.preventDefault();
            for (var row = 0; row < boardSize; row++) {
                var newRow = combine(getRow(row, true), true);

                deleteRow(row);

                for (var col = (boardSize - 1); col > (boardSize - 1) - newRow.length; col--) {
                    var newKey = tileKey(col, row);
                    board[newKey] = newRow[(boardSize - 1) - col];
                }
            }

            break;
        case 13:
            if (!playing)
                resetBoard();
            break;
        default:
            break;
    }

    if (!boardsEqual(board, oldBoard)) changed = true;

    //only add a new tile if the board was changed
    if (changed) addNewTile();

    updateBoard();
}

function moveUp() {
    for (var col = 0; col < boardSize; col++) {
        var newCol = combine(getCol(col, false), true);

        deleteCol(col);

        for (var row = 0; row < newCol.length; row++) {
            var newKey = tileKey(col, row);
            board[newKey] = newCol[row];
        }
    }
}

function moveDown() {
    for (var col = 0; col < boardSize; col++) {
        var newCol = combine(getCol(col, true), true);

        deleteCol(col);

        for (var row = (boardSize - 1); row > (boardSize - 1) - newCol.length; row--) {
            var newKey = tileKey(col, row);
            board[newKey] = newCol[(boardSize - 1) - row];
        }
    }
}

function moveLeft() {
    for (var row = 0; row < boardSize; row++) {
        var newRow = combine(getRow(row, false), true);

        deleteRow(row);

        for (var col = 0; col < newRow.length; col++) {
            var newKey = tileKey(col, row);
            board[newKey] = newRow[col];
        }
    }
}

function moveRight() {
    for (var row = 0; row < boardSize; row++) {
        var newRow = combine(getRow(row, true), true);

        deleteRow(row);

        for (var col = (boardSize - 1); col > (boardSize - 1) - newRow.length; col--) {
            var newKey = tileKey(col, row);
            board[newKey] = newRow[(boardSize - 1) - col];
        }
    }
}

function deleteRow(row) {
    for (var i = 0; i < boardSize; i++) {
        var oldKey = tileKey(i, row);
        var tile = document.querySelector("#" + oldKey);
        board[oldKey] = null;
    }
}

function deleteCol(col) {
    for (var i = 0; i < boardSize; i++) {
        var oldKey = tileKey(col, i);
        var tile = document.querySelector("#" + oldKey);
        board[oldKey] = null;
    }
}

//visually update board based on board state
function updateBoard() {

    if (playing) {
        //update values
        for (var r = 0; r < boardSize; r++) {
            for (var t = 0; t < boardSize; t++) {
                var key = tileKey(t, r);
                var tile = document.querySelector("#" + key);
                board[key] != null ? tile.innerHTML = board[key] : tile.innerHTML = "";
                tile.className = "tile";
                tile.classList.add("tile-" + board[key]);
            }
        }

        //update score
        scoreSpan.innerHTML = score;

        //check if game is over
        gameOver();
    }
}

//add new tile at random location
function addNewTile() {
    var empties = availableCells();
    var value = Math.random() < TWO_CHANCE ? 2 : 4;
    var newKey = empties[Math.floor(Math.random() * empties.length)];
    board[newKey] = value;

    //save game state
    localStorage.setItem("board", JSON.stringify(board));
}

//get all empty cells
function availableCells() {
    var cells = [];

    for (var r = 0; r < boardSize; r++) {
        for (var t = 0; t < boardSize; t++) {
            var key = tileKey(t, r);
            if (board[key] == null)
                cells.push(key);
        }
    }

    return cells;
}

//game logic to combine numbers
function combine(row, addScore) {
    var newRow = [];

    for (var i = 0; i < row.length; i++) {
        if (row[i] == row[i + 1]) {
            newRow.push(row[i] + row[i + 1]);
            if (addScore) score += (row[i] + row[i + 1]);
            i++;
        }
        else {
            newRow.push(row[i]);
        }
    }

    return newRow;
}

//get row at rowNum
function getRow(rowNum, reverse) {
    var row = [];

    if (reverse) {
        for (var i = (boardSize - 1); i >= 0; i--) {
            var key = tileKey(i, rowNum);
            var tile = document.querySelector("#" + key);
            if (board[key] != null) {
                row.push(board[key]);
            }
        }
    }
    else {
        for (var i = 0; i < boardSize; i++) {
            var key = tileKey(i, rowNum);
            var tile = document.querySelector("#" + key);
            if (board[key] != null) {
                row.push(board[key]);
            }
        }
    }

    return row;
}

//get col at colNum
function getCol(colNum, reverse) {
    var col = [];

    if (reverse) {
        for (var i = (boardSize - 1); i >= 0; i--) {
            var key = tileKey(colNum, i);
            var tile = document.querySelector("#" + key);
            if (board[key] != null) {
                col.push(board[key]);
            }
        }
    }
    else {
        for (var i = 0; i < boardSize; i++) {
            var key = tileKey(colNum, i);
            var tile = document.querySelector("#" + key);
            if (board[key] != null) {
                col.push(board[key]);
            }
        }
    }

    return col;
}

function boardsEqual(a, b) {

    for (var key in a) {
        if (a.hasOwnProperty(key)) {
            if (a[key] != b[key]) return false;
        }
    }

    return true;
}

//check if any more moves are possible
function gameOver() {

    var fakeOldBoard = Object.assign({}, board);
    var fakeNewBoard = Object.assign({}, board);

    //horizontal check
    for (var row = 0; row < boardSize; row++) {
        var newRow = combine(getRow(row, false), false);

        for (var col = 0; col < newRow.length; col++) {
            var newKey = tileKey(col, row);
            fakeNewBoard[newKey] = newRow[col];
        }
    }

    if (boardsEqual(fakeOldBoard, fakeNewBoard)) {
        //vertical check
        for (var col = 0; col < boardSize; col++) {
            var newCol = combine(getCol(col, false), false);

            for (var row = 0; row < newCol.length; row++) {
                var newKey = tileKey(col, row);
                fakeNewBoard[newKey] = newCol[row];
            }
        }
    }

    if (boardsEqual(fakeOldBoard, fakeNewBoard) && availableCells().length == 0) {
        playing = false;
        lossText.style.display = "block";
    }
}
