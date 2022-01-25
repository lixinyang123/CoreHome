function showCode() {
    document.querySelector("#qrcode").innerHTML = "";
    new QRCode(document.querySelector("#qrcode"), window.location.href);
}