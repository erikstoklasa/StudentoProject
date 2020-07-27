// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

var searchUsers = document.querySelector('#search'),
    users = document.querySelectorAll('tbody tr'),
    usersData = document.querySelectorAll('.search-data'),
    searchVal;

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


var _paq = window._paq = window._paq || [];
_paq.push(['trackPageView']);
_paq.push(['enableLinkTracking']);
(function () {
    var u = "//erikstoklasa.com/";
    _paq.push(['setTrackerUrl', u + 'matomo.php']);
    _paq.push(['setSiteId', '4']);
    var d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0];
    g.type = 'text/javascript'; g.async = true; g.src = u + 'matomo.js'; s.parentNode.insertBefore(g, s);
})();