"use strict";
var Model = require("../core/model");
var ajax_1 = require("./ajax");
var Enums = require("./enums");
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
            ajaxData = new Model.HistoryAjaxData(pagerBtn.startIndex);
        else
            ajaxData = { historyRowId: pagerBtn.rowId, startIndex: pagerBtn.startIndex };
        //ajaxData = new Model.SitemapAjaxData(pagerBtn.startIndex, pagerBtn.rowId);
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
//# sourceMappingURL=handlers.js.map