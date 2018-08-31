const mongoose = require('mongoose');

class DataBase
{
    constructor()
    {
        this.schemas = new Map();
        this.models = new Map();
        this.setUpSchemas();
        mongoose.connect('mongodb://localhost/arvisdb');

        // Error Handling
        this.db = mongoose.connection;
        this.db.on('error', console.error.bind(console, 'connection error:'));
        this.db.once('open', () => this.onDBOpen() );
    }

    doNextStep()
    {
        var BarChart = this.models.get('BarChart');
    }

    onDBOpen()
    {
        console.log(this.models.get('BarChart'));
        console.log("DataBase is open and connected.");

        // Create object from model
        var BarChart = this.models.get('BarChart');
        var barChart1           = new BarChart();
        barChart1.title         = 'Test-BarChart';
        barChart1.description   = 'This is only a test';
        barChart1.xVariable     = 'City';
        barChart1.xUnit         = '';
        barChart1.yVariable     = 'Population';
        barChart1.yUnit         = 'thousand People';
        barChart1.categories.push('Cat A');
        barChart1.categories.push('Cat B');
        barChart1.categories.push('Cat C');
        barChart1.values.push(20);
        barChart1.values.push(40);
        barChart1.values.push(60);

        // Save object to database
        barChart1.save(function(error, barChart1) {
            if(error){ return console.error(error); }
        });

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

    /**
     *  Function to set up all neccessary schemas.
     */
    setUpSchemas()
    {
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
        var modelBarChart = mongoose.model('BarChart', schemaBarChart);

        this.schemas.set('BarChart', schemaBarChart);
        this.models.set('BarChart', modelBarChart);
    }
};

module.exports = DataBase;
