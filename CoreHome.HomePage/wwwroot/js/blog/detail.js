function submit() {

    var id = document.getElementById("articleid").value;
    var detail = document.getElementById("message").value;
    var code = document.getElementById("verfyInput").value;

    var postData = {
        id: id,
        detail: detail,
        code: code
    };

    console.log(postData);

    $.ajax({
        type: 'POST',
        url: "/Blog/Comment",
        data: postData,
        success: function () {
            window.location.href = "/Home/Message?msg=评论成功&url=/Blog/Detail?articleID=" + id;
        },
        error: function (err) {
            alert(JSON.parse(err.responseText).detail);
            document.getElementById("verfyImg").click();
        }
    });
}