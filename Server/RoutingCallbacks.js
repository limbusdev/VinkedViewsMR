module.exports = class RoutingCallbacks
{
    static defaultPage(request, response)
    {
        response.sendFile(__dirname + '/Content/index.html');
    }
};
