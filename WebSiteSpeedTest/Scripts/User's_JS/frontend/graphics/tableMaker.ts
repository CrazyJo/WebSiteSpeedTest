export class TableMaker
{
    tableClass: string;
    numberColumn: boolean;
    maper: HeaderPropertyMaper;
    tableContainer: HTMLElement;

    curentColumnNumber: number;
    tableElement: HTMLElement;

    constructor(tableContainer?: HTMLElement, maper?: HeaderPropertyMaper)
    {
        this.numberColumn = true;
        this.maper = maper;
        this.tableContainer = tableContainer;
        this.curentColumnNumber = 1;
    }

    private makeHeaderRow(): string
    {
        let result = "";
        result += `<tr>`;

        result += this.makeHeadersByMaper();

        result += "</tr>";

        return result;
    }

    private makeHeadersByMaper(): string
    {
        let result = this.numberColumn ? "<th>#</th>" : "";

        for (let item of this.maper.list)
        {
            result += `<th>` + item.headerName + "</th>";
        }

        return result;
    }

    private makeTableRow(model): HTMLTableRowElement
    {
        let row: HTMLTableRowElement = document.createElement("tr");
        row.innerHTML = this.maper ? this.makeTableDataByMaper(model) : this.makeTableDataByDefault(model);
        return row;
    }

    private makeClassAttribute(classValue: string): string
    {
        return `${classValue ? `class=${classValue}` : ''}`;
    }

    private addRows(model: Array<Object>)
    {
        for (let item of model)
        {
            this.addRow(item);
        }
    }

    fillTableFrom(model: Array<Object>, mode = InsertionMode.Replace)
    {
        switch (mode)
        {
            case InsertionMode.Append:
                this.addRows(model);
                break;
            case InsertionMode.Replace:
                this.tableElement.innerHTML = '';
                this.addHeader();
                this.addRows(model);
                break;
        }
    }

    addRow(model)
    {
        this.tableElement.children[0].appendChild(this.makeTableRow(model));
    }

    addHeader()
    {
        this.tableElement.innerHTML = this.makeHeaderRow();
    }

    createTabelInContainer(tableContainer?: HTMLElement)
    {
        let container = tableContainer || this.tableContainer;
        if (!container)
            throw new Error("I can not insert a table into nowhere, tableContainer is undefined");

        let table = document.createElement("table");
        if (this.tableClass)
            table.className = this.tableClass;
        this.tableElement = table;
        container.appendChild(table);
    }

    private makeTableDataByDefault(model): string
    {
        let result = this.addRowCounter();
        for (let propName in model)
        {
            result += `<td>${model[propName]}</td>`;
        }
        return result;
    }

    private makeTableDataByMaper(model): string
    {
        let result = this.addRowCounter();
        for (let item of this.maper.list)
        {
            result += `<td>${model[item.propertyName]}</td>`;
        }
        return result;
    }

    private addRowCounter(): string
    {
        return this.numberColumn ? `<td>${this.curentColumnNumber++}</td>` : "";
    }
}

export enum InsertionMode {
    Replace,
    Append
}

export class HeaderPropertyMaper
{
    list: HeaderProperty[];

    constructor(headers?: string[], properties?: string[])
    {
        this.list = [];
        if (headers && properties)
            this.addMap(headers, properties);
    }

    addMap(headers: string[], properties: string[])
    {
        if (headers.length !== properties.length)
            throw new Error("Arrays must have the same length");

        for (let i = 0; i < headers.length; i++)
        {
            this.list.push(new HeaderProperty(headers[i], properties[i]));
        }
    }
}

export class HeaderProperty
{
    constructor(headerName: string, propertyName: string)
    {
        this.headerName = headerName;
        this.propertyName = propertyName;
    }

    headerName: string;
    propertyName: string;
}