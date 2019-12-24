function submit() {

    var contact = document.getElementById("detail").value;
    var title = document.getElementById("verfyInput").value;

    var postData = {

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
        error: function () {
            alert("验证码错误");
            document.getElementById("verfyImg").click();
        }
    });
}