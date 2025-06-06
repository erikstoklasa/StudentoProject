﻿document.addEventListener('DOMContentLoaded', (event) => {
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
});