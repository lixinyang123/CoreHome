function keyDown() {
    var e = window.event || arguments.callee.caller.arguments[0];
    if (e && e.keyCode == 13) {
        var keyword = document.getElementById("keyword").value;
        window.location.href = "/Blog/Search?Keyword=" + keyword;
    }
}