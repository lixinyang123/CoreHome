function getAllTags() {
    $.ajax({
        url: "/Tags/AllTags",
        type: "GET",
        success: function (data) {
            showTags(data);
        }
    });
}

function showTags(allTags) {
    if (allTags.length > 0) {
        let wordcloud = document.querySelector("#allTags");
        wordcloud.style.minHeight = "500px";
        WordCloud(wordcloud, {
            gridSize: 20,
            weightFactor: 3,
            shrinkToFit: true,
            hover: window.drawBox,
            click: function (item) {
                window.location.href = "/Blog/Tags/" + item[0];
            },
            backgroundColor: 'white',
            list: allTags
        });
    }
}