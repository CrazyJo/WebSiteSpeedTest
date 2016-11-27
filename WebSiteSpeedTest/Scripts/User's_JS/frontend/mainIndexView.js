/// <reference path="../../typings/jquery/jquery.d.ts" />
"use strict";
var ajax_1 = require("./infrastructure/ajax");
var Enums = require("./infrastructure/enums");
var initializer_1 = require("./infrastructure/initializer");
var displayer_1 = require("./graphics/displayer");
var signalR_1 = require("./infrastructure/signalR");
$(document)
    .ready(function () {
    var inputUrlErorrs = $("#inputUrlErorrs");
    var startBtnWaiter = $("#startTestWaiter");
    var stBtnDefText = $("#startTestDefaultText");
    startBtnWaiter.hide();
    var startBtn = $("#startTestBtn");
    var testTroviderUrl = startBtn.attr('data-url');
    var inputUrl = $("#input_url");
    var modalWaiter = $("#modalWaiter");
    modalWaiter.hide();
    var displayer = new displayer_1.Displayer("#chartContainer", "#tableContainer");
    var notifier = new signalR_1.Notifier(function (m) {
        displayer.show();
        displayer.visualize(m);
    });
    startBtn
        .click(function (e) {
        e.preventDefault();
        inputUrlErorrs.html("");
        inputUrl.removeClass("field-error");
        var value = inputUrl.val();
        var isValueValid = false;
        if (/^(ftp|http|https):\/\/[^ "]+$/.test(value)) {
            isValueValid = true;
        }
        else if (/^[a-zA-Z0-9][a-zA-Z0-9-_]{0,61}[a-zA-Z0-9]{0,1}\.([a-zA-Z]{1,6}|[a-zA-Z0-9-]{1,30}\.[a-zA-Z]{2,3})$/.test(value)) {
            value = "http://" + value;
            isValueValid = true;
        }
        if (isValueValid) {
            startBtnWaiter.show();
            stBtnDefText.hide();
            displayer.clean();
            $.ajax({
                type: "POST",
                url: testTroviderUrl,
                data: {
                    url: value,
                    connectionId: notifier.connectionId
                },
                success: function (v) {
                    if (v) {
                        inputUrlErorrs.html(v);
                        startBtnWaiter.hide();
                        stBtnDefText.show();
                        inputUrl.addClass("field-error");
                    }
                    else {
                        startBtnWaiter.hide();
                        stBtnDefText.show();
                        displayer.sortAndDisplay();
                    }
                },
                error: function () {
                    startBtnWaiter.hide();
                    stBtnDefText.show();
                    inputUrl.addClass("field-error");
                    displayer.hide();
                }
            });
        }
        else {
            inputUrl.addClass("field-error");
        }
    });
    $("#historyBtn")
        .click(function () {
        modalWaiter.show();
        var updateTarget = $(this).attr('data-update-custom');
        var el = $(updateTarget);
        $.ajax({
            type: $(this).attr('data-ajax-method'),
            url: $(this).attr('data-url'),
            success: function (e) {
                modalWaiter.hide();
                el.html(e);
                // find pager's btn and set handlers
                initializer_1.Initializer.pagerInit(updateTarget + " ul.pager a", "#historyTable");
            }
        });
    });
    var historyConteiner = document.querySelector("#historyContainer");
    historyConteiner.addEventListener("click", function (arg) {
        arg.preventDefault();
        var eventSource = $(arg.target);
        if (eventSource.is("a") &&
            eventSource.attr("data-toggle") === "collapse" &&
            eventSource.attr("data-switch") === "true") {
            var rowId_1 = eventSource.attr('href');
            var uri = eventSource.attr('data-url');
            ajax_1.Ajax.run(Enums.HttpMethod.POST, uri, {
                historyRowId: rowId_1.slice(1),
                startIndex: 0
            }, function (siteMap) {
                $(rowId_1).html(siteMap);
                eventSource.attr("data-switch", "false");
                // find pager's btn and set handlers
                initializer_1.Initializer.pagerInit(rowId_1 + " ul.pager a", "#sitemapTable");
            });
        }
    });
});
//# sourceMappingURL=mainIndexView.js.map