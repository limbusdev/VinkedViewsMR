<!-- ...........................................................................Imports -->
// Include System Modules
const http    = require('http');
const fs      = require('fs');      // FileSystem
const url     = require('url');
const mongoDB = require('mongodb'); // NoSQL DataBase MongoDB

// Include Custom Modules
var pageProvider = require('./Modules/ModulePageProvider');

<!-- ...........................................................................Main Code -->

// Set up server
const server = http.createServer();

var serverCallbackRequest = (request, response) =>
{
    var callbackOK = function(error, data)
    {
        response.writeHead(200, {'Content-Type': 'text/html'});
        response.write(data);
        response.end()
    };

    var callbackERROR = function(error, data)
    {
        response.writeHead(404, {'Content-Type': 'text/html'});
        response.write(data);
        response.end()
    };

    // Routing
    let path = url.parse(request.url).pathname;
    switch(path)
    {
        case '/':
            pageProvider.createPage("/", callbackOK);
            break;
        case '/about':
            pageProvider.createPage("/about", callbackOK);
            break;
        default:
            pageProvider.createPage("/error", callbackERROR);
            break;
    }
};

// Populate Server Events
server.on('request', serverCallbackRequest);
server.listen(8080, ()=>console.log("Node.js server created at port 8080"));

/*

# Notes

* Query string: request.url contains trailing address --> for localhost:8080/test it contains /test
* Parsing the Query string: url.parse(request.url, true).query returns the query-Object, containing all included variables

**Example:**

For 'http://localhost:8080/?year=2017&month=July'

´´´JavaScript
var q = url.parse(request.url, true).query;
var txt = q.year + " " + q.month;
´´´

*/
