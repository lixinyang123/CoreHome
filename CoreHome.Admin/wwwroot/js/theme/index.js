function changeTheme(type) {
    var themeTypes = document.getElementsByClassName("ThemeType");
    console.log(themeTypes);
    for (var i = 0; i < themeTypes.length; i++) {
        themeTypes[i].removeAttribute("checked");
    }
    document.getElementById(type).setAttribute("checked", "checked");
}

function changeBackground(type) {
    var backgroundTypes = document.getElementsByClassName("BackgroundType");
    for (var i = 0; i < backgroundTypes.length; i++) {
        backgroundTypes[i].removeAttribute("checked");
    }
    document.getElementById(type).setAttribute("checked", "checked");
}

function selectImage() {
    document.getElementById("fileSelector").click();
}

function uploadBackground() {
    alert("Uploading...")
    var formData = new FormData();
    formData.append("file", document.querySelector("#fileSelector").files[0]);

    $.ajax({
        url: "/Admin/Theme/UploadBackground",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            alert("Upload successful");
            changeBackground('custom');
        },
        error: function () {
            alert("Upload failed");
        }
    });
}

function apply_onclick() {
     if (confirm("更改主题?")) {
         document.getElementById("submit").click();
    }
}