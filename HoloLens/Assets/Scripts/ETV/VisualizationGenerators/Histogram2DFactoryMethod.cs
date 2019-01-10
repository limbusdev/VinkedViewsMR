using UnityEngine;


public class Histogram2DFactoryMethod : AETVFactoryMethod
{
    protected override bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals)
    {
        // Only for a single categorical attribute
        return (nominals + ordinals == 1 && intervals + rationals == 0);
    }

    /// <summary>
    /// Generates a 2D bar chart for one categorical attribute.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variable">Attribute to generate the histogram from.</param>
    /// <returns>GameObject containing the visualization.</returns>
    protected override GameObject GeneratorTemplate(int dataSetID, string[] variables)
    {
        var factory = Services.ETVFactory2D();
        var ds = Services.VisFactory().dataProvider.dataSets[dataSetID];
        var vis = factory.CreateBarChart(ds, variables[0]).gameObject;

        return vis;
    }
}
