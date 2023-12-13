setTimeout(function () {
    $("#msgAlert").fadeOut("slow");
}, 2000);

// Show/hide password onClick of button using Javascript only

// https://stackoverflow.com/questions/31224651/show-hide-password-onclick-of-button-using-javascript-only

function show() {
    var p = document.getElementById('pwd'); 
    p.setAttribute('type', 'text');  
}
function show1() {
    var p1 = document.getElementById('pwdcon');   
    p1.setAttribute('type', 'text');
}
function hide() {
    var p = document.getElementById('pwd');
    p.setAttribute('type', 'password');  
}
function hide1() {
    var p1 = document.getElementById('pwdcon');
    p1.setAttribute('type', 'password');
}

var pwShown = 0;
var pwShown1 = 0;

document.getElementById("eye").addEventListener("click", function () {
    if (pwShown == 0) {
        pwShown = 1;
        show();
    } else {
        pwShown = 0;
        hide();
    }
}, false);

document.getElementById("eye1").addEventListener("click", function () {
    if (pwShown1 == 0) {
        pwShown1 = 1;
        show1();
    } else {
        pwShown1 = 0;
        hide1();
    }
}, false);

