/// <reference path="../../../typings/jquery/jquery.d.ts" />
"use strict";
var Enums = require("./enums");
var Ajax = (function () {
    function Ajax() {
    }
    Ajax.run = function (httpMethodType, url, data, callBackOnDone) {
        $.ajax({
            type: Enums.HttpMethod[httpMethodType],
            url: url,
            data: data
        })
            .done(callBackOnDone);
    };
    return Ajax;
}());
exports.Ajax = Ajax;
//# sourceMappingURL=ajax.js.map