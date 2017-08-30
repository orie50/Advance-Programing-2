$(document).ready(function () {
    loadSettings();
});

/*
load settings from local storage
*/
function loadSettings() {
    $('#Rows').val(localStorage.Rows);
    $('#Cols').val(localStorage.Cols);
    $('#Algorithm').val(localStorage.Algorithm);
}

/*
if save button pressed
*/
$('#Save').click(function () {
    localStorage.Rows = $('#Rows').val();
    localStorage.Cols = $('#Cols').val();
    localStorage.Algorithm = $('#Algorithm').val();
});