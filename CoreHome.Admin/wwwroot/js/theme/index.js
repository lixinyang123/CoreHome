function changeTheme(type) {
    let themeTypes = document.querySelectorAll(".ThemeType");
    console.log(themeTypes);
    for (let i = 0; i < themeTypes.length; i++) {
        themeTypes[i].removeAttribute("checked");
    }
    document.getElementById(type).setAttribute("checked", "checked");
}

function changeBackground(type) {
    let backgroundTypes = document.querySelectorAll(".BackgroundType");
    for (let i = 0; i < backgroundTypes.length; i++) {
        backgroundTypes[i].removeAttribute("checked");
    }
    document.getElementById(type).setAttribute("checked", "checked");
}

function selectImage() {
    document.getElementById("fileSelector").click();
}

function uploadBackground() {
    let formData = new FormData();
    formData.append("file", document.querySelector("#fileSelector").files[0]);
    formData.append("__RequestVerificationToken", document.querySelector('[name=__RequestVerificationToken]').value);

    $.ajax({
        url: "/Admin/Theme/UploadBackground",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            document.querySelector('#customImg').src += `?${ Math.random() }`;
            changeBackground('custom');
        },
        error: function () {
            alert("Upload failed");
        }
    });
}