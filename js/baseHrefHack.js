var splittedPath = window.location.pathname.split('/');
var base = document.getElementsByTagName('base')[0];
if (window.location.host.includes('localhost')) {
    base.setAttribute('href', '/');
} else if (splittedPath.length > 2) {
    base.setAttribute('href', '/' + splittedPath[1] + '/');
} else if (splittedPath[splittedPath.length - 1].length !== 0) {
    window.location.replace(window.location.origin + window.location.pathname + '/' + window.location.search);
}