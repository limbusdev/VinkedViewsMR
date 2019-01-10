using UnityEngine;


public class Histogram3DFactoryMethod : AETVFactoryMethod
{
    protected override bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals)
    {
        // Only for a single categorical attribute
        return (nominals + ordinals == 1 && intervals + rationals == 0);
    }

    /// <summary>
    /// Generates a 3D bar map for two categorical attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to generate the histogram from.</param>
    /// <returns>GameObject containing the visualization.</returns>
    protected override GameObject GeneratorTemplate(int dataSetID, string[] variables)
    {
        var factory = Services.ETVFactory3D();
        var ds = Services.VisFactory().dataProvider.dataSets[dataSetID];
        var vis = factory.CreateBarChart(ds, variables[0]).gameObject;

        return vis;
    }
}
