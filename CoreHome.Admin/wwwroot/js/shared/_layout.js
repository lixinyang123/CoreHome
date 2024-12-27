var searchSelector = window.innerWidth < 892
    ? '.side-tool input'
    : '.nav-tool > input';

$(searchSelector).popover({
    html: true,
    trigger: 'manual'
});

async function presearch() {
    let keyword = document.querySelector(searchSelector).value.trim();

    if (!keyword) {
        hiddenPreSearch();
        return;
    }

    let res = await fetch(`/Service/PreSearch/${keyword}`);
    let articles = await res.json();

    if (articles.length <= 0) {
        hiddenPreSearch();
        return;
    }

    let list = '';

    articles.forEach(i => {
        let reg = eval(`/${keyword}/gi`);
        i.title = i.title.replace(reg, `<span class="bg-warning">${keyword}</span>`);
        i.overview = i.overview.replace(reg, `<span class="bg-warning">${keyword}</span>`);

        list += `
            <li class="list-group-item p-1">
                <p><a href="/Admin/Blog/Modify/${i.articleCode}">${i.title}</a></p>
                <div class="alert alert-light p-0 border-0" role="alert">
                    ${i.overview}
                </div>
            </li>`;
    });

    let html = `<ul class='list-group list-group-flush'>${list}</ul>`;

    document.querySelector(searchSelector).setAttribute('data-content', html);
    $(searchSelector).popover('show');
}

function hiddenPreSearch() {
    document.querySelector(searchSelector).removeAttribute('data-content');
    $(searchSelector).popover('hide');
}

let input = document.querySelector(searchSelector);
input.oninput = presearch;
