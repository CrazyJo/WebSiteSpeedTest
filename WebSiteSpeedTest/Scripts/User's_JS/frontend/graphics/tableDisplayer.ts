import { ElementDisplayer } from "./elementDisplayer"
import * as Model from "../core/model"
import * as Table from "./tableMaker"
import MeasurementResult = Model.MeasurementResult;
import MeasurementsViewModel = Model.MeasurementsViewModel;

export class TableDisplayer extends ElementDisplayer<MeasurementsViewModel>
{
    private tableMaker: Table.TableMaker;

    constructor(tableContainerId: string, model: MeasurementsViewModel)
    {
        super(tableContainerId, model);
        this.tableMakerInit();
    }

    private tableMakerInit()
    {
        let headers = ["Url", "Min (s)", "Max (s)"];
        let props = ["url", "mintime", "maxtime"];
        let maper = new Table.HeaderPropertyMaper(headers, props);

        this.tableMaker = new Table.TableMaker(this.htmlElement, maper);
        this.tableMaker.tableClass = "table table-bordered table-hover";
        this.tableMaker.createTabelInContainer();
    }

    protected displayFromLocalModel()
    {
        this.tableMaker.curentColumnNumber = 1;
        this.tableMaker.fillTableFrom(this.model.results);
        this.tableMaker.curentColumnNumber = 1;
    }

    protected displayFromOuterModel<T extends MeasurementResult>(model: T)
    {
        this.tableMaker.addRow(model);
    }

    clear()
    {
        let tbody = this.tableMaker.tableElement.children[0];
        if (tbody)
            tbody.innerHTML = '';
        this.tableMaker.addHeader();
    }
}
