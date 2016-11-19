export class Notifier
{
    connectionId;
    constructor(callback)
    {
        let notifier = $.connection.notificationHub;
        notifier.client.displayMessage = callback;
        let iAm = this;
        $.connection.hub.start().done(() =>
        {
            iAm.connectionId = $.connection.hub.id;
        });
    }
}