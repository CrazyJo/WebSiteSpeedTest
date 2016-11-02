import { PagerAjaxHandler } from "./handlers";
import * as View from "./view";

export class Initializer {
   
    static pagerInit(selector: string, updateTarget: string)
    {
        // find pager's btn and set handlers
        let pager: View.Pager;
        try
        {
            pager = new View.Pager(selector);
        }
        catch (e)
        {
            return;
        }

        let pagerAjax = new PagerAjaxHandler(pager, updateTarget);
        pager.nextBtn.click = () =>
        {
            pagerAjax.sendAjaxRequest(pager.nextBtn);
        };

        pager.previousBtn.click = () =>
        {
            pagerAjax.sendAjaxRequest(pager.previousBtn);
        };
    }
}