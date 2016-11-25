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
        this.results.sort((a, b) => b.mintime - a.mintime);
    }

    push(value: MeasurementResult)
    {
        this.results.push(value);
    }
}

export class MeasurementResult {
    url: string;
    mintime: number;
    maxtime: number;
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