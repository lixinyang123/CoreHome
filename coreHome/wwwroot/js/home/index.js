
//============MoveOperation================

function moveToDC() {
    var current = document.scrollingElement.scrollTop;
    current += 10;
    document.scrollingElement.scrollTop = current;
    timer = setTimeout(moveToDC, 0.1);

    if (document.scrollingElement.scrollTop > 810) {
        clearTimeout(timer);
    }
}

//===============RotateLogo================

var angle = 0;

function Rotate() {
    var logo = document.getElementById("user");
    logo.style.transform = "rotate(" + (angle) + "deg)";
    angle += 0.2;
    setTimeout(Rotate, 0.1);
}

//=============loadAnimation================
var speed = 25;

function LoadAnimation() {
    var current = document.scrollingElement.scrollTop;
    current += speed;
    speed--
    document.scrollingElement.scrollTop = current;
    timer = setTimeout(LoadAnimation, 0.5);
    if (document.scrollingElement.scrollTop == 0) {
        clearTimeout(timer);
    }
}

function CookieExist(cookieName) {
    if (document.cookie.indexOf(cookieName) != -1) {
        return true;
    }
    return false;
}

//=============Initializa================
if (!CookieExist("CookiePolicy")) {
    setTimeout(LoadAnimation, 1000);
}

player.addEventListener('ended', function () {
    setTimeout(() => {
        document.getElementById("player").play();
    }, 1000);
});

Rotate();

