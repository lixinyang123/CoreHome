function upload() {
    var selector = document.getElementById("selector");
    selector.onchange = () => {
        if (selector.value != undefined && selector.value != null && selector.value != "") {
            document.getElementById("submit").click();
        }
    }
    selector.click();
}

function fromUrl(bgmType) {
    var currentUrl = document.getElementById("currentUrl").value;
    var url = prompt("请输入url", currentUrl);
    if (url != null && url != "") {
        window.location.href = "/Admin/Bgm/SetBgmType?bgmType=" + bgmType + "&url=" + url;
    }
}

function play(musicName) {
    var url = "/Admin/Bgm/Play?musicName=" + musicName;
    document.getElementById("player").src = url;
}