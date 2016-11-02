/// <reference path="../../typings/jquery/jquery.d.ts" />
"use strict";
var ajax_1 = require("./infrastructure/ajax");
var Enums = require("./infrastructure/enums");
var initializer_1 = require("./infrastructure/initializer");
$(document)
    .ready(function () {
    var wLoader = $("#wait_loader");
    var sectionTabRes = $("#tableResult");
    $("#ajaxComputeLink")
        .click(function () {
        wLoader.show();
        sectionTabRes.show();
        $("#outPutTB").html("<tr><th>Url</th><th>Min (s)</th><th>Max (s)</th></tr>");
        var ajMeth = $(this).attr('data-ajax-method');
        var tUrl = $(this).attr('href');
        var inputData = $("#input_url").val();
        $.ajax({
            type: ajMeth,
            url: tUrl,
            data: { url: inputData }
        })
            .then(function (e) {
            wLoader.hide();
            //$("#outPut").html(e);
        });
        return false;
    });
    $("#historyBtn")
        .click(function () {
        var updateTarget = $(this).attr('data-update-custom');
        var el = $(updateTarget);
        $.ajax({
            type: $(this).attr('data-ajax-method'),
            url: $(this).attr('data-url')
        })
            .then(function (e) {
            el.html(e);
            // find pager's btn and set handlers
            initializer_1.Initializer.pagerInit(updateTarget + " ul.pager a", "#historyTable");
        });
    });
    var historyConteiner = document.querySelector("#historyContainer");
    historyConteiner.addEventListener("click", function (arg) {
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
        arg.preventDefault();
    });
});
//# sourceMappingURL=mainIndexView.js.map