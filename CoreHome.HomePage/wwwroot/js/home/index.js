
function addLinkTarget() {
    document.querySelectorAll(".fh5co-blog").forEach(ele => {
        ele.querySelectorAll("a").forEach(link => {
            link.setAttribute("target", "_blank");
        });
    });
}

var musicSrc = undefined;

function playMusic() {
    var player = document.getElementById("player");
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

function showLogo() {
    console.log(`
          ____               _   _
         / ___|___  _ __ ___| | | | ___  _ __ ___   ___
        | |   / _ \| '__/ _ \ |_| |/ _ \| '_ \` _ \ / _ \
        | |__| (_) | | |  __/  _  | (_) | | | | | |  __/
         \____\___/|_|  \___|_| |_|\___/|_| |_| |_|\___|
    `);
}

//=============Initializa================

function initHome() {
    musicSrc = document.getElementById("player").src;
    playMusic();
    addLinkTarget();
    showLogo();
}

initHome();