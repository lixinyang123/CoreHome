function changeTheme(type) {
    var themeTypes = document.getElementsByClassName("ThemeType");
    console.log(themeTypes);
    for (var i = 0; i < themeTypes.length; i++) {
        themeTypes[i].removeAttribute("checked");
    }
    document.getElementById(type).setAttribute("checked", "checked");
}

function changeBackground(type) {
    var backgroundTypes = document.getElementsByClassName("BackgroundType");
    for (var i = 0; i < backgroundTypes.length; i++) {
        backgroundTypes[i].removeAttribute("checked");
    }
    document.getElementById(type).setAttribute("checked", "checked");
}

function selectImage() {
    document.getElementById("backgroundImage").click();
}