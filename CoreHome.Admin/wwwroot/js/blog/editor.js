﻿editormd("editor", {
    width: "100%",
    height: "700px",
    path: "/Admin/lib/editor.md/lib/",
    emoji: true,
    imageUpload: true,
    imageFormats: ["jpg", "png", "gif"],
    imageUploadURL: "/Admin/Blog/UploadPic",
    htmlDecode: "style,script,iframe"
});

if (location.href.includes("Upload")) {
    console.log("Enable cache");

    setInterval(() => {
        let tempArticle = {
            "Title": document.querySelector("#Title").value,
            "CategoryName": document.querySelector("#CategoryName").value,
            "TagStr": document.querySelector("#TagStr").value,
            "Overview": document.querySelector("#Overview").value,
            "Content": document.querySelector("#Content").innerText,
            "__RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
        };

        $.ajax({
            url: "/Admin/Blog/Save",
            type: "POST",
            data: tempArticle,
            success: function () {
                console.log("save successful");
            },
            error: function () {
                console.log("save defeat");
            }
        });
    }, 5000);
}