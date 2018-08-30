'use strict';
<!-- ...........................................................................Imports -->
// Include System Modules
const http    = require('http');
const fs      = require('fs');          // FileSystem
const url     = require('url');
const mongoose= require('mongoose');    // DataBase-Acces for MongoDB
const express = require('express');     // Web-App-Framework

// Include Custom Modules
const DataBase = require('./DataBase')
const RoutingCallbacks = require('./RoutingCallbacks');
const RESTresponses = require('./RESTresponses');
var pageProvider = require('./Modules/ModulePageProvider');
const users = require('./Data/users');

<!-- ...........................................................................Main Code -->

class Main
{
    static main()
    {
        // Set up server
        var server = new Server();
    }
}

class Server
{
    constructor()
    {
        this.server = express();
        this.server.set('port', process.env.PORT || 8080);
        this.server.get('/', RoutingCallbacks.defaultPage);
        this.server.get('/about', Server.serverCallbackRequest);
        this.server.get('/users', RESTresponses.getUsers);

        // Error handling via Express middleware
        this.server.use((request, response) =>
            {
                response.type('text/plain');
                response.status('505');
                response.send('Error page');
            }
        );

        // Bind server to a port
        this.server.listen(8080, ()=>console.log('Express server started at port 8080'));

        // Initiate DataBase
        this.db = new DataBase();
    }

    static serverCallbackRequest(request, response)
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
    }

    // Routes

}
// Static Properties
Server.server;
Server.db;

// Run main code
Main.main();
