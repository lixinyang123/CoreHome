
function addLinkTarget() {
    document.querySelectorAll(".fh5co-blog").forEach(ele => {
        ele.querySelectorAll("a").forEach(link => {
            link.setAttribute("target", "_blank");
        });
    });
}

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
    playMusic();
    addLinkTarget();
}

initHome();