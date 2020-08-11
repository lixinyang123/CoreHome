function upload() {
    document.querySelector("#fileSelector").click();
}

function uploadAvatar() {
    var uploading = document.querySelector("#uploading");
    uploading.innerText = "上传中...";
    uploading.setAttribute("disabled", "disabled");

    var formData = new FormData();
    formData.append("avatar", document.querySelector("#fileSelector").files[0]);

    $.ajax({
        url: '/Admin/Profile/UploadAvatar',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            document.querySelector("#avatarUrl").value = data;
            document.querySelector("#avatar").src = data + "?" + Math.random();
            if (confirm("头像上传成功，是否提交？")) {
                document.querySelector("#submit-profile").click();
            }
            uploading.removeAttribute("disabled");
            uploading.innerText = "上传头像";
        },
        error: function (response) {
            alert("上传失败" + response);
            uploading.removeAttribute("disabled");
            uploading.innerText = "上传头像";
        }
    });
}