import * as Model from "../core/model"
import MeasurementsViewModel = Model.MeasurementsViewModel;
import MeasurementResult = Model.MeasurementResult;
import { ChartDisplayer } from "./chartDisplayer"
import { TableDisplayer } from "./tableDisplayer"

export class Displayer
{
    private isHidden = true;
    private chartDisplayer: ChartDisplayer;
    private tableDisplayer: TableDisplayer;
    private model: MeasurementsViewModel;

    constructor(chartId: string, tableContainerId: string)
    {
        this.model = new MeasurementsViewModel();
        this.chartDisplayer = new ChartDisplayer(chartId, this.model);
        this.tableDisplayer = new TableDisplayer(tableContainerId, this.model);
    }

    visualize(model: MeasurementResult)
    {
        this.format(model);
        this.model.push(model);
        this.chartDisplayer.display(model);
        this.tableDisplayer.display(model);
    }

    clean()
    {
        this.model.results = [];
        this.tableDisplayer.clear();
        this.chartDisplayer.clear();
    }

    sortAndDisplay()
    {
        this.model.sortModelExceptFirst();
        this.chartDisplayer.display();
        this.tableDisplayer.display();
    }

    show()
    {
        if (!this.isHidden) return;
        this.chartDisplayer.show();
        this.tableDisplayer.show();
        this.isHidden = false;
    }

    hide()
    {
        if (this.isHidden) return;
        this.chartDisplayer.hide();
        this.tableDisplayer.hide(); 
        this.isHidden = true;
    }

    private format(model: MeasurementResult)
    {
        model.mintime = +model.mintime.toFixed(2);
        model.maxtime = +model.maxtime.toFixed(2);
    }
}