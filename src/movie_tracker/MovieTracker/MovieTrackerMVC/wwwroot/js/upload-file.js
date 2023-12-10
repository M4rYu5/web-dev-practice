const max_image_size = 2 * 1024 * 1024; // 2MB

/** use setCover to set the cover file */
let cover = null;

// file input
const cover_input = $("<input>").attr("type", "file").attr("accept", "image/*");


$().ready(() => {
    // click the upload cover
    $(".file-upload").on("click", (e) => {
        cover_input.click();
    });

    // modify the look of drag region when drag over
    $(".file-upload").on("dragover", (e) => {
        $(".file-upload").addClass("file-drag-hover");
        e.preventDefault();
        e.stopPropagation()
    });
    $(".file-upload").on("dragleave", (e) => {
        $(".file-upload").removeClass("file-drag-hover");
        e.preventDefault();
        e.stopPropagation();
    });

    // manage file uploads by drag
    $(".file-upload").on("drop", (e) => {
        $(".file-upload").removeClass("file-drag-hover");
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

    updateCoverImageView(cover)
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
    }
    else {
        let reader = new FileReader();
        reader.onload = () => {
            $('#test-image').attr('src', reader.result)
        };
        reader.readAsDataURL(cover);

        $(".file-uplaod-text-container").addClass("d-none");
        $("#test-image").removeClass("d-none");
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


