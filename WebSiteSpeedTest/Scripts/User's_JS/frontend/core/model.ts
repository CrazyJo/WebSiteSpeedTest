export class HistoryAjaxData
{
    startIndex: number;

    constructor(startIndex: number)
    {
        this.startIndex = startIndex;
    }
}

export class SitemapAjaxData extends HistoryAjaxData
{
    historyRowId: string;

    constructor(startIndex: number, rowId: string)
    {
        super(startIndex);
        this.historyRowId = rowId;
    }
}

export class HistoryPage<T> {
    contentHistory: T;
    historyPager: HistoryPager;
}

export class HistoryPager
{
    isLastPage: boolean;
    isFirstPage: boolean;
    previousStartIndex: number;
    nextStartIndex: number;
}