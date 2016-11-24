/// <reference path="../../../typings/jquery/jquery.d.ts" />

import * as Enums from "./enums";

export class PagerBtn
{
    private element: JQuery;

    constructor(element: Element)
    {
        this.element = $(element);
    }

    set click(value: (eventObject: JQueryEventObject) => any)
    {
        this.element.click(value);
    }

    set isEnabled(value: boolean)
    {
        if (value)
            this.element.parent().removeClass("disabled");
        else
            this.element.parent().addClass("disabled");
    }
    get isEnabled(): boolean
    {
        return !this.element.parent().hasClass("disabled");
    }

    get role(): Enums.PagerElementRole
    {

        return Enums.PagerElementRole[this.element.attr("data-role")];
    }

    get startIndex(): number
    {
        return +this.element.attr("data-start-index");
    }
    set startIndex(value: number)
    {
        this.element.attr("data-start-index", value);
    }

    get url(): string
    {
        return this.element.attr("href");
    }

    get rowId():string
    {
        return this.element.attr("data-history-row-id");
    }
}

export class Pager
{
    nextBtn: PagerBtn;
    previousBtn: PagerBtn;
    selector: string;

    constructor(selector: string)
    {
        this.selector = selector;
        this.getPagerElements(selector);
    }

    private getPagerElements(selector: string)
    {
        let elments = $(selector);
        if (elments.length !== 0)
        {
            elments.each((index: number, elem: Element) =>
            {
                switch (Enums.PagerElementRole[$(elem).attr("data-role")])
                {
                    case Enums.PagerElementRole.Next:
                        this.nextBtn = new PagerBtn(elem);
                        break;
                    case Enums.PagerElementRole.Previous:
                        this.previousBtn = new PagerBtn(elem);
                        break;
                }
            });
        }
        else
        {
            throw "selector does not indicate page elements";
        }
    }
}