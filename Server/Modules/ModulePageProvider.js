// Page Provider Module

// ............................................................................. Imports
var fs = require('fs');


// ............................................................................. Main Code

/*
 *  Returns an HTML-page with the provided string.
 *
 *  @param pageName String to embed in the returned page.
 *  @param callback Function of form function(error, data)
 */
exports.createPage = function(pageName, callback)
{
    switch(pageName)
    {
        case '/error':
            callback("", "<html><head /><body><h1>404</h1><p>Error</p></body></html>"); break;
        case '/about':
            fs.readFile('./Content/about.html', callback);
            break;
        default:
            fs.readFile('./Content/index.html', callback);
            break;
    }

};
