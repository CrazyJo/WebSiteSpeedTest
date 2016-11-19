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
        let inputUrlErorrs = $("#inputUrlErorrs");
        let startBtnWaiter = $("#startTestWaiter");
        startBtnWaiter.hide();
        let stBtnDefText = $("#startTestDefaultText");
        let startBtn = $("#startTestBtn");
        let testTrovider = startBtn.attr('data-url');
        let inputUrl = $("#input_url");

        let modalWaiter = $("#modalWaiter");
        modalWaiter.hide();

        let displayer = new Displayer("#chartContainer", "#tableContainer");

        let notifier = new Notifier((m) =>
        {
            displayer.show();
            displayer.visualize(m);
        });

        startBtn
            .click((e) =>
            {
                e.preventDefault();
                let value = inputUrl.val();
                let isValueValid = value.match(/^(ftp|http|https):\/\/[^ "]+$/);

                if (isValueValid)
                {
                    inputUrl.removeClass("field-error");
                    startBtnWaiter.show();
                    stBtnDefText.hide();
                    inputUrlErorrs.html("");

                    displayer.clean();

                    $.ajax({
                        type: "POST",
                        url: testTrovider,
                        data: {
                            url: value,
                            connectionId: notifier.connectionId
                        },
                        success: (e) =>
                        {
                            if (e)
                            {
                                inputUrlErorrs.html(e);
                                startBtnWaiter.hide();
                                stBtnDefText.show();
                                inputUrl.addClass("field-error");
                            }
                            else
                            {
                                startBtnWaiter.hide();
                                stBtnDefText.show();

                                displayer.sortAndDisplay();
                            }
                        },
                        error: (e) =>
                        {
                            startBtnWaiter.hide();
                            stBtnDefText.show();
                            inputUrl.addClass("field-error");

                            displayer.hide();
                        }
                    });
                }
                else
                {
                    inputUrl.addClass("field-error");
                }
            });

        $("#historyBtn")
            .click(function ()
            {
                modalWaiter.show();
                let updateTarget: string = $(this).attr('data-update-custom');
                let el = $(updateTarget);
                $.ajax({
                    type: $(this).attr('data-ajax-method'),
                    url: $(this).attr('data-url'),
                    success: e =>
                    {
                        modalWaiter.hide();
                        el.html(e);

                        // find pager's btn and set handlers
                        Initializer.pagerInit(updateTarget + " ul.pager a", "#historyTable");
                    }
                });
            });

        let historyConteiner = document.querySelector("#historyContainer");
        historyConteiner.addEventListener("click",
            (arg: MouseEvent) =>
            {
                arg.preventDefault();
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
            });
    });

