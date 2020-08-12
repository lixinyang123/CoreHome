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
    formData.append("avatar", document.querySelector("#fileSelector").files[0]);

    $.ajax({
        url: "/Admin/Profile/UploadAvatar",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            document.querySelector("#avatarUrl").value = data;
            document.querySelector("#avatar").src = data + "?" + Math.random();
            if (confirm("Upload successful, whether to submit？")) {
                document.querySelector("#submit-profile").click();
            }
            uploadingState(false, "Upload Avatar");
        },
        error: function (response) {
            alert("Upload failed " + response);
            uploadingState(false, "Upload Avatar");
        }
    });
}

function uploadProjectCover() {
    uploadingState(true, "Upload Cover");

    var formData = new FormData();
    formData.append("cover", document.querySelector("#fileSelector").files[0]);

    $.ajax({
        url: "/Admin/Project/UploadCover",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            document.querySelector("#coverUrl").value = data;
            alert("Upload successful");
            uploadingState(false, "Upload Avatar");
        },
        error: function (response) {
            alert("Upload failed " + response);
            uploadingState(false, "Upload Avatar");
        }
    });
}