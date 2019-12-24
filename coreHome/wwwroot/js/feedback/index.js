function submit() {

    var contact = document.getElementById("contact").value;
    var title = document.getElementById("title").value;
    var message = document.getElementById("message").value;
    var verificationCode = document.getElementById("verificationCode").value;

    var postData = {
        contact: contact,
        title: title,
        message: message,
        verificationCode: verificationCode
    };

    console.log(postData);

    $.ajax({
        type: 'POST',
        url: "/FeedBack/Index",
        data: postData,
        success: function () {
            window.location.href = "/Home/Message?msg=感谢您的反馈，开发者会尽快答复&url=/Feedback";
        },
        error: function (err) {
            alert(JSON.parse(err.responseText).detail);
            document.getElementById("verfyImg").click();
        }
    });
}