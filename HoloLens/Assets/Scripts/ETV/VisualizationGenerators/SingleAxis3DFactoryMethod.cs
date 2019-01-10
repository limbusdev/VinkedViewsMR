using UnityEngine;

public class SingleAxis3DFactoryMethod : AETVFactoryMethod
{
    protected override bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals)
    {
        // Only for a single attribute
        return (nominals + ordinals + intervals + rationals == 1);
    }

    protected override GameObject GeneratorTemplate(int dataSetID, string[] variables)
    {
        var factory = Services.ETVFactory3D();
        var ds = Services.VisFactory().dataProvider.dataSets[dataSetID];
        var vis = factory.CreateSingleAxis(ds, variables[0]).gameObject;

        return vis;
    }
}
