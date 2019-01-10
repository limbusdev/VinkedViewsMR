using UnityEngine;


public class LineChart2DFactoryMethod : AETVFactoryMethod
{
    protected override bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals)
    {
        // Only for two numerical attributes
        return (nominals + ordinals == 0 && intervals + rationals == 2);
    }

    /// <summary>
    /// Generates a 2D line plot for 2 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the line plot.</param>
    /// <returns>GameObject containing the visualization.</returns>
    protected override GameObject GeneratorTemplate(int dataSetID, string[] variables)
    {
        var factory = Services.ETVFactory2D();
        var ds = Services.VisFactory().dataProvider.dataSets[dataSetID];
        var vis = factory.CreateLineChart(ds, variables[0], variables[1]).gameObject;

        return vis;
    }
}
