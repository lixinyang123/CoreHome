var allTags = undefined;

function getAllTags() {
    fetch("/Tags/AllTags", {
        method: "GET"
    }).then((res) => {
        res.text().then((text) => {
            allTags = JSON.parse(text);
            showTags();
        });
    });
}

function showTags() {
    WordCloud(document.getElementById('allTags'), {
        gridSize: 18,
        weightFactor: 3,
        fontFamily: 'Finger Paint, cursive, sans-serif',
        hover: window.drawBox,
        click: function (item) {
            window.location.href = "/Blog/Tags/" + item[0];
        },
        backgroundColor: 'white',
        list: allTags
    });
}