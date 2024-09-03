


let parent = document.getElementById("preview-container");

parent.innerHTML += createProjectPreviewHTML(
    "Pagination Practice",
    "https://github.com/M4rYu5/web-dev-practice/assets/30922014/7e0829cf-eebb-46bb-a8af-29016f439f9c",
    "https://github.com/m4ryu5/web-dev-practice/tree/main/src/pagination_practice",
    "https://m4ryu5.github.io/web-dev-practice/pagination_practice/pagination.html");

parent.innerHTML += createProjectPreviewHTML(
    "Chat Preview",
    "https://github.com/M4rYu5/web-dev-practice/assets/30922014/8c5b0cb1-93dd-4d16-9da4-3e49065bc2b7",
    "https://github.com/m4ryu5/web-dev-practice/tree/main/src/chat_preview",
    "https://m4ryu5.github.io/web-dev-practice/chat_preview");

parent.innerHTML += createProjectPreviewHTML(
    "Store Front React",
    "https://github.com/M4rYu5/web-dev-practice/assets/30922014/cdc943d8-4783-4ab4-9574-bcff767202f1",
    "https://github.com/m4ryu5/web-dev-practice/tree/main/src/store_front_react",
    "https://m4ryu5.github.io/web-dev-practice/store_front_react");

parent.innerHTML += createProjectPreviewHTML(
    "Movie Tracker",
    "https://github.com/M4rYu5/web-dev-practice/blob/main/content/movie_tracker_list.jpg?raw=true",
    "https://github.com/m4ryu5/web-dev-practice/tree/main/src/movie_tracker",
    "");


function createProjectPreviewHTML(title, imgSrc, codeLink, pageLink) {
    return (`
        <h2>${title}</h2>
        <div class="presentation">
            <div class="presentation-display">
                <img class="presentation-image" src="${imgSrc}"/>
                <div class="link-display">
                    <span>
                        <a href="${codeLink}">
                            <img class="img-icon" src="github-mark-white.png" alt="GitHub Logo"> Code
                        </a>
                    </span>
                    `
        + ((pageLink != "") ? `
                    <a href="${pageLink}">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                            stroke="currentColor" class="w-6 h-6">
                            <path stroke-linecap="round" stroke-linejoin="round"
                                d="M13.19 8.688a4.5 4.5 0 011.242 7.244l-4.5 4.5a4.5 4.5 0 01-6.364-6.364l1.757-1.757m13.35-.622l1.757-1.757a4.5 4.5 0 00-6.364-6.364l-4.5 4.5a4.5 4.5 0 001.242 7.244" />
                        </svg>
                        Page
                    </a>
                </div>
            </div>
        </div>` 
        : ``)
    );
}