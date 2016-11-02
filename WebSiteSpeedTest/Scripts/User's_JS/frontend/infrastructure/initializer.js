"use strict";
var handlers_1 = require("./handlers");
var View = require("./view");
var Initializer = (function () {
    function Initializer() {
    }
    Initializer.pagerInit = function (selector, updateTarget) {
        // find pager's btn and set handlers
        var pager;
        try {
            pager = new View.Pager(selector);
        }
        catch (e) {
            return;
        }
        var pagerAjax = new handlers_1.PagerAjaxHandler(pager, updateTarget);
        pager.nextBtn.click = function () {
            pagerAjax.sendAjaxRequest(pager.nextBtn);
        };
        pager.previousBtn.click = function () {
            pagerAjax.sendAjaxRequest(pager.previousBtn);
        };
    };
    return Initializer;
}());
exports.Initializer = Initializer;
//# sourceMappingURL=initializer.js.map