
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

//=============Initializa================

function initHome() {
    musicSrc = document.getElementById("player").src;
    playMusic();
    addLinkTarget();
}

initHome();

//============Snowfall EasterEggs==========
document.onreadystatechange = () => {
    document.getElementById("EasterEggs").addEventListener("click", () => {
        $(document).snowfall({ flakeCount: 100, maxSpeed: 5 });
        document.querySelector("#profile-thumb").src = "https://lllxy.oss-cn-shenzhen.aliyuncs.com/CoreHome/Images/f.jpg";
        document.querySelector("#content > div > div > header > div.container > div > div > div > div > h1 > span").innerText = "f j h";
        document.querySelector("#content > div > div:nth-child(4) > header > div.container > div > div > div > div > h2").innerText = "宝贝爱你嗷❤~";
        document.querySelector("#content > div > div:nth-child(4) > header > div.container > div > div > div > div > h3:nth-child(4) > span").innerText = "🌹🌹🌹🌹🌹🌹🌹🌹🌹🌹🌹🌹"
        document.querySelector("#content > div > div:nth-child(4) > header > div.container > div > div > div > div > h3:nth-child(6) > span").innerText = "🎉🎉🎉🎉🎉🎉🎉🎉🎉";
        MoveTop();
    });
}