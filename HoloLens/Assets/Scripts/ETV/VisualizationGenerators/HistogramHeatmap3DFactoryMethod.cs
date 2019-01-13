using UnityEngine;


public class HistogramHeatmap3DFactoryMethod : AETVFactoryMethod
{
    protected override bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals)
    {
        return (nominals + ordinals == 2 && intervals + rationals == 0);
    }

    /// <summary>
    /// Generates a 2D bar chart for one categorical attribute.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variable">Attribute to generate the histogram from.</param>
    /// <returns>GameObject containing the visualization.</returns>
    protected override GameObject GeneratorTemplate(int dataSetID, string[] variables)
    {
        var factory = Services.ETVFactory3D();
        var ds = Services.DataBase().dataSets[dataSetID];
        var vis = factory.CreateETVBarMap(ds, variables[0], variables[1]).gameObject;

        return vis;
    }
}
