using UnityEngine;


public class PCP3DFactoryMethod : AETVFactoryMethod
{
    protected override bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals)
    {
        return (nominals + ordinals + intervals + rationals > 1);
    }

    /// <summary>
    /// Generates a 3D Parallel Coordinates Plot for n attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the PCP.</param>
    /// <returns>GameObject containing the visualization.</returns>
    protected override GameObject GeneratorTemplate(int dataSetID, string[] variables)
    {
        var factory = Services.ETVFactory3D();
        var ds = Services.DataBase().dataSets[dataSetID];
        var vis = factory.CreatePCP(ds, variables).gameObject;

        return vis;
    }
}
