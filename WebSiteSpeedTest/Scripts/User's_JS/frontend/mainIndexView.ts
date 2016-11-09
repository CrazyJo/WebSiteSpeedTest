/// <reference path="../../typings/jquery/jquery.d.ts" />

import { Ajax } from "./infrastructure/ajax"
import * as Enums from "./infrastructure/enums";
import { Initializer } from "./infrastructure/initializer"
import * as Model from "./core/model"
import { Displayer } from "./graphics/displayer"
import { Notifier } from "./infrastructure/signalR"

$(document)
    .ready(() =>
    {
        debugger;
        let wLoader = $("#wait_loader");

        let disolayer = new Displayer("#chartContainer", "#tableContainer");

        let notifier = new Notifier((m) =>
        {
            disolayer.visualize(m);
        });

        $("#ajaxComputeLink")
            .click(function (event)
            {
                event.preventDefault();
                wLoader.show();
                disolayer.clean();

                let ajMeth = $(this).attr('data-ajax-method');
                let tUrl = $(this).attr('href');
                let inputData = $("#input_url").val();

                $.ajax({
                    type: ajMeth,
                    url: tUrl,
                    data: { url: inputData }
                })
                    .then((e: Array<Model.MeasurementResult>) =>
                    {
                        wLoader.hide();
                        disolayer.sortAndDisplay();
                    });

                disolayer.show();
            });

        $("#historyBtn")
            .click(function ()
            {
                let updateTarget: string = $(this).attr('data-update-custom');
                let el = $(updateTarget);
                $.ajax({
                    type: $(this).attr('data-ajax-method'),
                    url: $(this).attr('data-url')
                })
                    .then(e =>
                    {
                        el.html(e);

                        // find pager's btn and set handlers
                        Initializer.pagerInit(updateTarget + " ul.pager a", "#historyTable");
                    });
            });

        let historyConteiner = document.querySelector("#historyContainer");
        historyConteiner.addEventListener("click",
            (arg: MouseEvent) =>
            {
                let eventSource = $(arg.target);

                if (eventSource.is("a") &&
                    eventSource.attr("data-toggle") === "collapse" &&
                    eventSource.attr("data-switch") === "true")
                {
                    let rowId = eventSource.attr('href');
                    let uri = eventSource.attr('data-url');
                    Ajax.run(Enums.HttpMethod.POST,
                        uri,
                        {
                            historyRowId: rowId.slice(1),
                            startIndex: 0
                        },
                        (siteMap: string) =>
                        {
                            $(rowId).html(siteMap);

                            eventSource.attr("data-switch", "false");

                            // find pager's btn and set handlers
                            Initializer.pagerInit(rowId + " ul.pager a", "#sitemapTable");
                        });
                }
                arg.preventDefault();
            });
    });

