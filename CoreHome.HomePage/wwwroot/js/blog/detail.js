function showCode() {
    document.querySelector("#qrcode").innerHTML = "";
    new QRCode(document.querySelector("#qrcode"), window.location.href);
}

window.onload = () => {
    let warning = document.querySelector("#warning").value;
    if (warning != "") { alert(warning); }
}