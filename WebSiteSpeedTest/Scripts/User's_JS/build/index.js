/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};

/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {

/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId])
/******/ 			return installedModules[moduleId].exports;

/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			exports: {},
/******/ 			id: moduleId,
/******/ 			loaded: false
/******/ 		};

/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);

/******/ 		// Flag the module as loaded
/******/ 		module.loaded = true;

/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}


/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;

/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;

/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";

/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	/// <reference path="../../typings/jquery/jquery.d.ts" />
	"use strict";
	var ajax_1 = __webpack_require__(1);
	var Enums = __webpack_require__(2);
	var initializer_1 = __webpack_require__(3);
	var displayer_1 = __webpack_require__(6);
	var signalR_1 = __webpack_require__(13);
	$(document)
	    .ready(function () {
	    var inputUrlErorrs = $("#inputUrlErorrs");
	    var startBtnWaiter = $("#startTestWaiter");
	    var stBtnDefText = $("#startTestDefaultText");
	    startBtnWaiter.hide();
	    var startBtn = $("#startTestBtn");
	    var testTroviderUrl = startBtn.attr('data-url');
	    var inputUrl = $("#input_url");
	    var modalWaiter = $("#modalWaiter");
	    modalWaiter.hide();
	    var displayer = new displayer_1.Displayer("#chartContainer", "#tableContainer");
	    var notifier = new signalR_1.Notifier(function (m) {
	        displayer.show();
	        displayer.visualize(m);
	    });
	    startBtn
	        .click(function (e) {
	        e.preventDefault();
	        inputUrlErorrs.html("");
	        inputUrl.removeClass("field-error");
	        var value = inputUrl.val();
	        var isValueValid = false;
	        if (/^(ftp|http|https):\/\/[^ "]+$/.test(value)) {
	            isValueValid = true;
	        }
	        else if (/^[a-zA-Z0-9][a-zA-Z0-9-_]{0,61}[a-zA-Z0-9]{0,1}\.([a-zA-Z]{1,6}|[a-zA-Z0-9-]{1,30}\.[a-zA-Z]{2,3})$/.test(value)) {
	            value = "http://" + value;
	            isValueValid = true;
	        }
	        if (isValueValid) {
	            startBtnWaiter.show();
	            stBtnDefText.hide();
	            displayer.clean();
	            $.ajax({
	                type: "POST",
	                url: testTroviderUrl,
	                data: {
	                    url: value,
	                    connectionId: notifier.connectionId
	                },
	                success: function (v) {
	                    if (!v.TestCompletedSuccessfully) {
	                        inputUrlErorrs.html(v.Exception.Message);
	                        startBtnWaiter.hide();
	                        stBtnDefText.show();
	                        inputUrl.addClass("field-error");
	                    }
	                    else {
	                        startBtnWaiter.hide();
	                        stBtnDefText.show();
	                        displayer.sortAndDisplay();
	                    }
	                },
	                error: function () {
	                    startBtnWaiter.hide();
	                    stBtnDefText.show();
	                    inputUrl.addClass("field-error");
	                    displayer.hide();
	                }
	            });
	        }
	        else {
	            inputUrl.addClass("field-error");
	        }
	    });
	    $("#historyBtn")
	        .click(function () {
	        modalWaiter.show();
	        var updateTarget = $(this).attr('data-update-custom');
	        var el = $(updateTarget);
	        $.ajax({
	            type: $(this).attr('data-ajax-method'),
	            url: $(this).attr('data-url'),
	            success: function (e) {
	                modalWaiter.hide();
	                el.html(e);
	                // find pager's btn and set handlers
	                initializer_1.Initializer.pagerInit(updateTarget + " ul.pager a", "#historyTable");
	            }
	        });
	    });
	    var historyConteiner = document.querySelector("#historyContainer");
	    historyConteiner.addEventListener("click", function (arg) {
	        arg.preventDefault();
	        var eventSource = $(arg.target);
	        if (eventSource.is("a") &&
	            eventSource.attr("data-toggle") === "collapse" &&
	            eventSource.attr("data-switch") === "true") {
	            var rowId_1 = eventSource.attr('href');
	            var uri = eventSource.attr('data-url');
	            ajax_1.Ajax.run(Enums.HttpMethod.POST, uri, {
	                historyRowId: rowId_1.slice(1),
	                startIndex: 0
	            }, function (siteMap) {
	                $(rowId_1).html(siteMap);
	                eventSource.attr("data-switch", "false");
	                // find pager's btn and set handlers
	                initializer_1.Initializer.pagerInit(rowId_1 + " ul.pager a", "#sitemapTable");
	            });
	        }
	    });
	});
	

/***/ },
/* 1 */
/***/ function(module, exports, __webpack_require__) {

	/// <reference path="../../../typings/jquery/jquery.d.ts" />
	"use strict";
	var Enums = __webpack_require__(2);
	var Ajax = (function () {
	    function Ajax() {
	    }
	    Ajax.run = function (httpMethodType, url, data, callBackOnDone) {
	        $.ajax({
	            type: Enums.HttpMethod[httpMethodType],
	            url: url,
	            data: data
	        })
	            .done(callBackOnDone);
	    };
	    return Ajax;
	}());
	exports.Ajax = Ajax;
	

/***/ },
/* 2 */
/***/ function(module, exports) {

	"use strict";
	(function (HttpMethod) {
	    HttpMethod[HttpMethod["GET"] = 0] = "GET";
	    HttpMethod[HttpMethod["POST"] = 1] = "POST";
	    HttpMethod[HttpMethod["PUT"] = 2] = "PUT";
	    HttpMethod[HttpMethod["DELETE"] = 3] = "DELETE";
	})(exports.HttpMethod || (exports.HttpMethod = {}));
	var HttpMethod = exports.HttpMethod;
	(function (PagerElementRole) {
	    PagerElementRole[PagerElementRole["First"] = 1] = "First";
	    PagerElementRole[PagerElementRole["Next"] = 2] = "Next";
	    PagerElementRole[PagerElementRole["Previous"] = 3] = "Previous";
	    PagerElementRole[PagerElementRole["Last"] = 4] = "Last";
	})(exports.PagerElementRole || (exports.PagerElementRole = {}));
	var PagerElementRole = exports.PagerElementRole;
	

/***/ },
/* 3 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var handlers_1 = __webpack_require__(4);
	var View = __webpack_require__(5);
	var Initializer = (function () {
	    function Initializer() {
	    }
	    Initializer.pagerInit = function (selector, updateTarget) {
	        // find pager's btn and set handlers
	        var pager;
	        try {
	            pager = new View.Pager(selector);
	        }
	        catch (e) {
	            return;
	        }
	        var pagerAjax = new handlers_1.PagerAjaxHandler(pager, updateTarget);
	        pager.nextBtn.click = function () {
	            pagerAjax.sendAjaxRequest(pager.nextBtn);
	        };
	        pager.previousBtn.click = function () {
	            pagerAjax.sendAjaxRequest(pager.previousBtn);
	        };
	    };
	    return Initializer;
	}());
	exports.Initializer = Initializer;
	

/***/ },
/* 4 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var ajax_1 = __webpack_require__(1);
	var Enums = __webpack_require__(2);
	var PagerAjaxHandler = (function () {
	    function PagerAjaxHandler(pager, updateTarget) {
	        this.pager = pager;
	        this.updateTarget = updateTarget;
	    }
	    PagerAjaxHandler.prototype.sendAjaxRequest = function (pagerBtn) {
	        var _this = this;
	        if (!pagerBtn.isEnabled)
	            return;
	        var ajaxData;
	        if (!pagerBtn.rowId)
	            ajaxData = { startIndex: pagerBtn.startIndex };
	        else
	            ajaxData = { historyRowId: pagerBtn.rowId, startIndex: pagerBtn.startIndex };
	        ajax_1.Ajax.run(Enums.HttpMethod.POST, pagerBtn.url, ajaxData, function (newPage) {
	            $(_this.updateTarget).html(newPage.contentHistory);
	            _this.pagerBtnStyleToggle(newPage);
	        });
	    };
	    PagerAjaxHandler.prototype.pagerBtnStyleToggle = function (model) {
	        this.pager.previousBtn.startIndex = model.historyPager.previousStartIndex;
	        this.pager.previousBtn.isEnabled = !model.historyPager.isFirstPage;
	        this.pager.nextBtn.startIndex = model.historyPager.nextStartIndex;
	        this.pager.nextBtn.isEnabled = !model.historyPager.isLastPage;
	    };
	    return PagerAjaxHandler;
	}());
	exports.PagerAjaxHandler = PagerAjaxHandler;
	

/***/ },
/* 5 */
/***/ function(module, exports, __webpack_require__) {

	/// <reference path="../../../typings/jquery/jquery.d.ts" />
	"use strict";
	var Enums = __webpack_require__(2);
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
	

/***/ },
/* 6 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var Model = __webpack_require__(7);
	var MeasurementsViewModel = Model.MeasurementsViewModel;
	var chartDisplayer_1 = __webpack_require__(8);
	var tableDisplayer_1 = __webpack_require__(11);
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
	

/***/ },
/* 7 */
/***/ function(module, exports) {

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
	

/***/ },
/* 8 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var elementDisplayer_1 = __webpack_require__(9);
	var chart_1 = __webpack_require__(10);
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
	    };
	    ChartDisplayer.prototype.displayFromOuterModel = function (model) {
	        this.updateChart(model);
	    };
	    ChartDisplayer.prototype.clear = function () {
	        this.modelParts = new ResultPack();
	        this.replaceChartData(this.modelParts);
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
	        var w = 975;
	        var h = 450;
	        // hide the char container element
	        container.hide();
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
	

/***/ },
/* 9 */
/***/ function(module, exports) {

	"use strict";
	var ElementDisplayer = (function () {
	    function ElementDisplayer(elementId, model) {
	        this.htmlElement = document.querySelector(elementId);
	        this.model = model;
	    }
	    ElementDisplayer.prototype.display = function (model) {
	        if (model) {
	            this.displayFromOuterModel(model);
	        }
	        else {
	            this.displayFromLocalModel();
	        }
	    };
	    ElementDisplayer.prototype.show = function () {
	        this.htmlElement.style.display = "block";
	    };
	    ElementDisplayer.prototype.hide = function () {
	        this.htmlElement.style.display = "none";
	    };
	    return ElementDisplayer;
	}());
	exports.ElementDisplayer = ElementDisplayer;
	

/***/ },
/* 10 */
/***/ function(module, exports) {

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
	

/***/ },
/* 11 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var elementDisplayer_1 = __webpack_require__(9);
	var Table = __webpack_require__(12);
	var TableDisplayer = (function (_super) {
	    __extends(TableDisplayer, _super);
	    function TableDisplayer(tableContainerId, model) {
	        _super.call(this, tableContainerId, model);
	        this.tableMakerInit();
	    }
	    TableDisplayer.prototype.tableMakerInit = function () {
	        var headers = ["Url", "Min (s)", "Max (s)"];
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
	

/***/ },
/* 12 */
/***/ function(module, exports) {

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
	

/***/ },
/* 13 */
/***/ function(module, exports) {

	"use strict";
	var Notifier = (function () {
	    function Notifier(callback) {
	        var notifier = $.connection.notificationHub;
	        notifier.client.displayMessage = callback;
	        var iAm = this;
	        $.connection.hub.start().done(function () {
	            iAm.connectionId = $.connection.hub.id;
	        });
	    }
	    return Notifier;
	}());
	exports.Notifier = Notifier;
	

/***/ }
/******/ ]);
//# sourceMappingURL=index.js.map