export abstract class ElementDisplayer<TData>
{
    protected htmlElement: HTMLElement;
    protected model: TData;

    constructor(elementId: string, model: TData)
    {
        this.htmlElement = <HTMLElement>document.querySelector(elementId);
        this.model = model;
    }

    display<T>(model?: T)
    {
        if (model)
        {
            this.displayFromOuterModel(model);
        }
        else
        {
            this.displayFromLocalModel();
        }
    }

    show()
    {
        this.htmlElement.style.display = "block";
    }

    hide()
    {
        this.htmlElement.style.display = "none";
    }

    protected abstract displayFromLocalModel();
    protected abstract displayFromOuterModel<T>(model: T);
}