"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var HistoryAjaxData = (function () {
    function HistoryAjaxData(startIndex) {
        this.startIndex = startIndex;
    }
    return HistoryAjaxData;
}());
exports.HistoryAjaxData = HistoryAjaxData;
var SitemapAjaxData = (function (_super) {
    __extends(SitemapAjaxData, _super);
    function SitemapAjaxData(startIndex, rowId) {
        _super.call(this, startIndex);
        this.historyRowId = rowId;
    }
    return SitemapAjaxData;
}(HistoryAjaxData));
exports.SitemapAjaxData = SitemapAjaxData;
var HistoryPage = (function () {
    function HistoryPage() {
    }
    return HistoryPage;
}());
exports.HistoryPage = HistoryPage;
var HistoryPager = (function () {
    function HistoryPager() {
    }
    return HistoryPager;
}());
exports.HistoryPager = HistoryPager;
//# sourceMappingURL=model.js.map