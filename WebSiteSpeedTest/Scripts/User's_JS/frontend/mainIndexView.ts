/// <reference path="../../typings/jquery/jquery.d.ts" />

import {Ajax} from "./infrastructure/ajax"
import * as Enums from "./infrastructure/enums";
import { Initializer } from "./infrastructure/initializer"

$(document)
    .ready(() =>
    {
        let wLoader = $("#wait_loader");
        let sectionTabRes = $("#tableResult");

        $("#ajaxComputeLink")
            .click(function()
            {
                wLoader.show();
                sectionTabRes.show();
                $("#outPutTB").html("<tr><th>Url</th><th>Min (s)</th><th>Max (s)</th></tr>");
                let ajMeth = $(this).attr('data-ajax-method');
                let tUrl = $(this).attr('href');
                let inputData = $("#input_url").val();

                $.ajax({
                        type: ajMeth,
                        url: tUrl,
                        data: { url: inputData }
                    })
                    .then(e =>
                    {
                        wLoader.hide();

                        //$("#outPut").html(e);
                    });
                return false;
            });

        $("#historyBtn")
            .click(function()
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

