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
        let height = document.querySelector("#content").clientHeight - 200;
        wordcloud.style.minHeight = `${height > 550 ? height : 550}px`;

        WordCloud(wordcloud, {
            gridSize: 20,
            weightFactor: 3,
            shrinkToFit: true,
            click: (item) => {
                window.location.href = "/Blog/Tags/" + item[0];
            },
            backgroundColor: 'white',
            list: allTags
        });
    }
}