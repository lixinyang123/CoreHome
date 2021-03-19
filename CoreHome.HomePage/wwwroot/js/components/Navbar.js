let input = document.querySelector("#search > input");

input.onkeydown = () => {
    if (window.event.keyCode == 13) {
        let keyword = input.value.trim();
        if (keyword)
            location.href = `/Blog/Search/${keyword}`;
    }
};