class Main
{
    static main()
    {
        var width = 420,
            barHeight = 20;

        var x = d3.scaleLinear()
            .range([0, width]);

        var chart = d3.select(".chart")
            .attr("width", width);

        d3.csv("data/exampleData.csv", Main.type, function(error,data)
        {
            console.log(data[0]);
            x.domain([0, d3.max(data, function(d) { return d.value; })]);

            chart.attr("height", barHeight * data.length);

            var bar = chart.selectAll("g")
                .data(data)
            .enter().append("g")
                .attr("transform", function(d, i) { return "translate(0," + i * barHeight + ")"; });

            bar.append("rect")
                .attr("width", function(d) { return x(d.value); })
                .attr("height", barHeight - 1);

            bar.append("text")
                .attr("x", function(d) { return x(d.value) - 3; })
                .attr("y", barHeight / 2)
                .attr("dy", ".35em")
                .text(function(d) { return d.value; });
        });
    }

    static type(d)
    {
        d.value = +d.value;
        return d;
    }
}

// Static Properties
//Main.data = [30, 86, 168, 281, 303, 365];
