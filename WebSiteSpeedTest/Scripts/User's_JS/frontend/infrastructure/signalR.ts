export class Notifier
{
    constructor(callback)
    {
        let notifier = $.connection.notificationHub;
        notifier.client.displayMessage = callback;
        $.connection.hub.start();
    }
}