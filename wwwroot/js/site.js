// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
document.addEventListener('DOMContentLoaded', (event) => {
    //Search
    var searchUsers = document.querySelector('#search'),
        users = document.querySelectorAll('tbody tr'),
        usersData = document.querySelectorAll('.search-data'),
        searchVal;
    if (searchUsers != null) {
        searchUsers.addEventListener('keyup', function () {
            searchVal = this.value.trim().toLowerCase();
            for (var i = 0; i < users.length; i++) {
                if (!searchVal || usersData[i].textContent.toLowerCase().indexOf(searchVal) > -1) {
                    users[i].style['display'] = 'table-row';
                }
                else {
                    users[i].style['display'] = 'none';
                }
            }
        });
    }

    //Timetable string shortener
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