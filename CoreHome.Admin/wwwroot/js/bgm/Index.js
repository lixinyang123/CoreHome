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

    var reqUrl = "/Admin/Bgm/SetBgmType?bgmType=" + bgmType;

    if (url != null && url != "") {
        $.ajax({
            type: 'POST',
            url: reqUrl,
            data: { url: url },
            success: function () {
                window.location.reload();
            }
        });
    }
}

function play(musicName) {
    var url = "/Admin/Bgm/Play?musicName=" + musicName;
    document.getElementById("player").src = url;
}