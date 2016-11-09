"use strict";
var ElementDisplayer = (function () {
    function ElementDisplayer(elementId, model) {
        this.htmlElement = document.querySelector(elementId);
        this.model = model;
    }
    ElementDisplayer.prototype.display = function (model) {
        if (model) {
            this.displayFromOuterModel(model);
        }
        else {
            this.displayFromLocalModel();
        }
    };
    ElementDisplayer.prototype.show = function () {
        this.htmlElement.style.display = "block";
    };
    ElementDisplayer.prototype.hide = function () {
        this.htmlElement.style.display = "none";
    };
    return ElementDisplayer;
}());
exports.ElementDisplayer = ElementDisplayer;
//# sourceMappingURL=elementDisplayer.js.map