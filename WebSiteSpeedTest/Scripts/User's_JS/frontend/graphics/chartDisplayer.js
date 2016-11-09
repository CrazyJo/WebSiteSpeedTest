"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var elementDisplayer_1 = require("./elementDisplayer");
var chart_1 = require("./chart");
var ChartDisplayer = (function (_super) {
    __extends(ChartDisplayer, _super);
    function ChartDisplayer(divContainerId, model) {
        _super.call(this, divContainerId, model);
        this.chartInit(this.htmlElement);
        this.dataInit();
        this.chart.create();
    }
    ChartDisplayer.prototype.displayFromLocalModel = function () {
        this.replaceChartData(this.splitModel());
        this.chart.update();
        this.modelParts = new ResultPack();
        this.replaceChartData(this.modelParts);
    };
    ChartDisplayer.prototype.displayFromOuterModel = function (model) {
        this.updateChart(model);
    };
    ChartDisplayer.prototype.initAndDisplay = function (model) {
        this.updateModelParts(model);
        this.show();
        this.chart.create();
    };
    ChartDisplayer.prototype.updateModelParts = function (value) {
        this.modelParts.urls.push(value.url);
        this.modelParts.minValues.push(value.mintime);
        this.modelParts.maxValues.push(value.maxtime);
    };
    ChartDisplayer.prototype.updateChart = function (value) {
        this.updateModelParts(value);
        this.chart.update();
    };
    ChartDisplayer.prototype.replaceChartData = function (modelAsArray) {
        this.chart.data[0].x = modelAsArray.urls;
        this.chart.data[1].x = modelAsArray.urls;
        this.chart.data[0].y = modelAsArray.maxValues;
        this.chart.data[1].y = modelAsArray.minValues;
    };
    ChartDisplayer.prototype.splitModel = function () {
        var res = new ResultPack();
        for (var _i = 0, _a = this.model.results; _i < _a.length; _i++) {
            var item = _a[_i];
            res.urls.push(item.url);
            res.minValues.push(item.mintime);
            res.maxValues.push(item.maxtime);
        }
        return res;
    };
    ChartDisplayer.prototype.dataInit = function () {
        this.modelParts = new ResultPack();
        var trace1 = {
            x: this.modelParts.urls,
            y: this.modelParts.maxValues,
            name: 'Max',
            type: 'bar',
            marker: { color: 'rgb(55, 83, 109)' }
        };
        var trace2 = {
            x: this.modelParts.urls,
            y: this.modelParts.minValues,
            name: 'Min',
            type: 'bar',
            marker: { color: 'rgb(26, 118, 255)' }
        };
        var data = [trace1, trace2];
        this.chart.data = data;
    };
    ChartDisplayer.prototype.chartInit = function (divId) {
        var container = $(divId);
        var w = container.width();
        var h = container.height();
        // hide the char container element
        this.hide();
        this.chart = new chart_1.Chart();
        var layout = {
            font: {
                family: "Segoe UI, Times New Roman, Open Sans, verdana, arial, sans-serif",
                color: '#444'
            },
            title: 'Load Time Results',
            barmode: 'overlay',
            autosize: true,
            width: w,
            height: h,
            xaxis: {
                title: 'Urls',
                showticklabels: false,
                autorange: true
            },
            yaxis: {
                title: 'Time (s)',
                autorange: true,
                titlefont: {
                    size: 16,
                    color: 'rgb(107, 107, 107)'
                },
                tickfont: {
                    size: 14,
                    color: 'rgb(107, 107, 107)'
                }
            },
            legend: {
                x: 0,
                y: 1.0,
                bgcolor: 'rgba(255, 255, 255, 0)',
                bordercolor: 'rgba(255, 255, 255, 0)'
            }
        };
        this.chart.layout = layout;
        this.chart.canvasElement = divId;
    };
    return ChartDisplayer;
}(elementDisplayer_1.ElementDisplayer));
exports.ChartDisplayer = ChartDisplayer;
var ResultPack = (function () {
    function ResultPack() {
        this.urls = [];
        this.minValues = [];
        this.maxValues = [];
    }
    return ResultPack;
}());
//# sourceMappingURL=chartDisplayer.js.map