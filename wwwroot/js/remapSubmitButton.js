document.addEventListener('DOMContentLoaded', (event) => {

    let form = document.querySelector("#profile");
    let editButton = document.querySelector("#submit-btn");
    editButton.addEventListener("click", function () {
        form.submit();
    }, false);

});