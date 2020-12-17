document.addEventListener('DOMContentLoaded', (event) => {
    let elements = document.querySelectorAll(".timeframe strong");
    let width = document.documentElement.clientWidth;
    let displayModeRow = document.querySelector("#displayModeRow");
    function truncate(str, n) {
        return (str.length > n) ? str.substr(0, n - 1) + '..' : str;
    }
    if (displayModeRow.checked == true) {
        for (let i = 0; i < elements.length; i++) {
            let string = elements[i].textContent.trim();
            if (width < 680) {
                string = truncate(string, 7);
            } else {
                string = truncate(string, 11);
            }
            elements[i].textContent = string;
        }
    } else {
        for (let i = 0; i < elements.length; i++) {
            let string = elements[i].textContent.trim();
            if (width < 680) {
                string = truncate(string, 8);
            } else {
                string = truncate(string, 20);
            }
            elements[i].textContent = string;
        }
    }
});