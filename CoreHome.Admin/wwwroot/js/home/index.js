let timeOut = 60;

function login() {
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
    document.querySelector("#btn_login").setAttribute("disabled", "");
    document.querySelector("#btn_login").innerText = `Get verification code again (${timeOut}s)`;
    timeOut--;
    if (timeOut < 0) {
        let btn = document.querySelector("#btn_login");
        btn.innerText = "Get verification code again";
        btn.removeAttribute("disabled");
        timeOut = 60;
        return;
    }
    setTimeout(showTime, 1000);
}

function changeLoginFunc(index) {
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