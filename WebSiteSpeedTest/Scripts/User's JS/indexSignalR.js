$(function ()
{

    var notificationhub = $.connection.notificationHub;

    notificationhub.client.displayMessage = function (message)
    {
        $('#outPutTB').append(createTr(message.url, message.mintime, message.maxtime));
    };

    $.connection.hub.start();

    function createTr(url, min, max)
    {
        return "<tr><td>" + url + "</td><td>" + min + "</td><td>" + max + "</td></tr>";
    }

});