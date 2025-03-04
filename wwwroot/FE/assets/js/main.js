/**
 *
 * @param {String} query
 * @param {String} variable
 * @returns {String|Boolean}
 */
var BBGetQueryVariable = BBGetQueryVariable || function (query, variable) {
    if (typeof query !== 'string' || query == '' || typeof variable == 'undefined' || variable == '') {
        return '';
    }

    var vars = query.split("&");

    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");

        if (pair[0] == variable) {
            return pair[1];
        }
    }
    return (false);
};

var BBGetUrlParameter = BBGetUrlParameter || function (url, parameter_name) {
    parameter_name = parameter_name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + parameter_name + '=([^&#]*)');
    var results = regex.exec(url);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};