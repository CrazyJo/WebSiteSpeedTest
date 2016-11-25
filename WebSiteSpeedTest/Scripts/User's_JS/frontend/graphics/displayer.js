"use strict";
var Model = require("../core/model");
var MeasurementsViewModel = Model.MeasurementsViewModel;
var chartDisplayer_1 = require("./chartDisplayer");
var tableDisplayer_1 = require("./tableDisplayer");
var Displayer = (function () {
    function Displayer(chartId, tableContainerId) {
        this.isHidden = true;
        this.model = new MeasurementsViewModel();
        this.chartDisplayer = new chartDisplayer_1.ChartDisplayer(chartId, this.model);
        this.tableDisplayer = new tableDisplayer_1.TableDisplayer(tableContainerId, this.model);
    }
    Displayer.prototype.visualize = function (model) {
        this.format(model);
        this.model.push(model);
        this.chartDisplayer.display(model);
        this.tableDisplayer.display(model);
    };
    Displayer.prototype.clean = function () {
        this.model.results = [];
        this.tableDisplayer.clear();
        this.chartDisplayer.clear();
    };
    Displayer.prototype.sortAndDisplay = function () {
        this.model.sortModelExceptFirst();
        this.chartDisplayer.display();
        this.tableDisplayer.display();
    };
    Displayer.prototype.show = function () {
        if (!this.isHidden)
            return;
        this.chartDisplayer.show();
        this.tableDisplayer.show();
        this.isHidden = false;
    };
    Displayer.prototype.hide = function () {
        if (this.isHidden)
            return;
        this.chartDisplayer.hide();
        this.tableDisplayer.hide();
        this.isHidden = true;
    };
    Displayer.prototype.format = function (model) {
        model.mintime = +model.mintime.toFixed(2);
        model.maxtime = +model.maxtime.toFixed(2);
    };
    return Displayer;
}());
exports.Displayer = Displayer;
//# sourceMappingURL=displayer.js.map