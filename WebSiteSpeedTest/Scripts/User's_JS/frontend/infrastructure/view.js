/// <reference path="../../../typings/jquery/jquery.d.ts" />
"use strict";
var Enums = require("./enums");
var PagerBtn = (function () {
    function PagerBtn(element) {
        this.element = $(element);
    }
    Object.defineProperty(PagerBtn.prototype, "click", {
        set: function (value) {
            this.element.click(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagerBtn.prototype, "isEnabled", {
        get: function () {
            return !this.element.parent().hasClass("disabled");
        },
        set: function (value) {
            if (value)
                this.element.parent().removeClass("disabled");
            else
                this.element.parent().addClass("disabled");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagerBtn.prototype, "role", {
        get: function () {
            return Enums.PagerElementRole[this.element.attr("data-role")];
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagerBtn.prototype, "startIndex", {
        get: function () {
            return +this.element.attr("data-start-index");
        },
        set: function (value) {
            this.element.attr("data-start-index", value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagerBtn.prototype, "url", {
        get: function () {
            return this.element.attr("href");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PagerBtn.prototype, "rowId", {
        get: function () {
            return this.element.attr("data-history-row-id");
        },
        enumerable: true,
        configurable: true
    });
    return PagerBtn;
}());
exports.PagerBtn = PagerBtn;
var Pager = (function () {
    function Pager(selector) {
        this.selector = selector;
        this.getPagerElements(selector);
    }
    Pager.prototype.getPagerElements = function (selector) {
        var _this = this;
        var elments = $(selector);
        if (elments.length !== 0) {
            elments.each(function (index, elem) {
                switch (Enums.PagerElementRole[$(elem).attr("data-role")]) {
                    case Enums.PagerElementRole.Next:
                        _this.nextBtn = new PagerBtn(elem);
                        break;
                    case Enums.PagerElementRole.Previous:
                        _this.previousBtn = new PagerBtn(elem);
                        break;
                }
            });
        }
        else {
            throw "selector does not indicate page elements";
        }
    };
    return Pager;
}());
exports.Pager = Pager;
var Preloader = (function () {
    function Preloader(preloaderId) {
        this.preloader = $(preloaderId);
    }
    Preloader.prototype.hide = function () {
        this.preloader.hide();
    };
    Preloader.prototype.show = function () {
        this.preloader.show();
    };
    return Preloader;
}());
exports.Preloader = Preloader;
var FormGroup = (function () {
    function FormGroup() {
    }
    return FormGroup;
}());
exports.FormGroup = FormGroup;
//# sourceMappingURL=view.js.map