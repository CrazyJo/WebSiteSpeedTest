"use strict";
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
        this.results.sort(function (a, b) { return b.mintime - a.mintime; });
    };
    MeasurementsViewModel.prototype.push = function (value) {
        this.results.push(value);
    };
    return MeasurementsViewModel;
}());
exports.MeasurementsViewModel = MeasurementsViewModel;
var MeasurementResult = (function () {
    function MeasurementResult() {
    }
    return MeasurementResult;
}());
exports.MeasurementResult = MeasurementResult;
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
var TestResult = (function () {
    function TestResult() {
    }
    return TestResult;
}());
exports.TestResult = TestResult;
//# sourceMappingURL=model.js.map