"use strict";
var Chart = (function () {
    function Chart() {
    }
    Chart.prototype.create = function () {
        Plotly.newPlot(this.canvasElement, this.data, this.layout);
    };
    Chart.prototype.update = function () {
        Plotly.redraw(this.canvasElement);
    };
    return Chart;
}());
exports.Chart = Chart;
//# sourceMappingURL=chart.js.map