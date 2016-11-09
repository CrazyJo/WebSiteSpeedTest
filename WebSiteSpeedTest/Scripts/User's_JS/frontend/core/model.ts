export class MeasurementsViewModel
{
    results: MeasurementResult[];

    constructor(model?: MeasurementResult[])
    {
        this.results = model;
    }

    sortModelExceptFirst()
    {
        let first = this.results.shift();
        this.sortModel();
        this.results.unshift(first);
    }

    sortModel()
    {
        this.results.sort((a, b) => a.mintime - b.mintime);
    }

    push(value: MeasurementResult)
    {
        this.results.push(value);
    }
}

export class HistoryAjaxData
{
    startIndex: number;

    constructor(startIndex: number)
    {
        this.startIndex = startIndex;
    }
}

export class MeasurementResult {
    url: string;
    mintime: number;
    maxtime: number;
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