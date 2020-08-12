var musicSrc = undefined;

function playMusic() {
    var player = document.querySelector("#player");
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
    musicSrc = document.querySelector("#player").src;
    window.onreadystatechange = playMusic();
}

initHome();