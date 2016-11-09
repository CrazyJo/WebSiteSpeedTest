"use strict";
var TableMaker = (function () {
    function TableMaker(tableContainer, maper) {
        this.numberColumn = true;
        this.maper = maper;
        this.tableContainer = tableContainer;
        this.curentColumnNumber = 1;
    }
    TableMaker.prototype.makeHeaderRow = function () {
        var result = "";
        result += "<tr>";
        result += this.makeHeadersByMaper();
        result += "</tr>";
        return result;
    };
    TableMaker.prototype.makeHeadersByMaper = function () {
        var result = this.numberColumn ? "<th>#</th>" : "";
        for (var _i = 0, _a = this.maper.list; _i < _a.length; _i++) {
            var item = _a[_i];
            result += "<th>" + item.headerName + "</th>";
        }
        return result;
    };
    TableMaker.prototype.makeTableRow = function (model) {
        var row = document.createElement("tr");
        row.innerHTML = this.maper ? this.makeTableDataByMaper(model) : this.makeTableDataByDefault(model);
        return row;
    };
    TableMaker.prototype.makeClassAttribute = function (classValue) {
        return "" + (classValue ? "class=" + classValue : '');
    };
    TableMaker.prototype.addRows = function (model) {
        for (var _i = 0, model_1 = model; _i < model_1.length; _i++) {
            var item = model_1[_i];
            this.addRow(item);
        }
    };
    TableMaker.prototype.fillTableFrom = function (model, mode) {
        if (mode === void 0) { mode = InsertionMode.Replace; }
        switch (mode) {
            case InsertionMode.Append:
                this.addRows(model);
                break;
            case InsertionMode.Replace:
                this.tableElement.innerHTML = '';
                this.addHeader();
                this.addRows(model);
                break;
        }
    };
    TableMaker.prototype.addRow = function (model) {
        this.tableElement.children[0].appendChild(this.makeTableRow(model));
    };
    TableMaker.prototype.addHeader = function () {
        this.tableElement.innerHTML = this.makeHeaderRow();
    };
    TableMaker.prototype.createTabelInContainer = function (tableContainer) {
        var container = tableContainer || this.tableContainer;
        if (!container)
            throw new Error("I can not insert a table into nowhere, tableContainer is undefined");
        var table = document.createElement("table");
        if (this.tableClass)
            table.className = this.tableClass;
        this.tableElement = table;
        container.appendChild(table);
    };
    TableMaker.prototype.makeTableDataByDefault = function (model) {
        var result = this.addRowCounter();
        for (var propName in model) {
            result += "<td>" + model[propName] + "</td>";
        }
        return result;
    };
    TableMaker.prototype.makeTableDataByMaper = function (model) {
        var result = this.addRowCounter();
        for (var _i = 0, _a = this.maper.list; _i < _a.length; _i++) {
            var item = _a[_i];
            result += "<td>" + model[item.propertyName] + "</td>";
        }
        return result;
    };
    TableMaker.prototype.addRowCounter = function () {
        return this.numberColumn ? "<td>" + this.curentColumnNumber++ + "</td>" : "";
    };
    return TableMaker;
}());
exports.TableMaker = TableMaker;
(function (InsertionMode) {
    InsertionMode[InsertionMode["Replace"] = 0] = "Replace";
    InsertionMode[InsertionMode["Append"] = 1] = "Append";
})(exports.InsertionMode || (exports.InsertionMode = {}));
var InsertionMode = exports.InsertionMode;
var HeaderPropertyMaper = (function () {
    function HeaderPropertyMaper(headers, properties) {
        this.list = [];
        if (headers && properties)
            this.addMap(headers, properties);
    }
    HeaderPropertyMaper.prototype.addMap = function (headers, properties) {
        if (headers.length !== properties.length)
            throw new Error("Arrays must have the same length");
        for (var i = 0; i < headers.length; i++) {
            this.list.push(new HeaderProperty(headers[i], properties[i]));
        }
    };
    return HeaderPropertyMaper;
}());
exports.HeaderPropertyMaper = HeaderPropertyMaper;
var HeaderProperty = (function () {
    function HeaderProperty(headerName, propertyName) {
        this.headerName = headerName;
        this.propertyName = propertyName;
    }
    return HeaderProperty;
}());
exports.HeaderProperty = HeaderProperty;
//# sourceMappingURL=tableMaker.js.map