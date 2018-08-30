'use strict'

const users = require('./Data/users');

module.exports = class RESTresponses
{
    static getUsers(request, response)
    {
        response.json(users);
    }
}
