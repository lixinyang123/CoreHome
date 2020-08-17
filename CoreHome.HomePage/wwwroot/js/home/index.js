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

//=============Initializa================

function initHome() {
    player = document.querySelector("#player");
    musicSrc = player.src;
    playMusic();
}

initHome();