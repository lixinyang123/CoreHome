const timeOut = 60

function login() {
    $.ajax({
        url: '/Admin/Home/Login',
        type: 'get',
        dataType: 'text',
        success: () => showTime(),
        error: (err) => document.querySelector("#btn_login").innerText = err.responseText
    })
}

function showTime() {
    document.querySelector("#btn_login").setAttribute("disabled", "")
    document.querySelector("#btn_login").innerText = `Get verification code again (${timeOut}s)`
    timeOut--
    if (timeOut < 0) {
        let btn = document.querySelector("#btn_login")
        btn.innerText = "Get verification code again"
        btn.removeAttribute("disabled")
        timeOut = 60
        return
    }
    setTimeout(showTime, 1000)
}

function init() {
    $('#loginCarousel').on('slid.bs.carousel', () => {
        let method = document.querySelector('.active').getAttribute('loginMethod')
        localStorage.setItem("loginMethod", method)
    })

    let methods = Array.from(document.querySelectorAll('.carousel-item'))
        .map(i => i.getAttribute('loginMethod'))
    
    let method = localStorage.getItem("loginMethod")

    if (!method || !methods.includes(method))
        method = 'VerificationCode'

    document.querySelector(`[loginMethod='${method}']`).className = 'carousel-item active'
}

init()
