function showCode() {
    document.querySelector("#qrcode").innerHTML = "";
    new QRCode(document.querySelector("#qrcode"), window.location.href);
}

function initializeCommentLoader() {
    const comments = document.querySelector("#comments");
    const button = document.querySelector("#load-more-comments");

    if (!comments || !button) {
        return;
    }

    button.addEventListener("click", async () => {
        button.disabled = true;
        button.textContent = "Loading...";

        try {
            const url = new URL(comments.dataset.url, window.location.href);
            url.searchParams.set("skip", comments.dataset.skip);

            const response = await fetch(url);
            if (!response.ok) {
                throw new Error("Failed to load comments.");
            }

            const batch = document.createElement("div");
            batch.innerHTML = await response.text();
            const batchElement = batch.querySelector(".comment-batch");
            if (!batchElement) {
                throw new Error("Invalid comment response.");
            }

            const commentItems = [...batchElement.querySelectorAll(".comment-item")];
            const commentList = document.querySelector("#comment-list");
            commentList.append(...commentItems);
            comments.dataset.skip = Number(comments.dataset.skip) + commentItems.length;

            commentItems.forEach(item => {
                item.querySelectorAll(".markdown").forEach(element => {
                    const id = guid();
                    element.setAttribute("id", id);
                    render(id);
                });
                item.querySelectorAll(".loading").forEach(element => element.remove());
                item.querySelectorAll(".markdown img").forEach(image => {
                    image.onclick = () => { new Viewer(image).show(); };
                });
            });

            if (batchElement.dataset.hasMore !== "true") {
                button.remove();
                return;
            }

            button.disabled = false;
            button.textContent = "Load mroe";
        } catch (error) {
            console.error("Failed to load more comments.", error);
            button.disabled = false;
            button.textContent = "Load mroe";
        }
    });
}

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initializeCommentLoader, { once: true });
} else {
    initializeCommentLoader();
}
