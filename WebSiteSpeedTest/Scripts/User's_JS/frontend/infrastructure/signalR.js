"use strict";
var Notifier = (function () {
    function Notifier(callback) {
        var notifier = $.connection.notificationHub;
        notifier.client.displayMessage = callback;
        $.connection.hub.start();
    }
    return Notifier;
}());
exports.Notifier = Notifier;
//# sourceMappingURL=signalR.js.map