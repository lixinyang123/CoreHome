var player = undefined;
var musicSrc = undefined;

function playMusic() {
    try {
        player.addEventListener('ended', () => {
            setTimeout(() => {
                player.src = musicSrc + "?" + Math.random();
                player.play();
            }, 1000);
        });
        player.play();
    } catch (e) {}
}

function test() {
    var infoList = document.querySelector("#infoList").value.split("#");
    new Typed('#info', {
        strings: infoList,
        typeSpeed: 70,
        backSpeed: 50,
        backDelay: 5000,
        startDelay: 0,
        fadeOut: false,
        loop: true
    });
}

//=============Initializa================

function initHome() {
    test();
    player = document.querySelector("#player");
    musicSrc = player.src;
    playMusic();
}

initHome();