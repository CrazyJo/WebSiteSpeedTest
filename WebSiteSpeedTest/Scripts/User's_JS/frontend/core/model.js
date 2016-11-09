"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var MeasurementsViewModel = (function () {
    function MeasurementsViewModel(model) {
        this.results = model;
    }
    MeasurementsViewModel.prototype.sortModelExceptFirst = function () {
        var first = this.results.shift();
        this.sortModel();
        this.results.unshift(first);
    };
    MeasurementsViewModel.prototype.sortModel = function () {
        this.results.sort(function (a, b) { return a.mintime - b.mintime; });
    };
    MeasurementsViewModel.prototype.push = function (value) {
        this.results.push(value);
    };
    return MeasurementsViewModel;
}());
exports.MeasurementsViewModel = MeasurementsViewModel;
var HistoryAjaxData = (function () {
    function HistoryAjaxData(startIndex) {
        this.startIndex = startIndex;
    }
    return HistoryAjaxData;
}());
exports.HistoryAjaxData = HistoryAjaxData;
var MeasurementResult = (function () {
    function MeasurementResult() {
    }
    return MeasurementResult;
}());
exports.MeasurementResult = MeasurementResult;
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