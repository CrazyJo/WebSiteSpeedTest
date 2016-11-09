"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var elementDisplayer_1 = require("./elementDisplayer");
var Table = require("./tableMaker");
var TableDisplayer = (function (_super) {
    __extends(TableDisplayer, _super);
    function TableDisplayer(tableContainerId, model) {
        _super.call(this, tableContainerId, model);
        this.tableMakerInit();
    }
    TableDisplayer.prototype.tableMakerInit = function () {
        var headers = ["Url", "Min (S)", "Max (s)"];
        var props = ["url", "mintime", "maxtime"];
        var maper = new Table.HeaderPropertyMaper(headers, props);
        this.tableMaker = new Table.TableMaker(this.htmlElement, maper);
        this.tableMaker.tableClass = "table table-bordered table-hover";
        this.tableMaker.createTabelInContainer();
    };
    TableDisplayer.prototype.displayFromLocalModel = function () {
        this.tableMaker.curentColumnNumber = 1;
        this.tableMaker.fillTableFrom(this.model.results);
        this.tableMaker.curentColumnNumber = 1;
    };
    TableDisplayer.prototype.displayFromOuterModel = function (model) {
        this.tableMaker.addRow(model);
    };
    TableDisplayer.prototype.clear = function () {
        var tbody = this.tableMaker.tableElement.children[0];
        if (tbody)
            tbody.innerHTML = '';
        this.tableMaker.addHeader();
    };
    return TableDisplayer;
}(elementDisplayer_1.ElementDisplayer));
exports.TableDisplayer = TableDisplayer;
//# sourceMappingURL=tableDisplayer.js.map