let player, musicSrc, typed;

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

function showInfo() {
    let infoList = document.querySelector("#infoList").value.split("#");
    typed = new Typed('#info', {
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
    showInfo();
    player = document.querySelector("#player");
    musicSrc = player.src;
    playMusic();
}

initHome();