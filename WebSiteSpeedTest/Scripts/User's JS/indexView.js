(function ()
{
    var wLoader = $("#wait_loader");
    var sectionTabRes = $("#tableResult");

    function ajaxQuery(type, url, data, callBackOnDone)
    {
        $.ajax({
            type: type,
            url: url,
            data: data
        })
            .done(callBackOnDone);
    }

    $("#ajaxComputeLink")
        .click(function ()
        {
            wLoader.show();
            sectionTabRes.show();
            $("#outPutTB").html("<tr><th>Url</th><th>Min (s)</th><th>Max (s)</th></tr>");
            var ajMeth = $(this).attr('data-ajax-method');
            var t_url = $(this).attr('href');
            var input_data = $("#input_url").val();

            $.ajax({
                type: ajMeth,
                url: t_url,
                data: { url: input_data }
            })
                .then(function (e)
                {
                    wLoader.hide();

                    //$("#outPut").html(e);
                });
            return false;
        });

    $("#HistoryBtn")
        .click(function ()
        {
            var updateTarget = $(this).attr('data-update-custom');
            var el = $(updateTarget);

            $.ajax({
                type: $(this).attr('data-ajax-method'),
                url: $(this).attr('data-url')
            })
                .then(function (e)
                {
                    el.html(e);
                });
        });

    // ajax for get sitemapHistory
    var historyConteiner = document.querySelector("#HistoryContainer");
    historyConteiner.addEventListener("click", function (arg)
    {
        var eventSource = $(arg.target);
        if (eventSource.is("a") && eventSource.attr("data-toggle") === "collapse"
            && eventSource.attr("data-switch") === "true")
        {
            var hRowId = eventSource.attr('href');
            var uri = eventSource.attr('data-url');
            ajaxQuery("post",
                uri,
                {
                    historyRowId: hRowId.slice(1),
                    startIndex: 0
                },
                function (e)
                {
                    $(hRowId).html(e);
                    eventSource.attr("data-switch", "false");
                });
        }
    });

    // Ajax - Pager's handlers


})();
