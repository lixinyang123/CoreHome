function upload() {
    var selector = document.getElementById("selector");
    selector.onchange = () => {
        if (selector.value != undefined && selector.value != null && selector.value != "") {
            document.getElementById("submit").click();
        }
    }
    selector.click();
}