function initData() {
    document.getElementById("overview").value = document.getElementById("editoverview").value;
    document.getElementById("content").value = document.getElementById("editcontent").value;
}

function checkData() {
    var dataControl = document.getElementsByClassName("data");
    for (var i = 0; i < dataControl.length; i++) {
        if (dataControl[i].value.trim() == "") {
            return false;
        }
    }
    return true;
}

function postData() {
    if (confirm("确定提交?")) {
        document.getElementById("submit").click();
    }
}

function upload() {
    initData();
    //检测数据是否符合提交规范
    if (checkData()) {
        postData();
    }
    else {
        alert("请完善数据");
    }
}