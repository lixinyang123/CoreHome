editormd("editor", {
    width: "100%",
    height: "700px",
    path: "/Admin/lib/editor.md/lib/",
    emoji: true,
    imageUpload: true,
    imageFormats: ["jpg", "png", "gif", "jpeg", "webp"],
    imageUploadURL: "/Admin/Blog/UploadPic",
    htmlDecode: "style,script,iframe"
});

if (location.href.includes("Upload")) {
    setInterval(() => {
        let tempArticle = {
            "Title": document.querySelector("#Title").value,
            "CategoryName": document.querySelector("#CategoryName").value,
            "TagStr": document.querySelector("#TagStr").value,
            "Overview": document.querySelector("#Overview").value,
            "Content": document.querySelector("#Content").innerText,
            "__RequestVerificationToken": document.querySelector('[name=__RequestVerificationToken]').value
        };

        fetch('/Admin/Blog/Save', {
            method: 'POST',
            body: JSON.stringify(tempArticle)
        }).catch(err => {
            console.log(err)
        })

    }, 5000);
}