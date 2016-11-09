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
//class ChartDisplayer2
//{
//    chart: Chart;
//    private modelParts: ResultPack;
//    private state: ChartMode;
//    private model: MeasurementsViewModel;
//    constructor(div: HTMLElement, model: MeasurementsViewModel)
//    {
//        this.state = ChartMode.Initialize;
//        this.model = model;
//        this.chartInit(div);
//        this.dataInit();
//    }
//    display(model?: MeasurementResult)
//    {
//        switch (this.state)
//        {
//            case ChartMode.Initialize:
//                this.initAndDisplay(model);
//                this.state = ChartMode.RealTimeUpdate;
//                break;
//            case ChartMode.RealTimeUpdate:
//                this.updateChart(model);
//                break;
//            case ChartMode.SortAndDisplay:
//                this.replaceChartData();
//                this.chart.update();
//                this.state = ChartMode.RealTimeUpdate;
//                break;
//        }
//    }
//    sortAdnDisplay()
//    {
//        this.state = ChartMode.SortAndDisplay;
//        this.display();
//    }
//    private updateModelParts(value: MeasurementResult)
//    {
//        this.modelParts.urls.push(value.url);
//        this.modelParts.minValues.push(value.mintime);
//        this.modelParts.maxValues.push(value.maxtime);
//    }
//    private initAndDisplay(value: MeasurementResult)
//    {
//        this.updateModelParts(value);
//        //this.chart.show();
//        this.chart.create();
//    }
//    private updateChart(value: MeasurementResult)
//    {
//        this.updateModelParts(value);
//        this.chart.update();
//    }
//    private replaceChartData()
//    {
//        let tempModelAsArray = this.splitModel();
//        this.chart.data[0].x = tempModelAsArray.urls;
//        this.chart.data[1].x = tempModelAsArray.urls;
//        this.chart.data[0].y = tempModelAsArray.maxValues;
//        this.chart.data[1].y = tempModelAsArray.minValues;
//    }
//    private splitModel(): ResultPack
//    {
//        let res: ResultPack = new ResultPack();
//        for (let item of this.model.results)
//        {
//            res.urls.push(item.url);
//            res.minValues.push(item.mintime);
//            res.maxValues.push(item.maxtime);
//        }
//        return res;
//    }
//    private dataInit()
//    {
//        this.modelParts = new ResultPack();
//        let trace1 = {
//            x: this.modelParts.urls,
//            y: this.modelParts.maxValues,
//            name: 'Max',
//            type: 'bar',
//            marker: { color: 'rgb(55, 83, 109)' }
//        };
//        let trace2 = {
//            x: this.modelParts.urls,
//            y: this.modelParts.minValues,
//            name: 'Min',
//            type: 'bar',
//            marker: { color: 'rgb(26, 118, 255)' }
//        };
//        let data = [trace1, trace2];
//        this.chart.data = data;
//    }
//    private chartInit(divId: HTMLElement)
//    {
//        this.chart = new Chart();
//        let layout = {
//            font: {
//                family: "Segoe UI, Times New Roman, Open Sans, verdana, arial, sans-serif",
//                color: '#444'
//            },
//            title: 'Load Time Results',
//            barmode: 'overlay',
//            autosize: true,
//            xaxis: {
//                title: 'Urls',
//                showticklabels: false,
//                autorange: true
//            },
//            yaxis: {
//                title: 'Time (s)',
//                autorange: true,
//                titlefont: {
//                    size: 16,
//                    color: 'rgb(107, 107, 107)'
//                },
//                tickfont: {
//                    size: 14,
//                    color: 'rgb(107, 107, 107)'
//                }
//            },
//            legend: {
//                x: 0,
//                y: 1.0,
//                bgcolor: 'rgba(255, 255, 255, 0)',
//                bordercolor: 'rgba(255, 255, 255, 0)'
//            }
//        };
//        this.chart.layout = layout;
//        this.chart.canvasElement = divId;
//    }
//}
//enum ChartMode
//{
//    Initialize,
//    RealTimeUpdate,
//    SortAndDisplay
//} 
//# sourceMappingURL=displayer.js.map