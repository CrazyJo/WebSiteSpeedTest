import * as Model from "../core/model";
import {Ajax} from "./ajax"
import * as Enums from "./enums";
import * as View from "./view";

export class PagerAjaxHandler
{
    pager: View.Pager;
    updateTarget: string;

    constructor(pager: View.Pager, updateTarget: string)
    {
        this.pager = pager;
        this.updateTarget = updateTarget;
    }

    sendAjaxRequest(pagerBtn: View.PagerBtn)
    {
        if (!pagerBtn.isEnabled)
            return;

        let ajaxData;
        if (!pagerBtn.rowId)
            ajaxData = { startIndex: pagerBtn.startIndex };
        else
            ajaxData = { historyRowId: pagerBtn.rowId, startIndex: pagerBtn.startIndex}

        Ajax.run(Enums.HttpMethod.POST,
            pagerBtn.url,
            ajaxData,
            (newPage: Model.HistoryPage<string>) =>
            {
                $(this.updateTarget).html(newPage.contentHistory);
                this.pagerBtnStyleToggle(newPage);
            });
    }

    private pagerBtnStyleToggle(model: Model.HistoryPage<string>)
    {
        this.pager.previousBtn.startIndex = model.historyPager.previousStartIndex;
        this.pager.previousBtn.isEnabled = !model.historyPager.isFirstPage;
        this.pager.nextBtn.startIndex = model.historyPager.nextStartIndex;
        this.pager.nextBtn.isEnabled = !model.historyPager.isLastPage;
    }
}