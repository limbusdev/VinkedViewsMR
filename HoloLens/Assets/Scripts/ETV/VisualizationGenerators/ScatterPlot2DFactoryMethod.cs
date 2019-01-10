using UnityEngine;


public class ScatterPlot2DFactoryMethod : AETVFactoryMethod
{
    protected override bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals)
    {
        // Only for exactly two attributes
        return (nominals + ordinals + intervals + rationals == 2);
    }

    /// <summary>
    /// Generates a 2D Scatterplot for 2 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the scatterplot.</param>
    /// <returns>GameObject containing the visualization.</returns>
    protected override GameObject GeneratorTemplate(int dataSetID, string[] variables)
    {
        var factory = Services.ETVFactory2D();
        var ds = Services.VisFactory().dataProvider.dataSets[dataSetID];
        var vis = factory.CreateScatterplot(ds, variables).gameObject;

        return vis;
    }
}
