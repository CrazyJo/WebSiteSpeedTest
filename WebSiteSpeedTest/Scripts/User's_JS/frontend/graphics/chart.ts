export class Chart
{
    data: any;
    layout: any;
    canvasElement: HTMLElement;

    create()
    {
        Plotly.newPlot(this.canvasElement, this.data, this.layout);
    }

    update()
    {
        Plotly.redraw(this.canvasElement);
    }
}