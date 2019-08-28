function getPassword() {
    var btn = document.getElementById("btn_sign");
    btn.innerText = "Get random password again";
    btn.setAttribute("disabled", "");

    document.getElementsByTagName("form")[0].removeAttribute("hidden");

    $.ajax({
        url: '/Home/Login',
        type: 'get',
        dataType: 'text',
        success: function (data) {
            alert(data);
        },
        error: error => console.log(error)
    });
}