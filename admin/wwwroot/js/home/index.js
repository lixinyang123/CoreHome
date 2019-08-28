function signIn() {
    var btn = document.getElementById("btn_sign");
    btn.innerText = "Get random password again";
    btn.setAttribute("disabled", "");

    document.getElementsByTagName("form")[0].removeAttribute("hidden");

    getPassword();

    setTimeout(resetSignIn, 1000 * 60);
}

function getPassword() {
    $.ajax({
        url: '/Home/Login',
        type: 'get',
        dataType: 'text',
        error: error => console.log(error)
    });
}

function resetSignIn() {
    document.getElementById("btn_sign").removeAttribute("disabled");
}