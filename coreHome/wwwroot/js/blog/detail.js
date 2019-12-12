function initPage() {
    var mdObj = document.getElementById("md")
    var md = mdObj.value;
    document.getElementById('detail').innerHTML = marked(md);

    hljs.initHighlighting();
    hljs.initLineNumbersOnLoad();

    mdObj.parentElement.removeChild(mdObj);
}