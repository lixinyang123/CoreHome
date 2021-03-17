let timeOut = 60;

function signIn() {
    getPassword();
    showTime();
}

function getPassword() {
    $.ajax({
        url: '/Admin/Home/Login',
        type: 'get',
        dataType: 'text',
        success: function (data) {
            alert(data);
        },
        error: error => console.log(error)
    });
}

function showTime() {
    document.querySelector("#btn_sign").setAttribute("disabled", "");
    document.querySelector("#btn_sign").innerText = `Get verification code again（${timeOut}s）`;
    timeOut--;
    if (timeOut < 0) {
        let btn = document.querySelector("#btn_sign");
        btn.innerText = "Get verification code again";
        btn.removeAttribute("disabled");
        timeOut = 60;
        return;
    }
    setTimeout(showTime, 1000);
}

function changeSigninFunc(index) {
    window.localStorage.setItem("SigninFunc", index);
}

function init() {
    let signFunc = localStorage.getItem("SigninFunc");

    if (isNaN(signFunc))
        signFunc = 0;

    if (!signFunc) 
        document.querySelector(".carousel-item").className = "carousel-item active";
    else
        document.querySelectorAll(".carousel-item")[signFunc].className = "carousel-item active";
}

init();