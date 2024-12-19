$(function () {
    $('[data-toggle="popover"]').popover()
})

$('#search > input').popover({
    html: true,
    trigger: "manual"
});

async function presearch() {
    var keyword = document.querySelector("#search > input").value.trim();

    if (!keyword) {
        hiddenPreSearch();
        return;
    }

    var res = await fetch(`/Service/PreSearch/${keyword}`);
    var articles = await res.json();

    if (articles.length <= 0) {
        hiddenPreSearch();
        return;
    }

    let list = "";

    articles.forEach(i => {
        let reg = eval(`/${keyword}/gi`);
        i.title = i.title.replace(reg, `<span class="bg-warning">${keyword}</span>`);
        i.overview = i.overview.replace(reg, `<span class="bg-warning">${keyword}</span>`);

        list += `
            <li class="list-group-item p-1">
                <p><a href="/Blog/Detail/${i.articleCode}">${i.title}</a></p>
                <div class="alert alert-light p-0 border-0" role="alert">
                    ${i.overview}
                </div>
            </li>`;
    });

    let html = `<ul class='list-group list-group-flush'>${list}</ul>`;

    document.querySelector("input").setAttribute("data-content", html);
    $('#search > input').popover("show");
}

function hiddenPreSearch() {
    document.querySelector("input").removeAttribute("data-content");
    $('#search > input').popover("hide");
}

let input = document.querySelector("#search > input");

input.onkeydown = e => {
    if (e.keyCode != 13) return
    let keyword = input.value.trim()
    if (keyword)
        location.href = `/Blog/Search/${keyword}`;
}
