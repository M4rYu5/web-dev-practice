const max_image_size = 2 * 1024 * 1024; // 2MB

/** use setCover to set the cover file */
let cover = null;

// file input
const cover_input = $("<input>").attr("type", "file").attr("accept", "image/*");


$().ready(() => {
    const uploadElement = $(".file-upload");
    uploadElement.addClass("file-upload-empty");

    $("#remove-cover").on("click", () => setCover(null))

    // click the upload cover
    uploadElement.on("click", (e) => {
        cover_input.click();
    });

    // modify the look of drag region when drag over
    uploadElement.on("dragover", (e) => {
        uploadElement.addClass("file-drag-hover");
        e.preventDefault();
        e.stopPropagation()
    });
    uploadElement.on("dragleave", (e) => {
        uploadElement.removeClass("file-drag-hover");
        e.preventDefault();
        e.stopPropagation();
    });

    // manage file uploads by drag
    uploadElement.on("drop", (e) => {
        uploadElement.removeClass("file-drag-hover");
        e.preventDefault();
        e.stopPropagation();
        fileDropped(e);
    });

    // manage file uploads by click
    cover_input.on("change", (e) => {
        let files = e.originalEvent.target.files;
        setCover(files);
    });
})


// handle paste of an image file
document.onpaste = function (event) {
    var items = (event.clipboardData || event.originalEvent.clipboardData).items;
    for (index in items) {
        var item = items[index];
        if (item.type != null && item.type.startsWith('image/')) {
            var blob = item.getAsFile();
            setCover([blob]);
        }
    }
}


/**
 * Check the file and set the input or the error message
 * @param {*} dragEvent the "drag" event
 */
function fileDropped(dragEvent) {
    let files = dragEvent.originalEvent.dataTransfer.files;
    setCover(files);
}

/**
 * Checks and set errors for uploaded files (client side only).
 * @param {FileList} file list of uploaded files
 */
function setCover(files) {
    if (files == null) {
        cover = null;
        updateCoverImageView(cover);
        return;
    }

    let error = checkFiles(files);
    if (error == null) {
        cover = files[0];
        $("#image-validation").text("");
    }
    else {
        cover = null;
        $("#image-validation").text(error);
    }

    updateCoverImageView(cover);
}


/**
 * Updates the upload view with the new image
 * or set's it back to text
 * @param {File} cover (probably a file)
 */
function updateCoverImageView(cover) {
    if (cover == null) {
        $(".file-uplaod-text-container").removeClass("d-none");
        $("#test-image").addClass("d-none");
        $(".file-upload").addClass("file-upload-empty");
        $("#remove-cover").addClass("d-none");
    }
    else {
        let reader = new FileReader();
        reader.onload = () => {
            $('#test-image').attr('src', reader.result)
        };
        reader.readAsDataURL(cover);

        $(".file-uplaod-text-container").addClass("d-none");
        $("#test-image").removeClass("d-none");
        $(".file-upload").removeClass("file-upload-empty");
        $("#remove-cover").removeClass("d-none");
    }
}



/**
 * Check if the provided files are good to be uploaded.
 * @param {FileList} files Files to check
 * @returns {string | null} error message.
 */
function checkFiles(files) {
    if (files.length > 1) {
        return "Only one cover image is permitted per title.";
    }

    if (!files[0].type.startsWith('image/')) {
        return "The cover must be an image.";
    }

    if (files[0].size > max_image_size) {
        return "The maximum allowed size is 2MB.";
    }

    return null;
}