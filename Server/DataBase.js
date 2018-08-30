const mongoose = require('mongoose');

class DataBase
{
    constructor()
    {
        mongoose.connect('mongodb://localhost/arvisdb');
        this.mongoose = mongoose;
    }
};

module.exports = DataBase;
