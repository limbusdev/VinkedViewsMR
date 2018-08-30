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
    fs.readFile('./Content/index.html', callback);
};
