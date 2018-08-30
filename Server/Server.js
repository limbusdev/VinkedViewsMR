<!-- ...........................................................................Imports -->
// Include System Modules
var http    = require('http');
var fs      = require('fs');    // FileSystem

// Include Custom Modules
var pageProvider = require('./Modules/ModulePageProvider');

<!-- ...........................................................................Main Code -->

// Set up server
http.createServer(

    function (request, response)
    {
        var callback = function(error, data)
        {
            response.writeHead(200, {'Content-Type': 'text/html'});
            response.write(data);
            response.end()
        };
        pageProvider.createPage("DefaultPage", callback);
    }

).listen(8080);

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
