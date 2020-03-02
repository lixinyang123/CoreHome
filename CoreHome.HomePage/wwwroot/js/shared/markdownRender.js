function guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function render(id) {
    editormd.markdownToHTML(id, {
        htmlDecode: "style,script,iframe",
        emoji: true,
        taskList: true,
        flowchart: true,
        sequenceDiagram: true,
    });
}

function init() {
    document.querySelectorAll(".markdown").forEach(element => {
        var id = guid();
        element.setAttribute("id", id);
        render(id);
    });
}