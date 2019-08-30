function initPage() {
    var md = document.getElementById("md").value;
    document.getElementById('detail').innerHTML = marked(md);

    hljs.initHighlighting();
    hljs.initLineNumbersOnLoad();
}