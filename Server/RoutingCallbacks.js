module.exports = class RoutingCallbacks
{
    static defaultPage(request, response)
    {
        response.sendFile(__dirname + '/public/index.html');
    }

    static errorPage(request, response)
    {
        response.send("<html><head /><body><h1>404</h1><p>Error</p></body></html>");
    }
};
