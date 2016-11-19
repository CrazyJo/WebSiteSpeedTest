import { ElementDisplayer } from "./elementDisplayer"
import { Chart } from "./chart"
import * as Model from "../core/model"
import MeasurementResult = Model.MeasurementResult;
import MeasurementsViewModel = Model.MeasurementsViewModel;

export class ChartDisplayer extends ElementDisplayer<MeasurementsViewModel>
{
    private chart: Chart;
    private modelParts: ResultPack;

    constructor(divContainerId: string, model: MeasurementsViewModel)
    {
        super(divContainerId, model);
        this.chartInit(this.htmlElement);
        this.dataInit();
        this.chart.create();
    }

    protected displayFromLocalModel()
    {
        this.replaceChartData(this.splitModel());
        this.chart.update();
    }

    protected displayFromOuterModel<T extends MeasurementResult>(model: T)
    {
        this.updateChart(model);
    }

    clear()
    {
        this.modelParts = new ResultPack();
        this.replaceChartData(this.modelParts);
    }

    private updateModelParts(value: MeasurementResult)
    {
        this.modelParts.urls.push(value.url);
        this.modelParts.minValues.push(value.mintime);
        this.modelParts.maxValues.push(value.maxtime);
    }

    private updateChart(value: MeasurementResult)
    {
        this.updateModelParts(value);
        this.chart.update();
    }

    private replaceChartData(modelAsArray: ResultPack)
    {
        this.chart.data[0].x = modelAsArray.urls;
        this.chart.data[1].x = modelAsArray.urls;
        this.chart.data[0].y = modelAsArray.maxValues;
        this.chart.data[1].y = modelAsArray.minValues;
    }

    private splitModel(): ResultPack
    {
        let res: ResultPack = new ResultPack();
        for (let item of this.model.results)
        {
            res.urls.push(item.url);
            res.minValues.push(item.mintime);
            res.maxValues.push(item.maxtime);
        }
        return res;
    }

    private dataInit()
    {
        this.modelParts = new ResultPack();

        let trace1 = {
            x: this.modelParts.urls,
            y: this.modelParts.maxValues,
            name: 'Max',
            type: 'bar',
            marker: { color: 'rgb(55, 83, 109)' }
        };
        let trace2 = {
            x: this.modelParts.urls,
            y: this.modelParts.minValues,
            name: 'Min',
            type: 'bar',
            marker: { color: 'rgb(26, 118, 255)' }
        };
        let data = [trace1, trace2];
        this.chart.data = data;
    }

    private chartInit(divId: HTMLElement)
    {
        let container = $(divId);
        let w = 975;
        let h = 450;
        // hide the char container element
        container.hide();

        this.chart = new Chart();
        let layout = {
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
    }
}

class ResultPack
{
    constructor()
    {
        this.urls = [];
        this.minValues = [];
        this.maxValues = [];
    }

    urls: string[];
    minValues: number[];
    maxValues: number[];
}