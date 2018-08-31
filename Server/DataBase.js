
class DataBase
{
    constructor()
    {
        this.mongoose = require('mongoose');
        this.mongoose.connect('mongodb://localhost/arvisdb');

        // Error Handling
        this.db = this.mongoose.connection;
        this.db.on('error', console.error.bind(console, 'connection error:'));
        this.db.once('open', this.onDBOpen);
    }

    onDBOpen()
    {
        console.log("DataBase is open and connected.");

        // Setting up schemas. Example at https://mongoosejs.com/docs/schematypes.html
        var schemaBarChart = new mongoose.Schema(
            {
            title:          String,
            description:    String,
            categories:     [String],
            values:         [Number],
            xVariable:      String,
            xUnit:          String,
            yVariable:      String,
            yUnit:          String
        });
        // Model class
        var BarChart = mongoose.model('BarChart', schemaBarChart);

        // Create object from model
        var barChart1 = new BarChart(
        {
            title:          'Test-BarChart',
            description:    'This is only a test',
            categories:     {'Cat A', 'Cat B', 'Cat C'},
            values:         {20, 40, 60},
            xVariable:      'City',
            xUnit:          '',
            yVariable:      'Population',
            yUnit:          'thousand People'
        });

        // Save object to database
        barChart1.save(function(error, barChart1)) {
            if(error){ return console.error(error); }
        };

        // Display all created BarCharts
        BarChart.find(function(error, barCharts) {
            if(error)   {return console.error(error);}
            else        {console.log(barCharts);}
        });

        // Find a Bar Chart by name in the DataBase
        BarChart.find({title: /^Test/}, function(error, foundBarCharts) {
            if(error)   {return console.error(error);}
            else        {console.log(foundBarCharts);}
        });
    }
};

module.exports = DataBase;
