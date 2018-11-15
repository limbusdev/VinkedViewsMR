using UnityEngine;

public class MetaScatterplot2DUpdater : AMetaVisUpdater
{
    protected override MetaVisType Type()
    {
        return MetaVisType.SCATTERPLOT_2D;
    }

    protected override void UpdateNormalVector()
    {
        normal = Vector3.Cross(
                    spanningAxes.A.GetAxisDirectionGlobal(),
                    spanningAxes.B.GetAxisDirectionGlobal());
    }

    protected override void UpdatePosition()
    {
        metaVisualization.transform.position =
                    (spanningAxes.A.GetAxisBaseGlobal()
                    + spanningAxes.B.GetAxisBaseGlobal()) / 2f;
    }

    protected override void UpdateRotation()
    {
        // TODO
    }
}
