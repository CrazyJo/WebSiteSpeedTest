$(function ()
{

    var notificationhub = $.connection.loggerHub;

    notificationhub.client.displayMessage = function (message)
    {
        $('#outPutTB').append(createTr(message.url, message.time));
    };

    $.connection.hub.start();

    function createTr(url, time)
    {
        return "<tr><td>" + url + "</td><td>" + time + "</td></tr>";
    }

});