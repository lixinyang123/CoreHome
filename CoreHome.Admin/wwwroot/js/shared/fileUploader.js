function upload() {
    document.querySelector("#fileSelector").click();
}

function uploadingState(flag, text) {
    var uploading = document.querySelector("#uploading");
    if (flag) {
        uploading.innerText = "Uploading...";
        uploading.setAttribute("disabled", "disabled");
    }
    else {
        uploading.innerText = text;
        uploading.removeAttribute("disabled");
    }
}

function uploadAvatar() {
    uploadingState(true, "Upload Avatar");

    var formData = new FormData();
    formData.append("file", document.querySelector("#fileSelector").files[0]);

    $.ajax({
        url: "/Admin/Profile/UploadAvatar",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            document.querySelector("#avatarUrl").value = data.toString();
            document.querySelector("#avatar").src = data.toString() + "?" + Math.random();
            alert("Upload successful");
            uploadingState(false, "Upload Avatar");
        },
        error: function (response) {
            alert("Upload failed " + response.toString());
            uploadingState(false, "Upload Avatar");
        }
    });
}

function uploadProjectCover() {
    uploadingState(true, "Upload Cover");

    var formData = new FormData();
    formData.append("file", document.querySelector("#fileSelector").files[0]);

    $.ajax({
        url: "/Admin/Project/UploadCover",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            document.querySelector("#coverUrl").value = data.toString();
            alert("Upload successful");
            uploadingState(false, "Upload Avatar");
        },
        error: function (response) {
            alert("Upload failed " + response.toString());
            uploadingState(false, "Upload Avatar");
        }
    });
}