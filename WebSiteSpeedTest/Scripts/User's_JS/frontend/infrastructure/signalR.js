"use strict";
var Notifier = (function () {
    function Notifier(callback) {
        var notifier = $.connection.notificationHub;
        notifier.client.displayMessage = callback;
        var iAm = this;
        $.connection.hub.start().done(function () {
            iAm.connectionId = $.connection.hub.id;
        });
    }
    return Notifier;
}());
exports.Notifier = Notifier;
//# sourceMappingURL=signalR.js.map