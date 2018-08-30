'use strict';
<!-- ...........................................................................Imports -->
// Include System Modules
const http    = require('http');
const fs      = require('fs');      // FileSystem
const url     = require('url');
const mongoDB = require('mongodb'); // NoSQL DataBase MongoDB

// Include Custom Modules
const DataBase = require('./DataBase')
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

// Initiate DataBase
var DB = new DataBase();
