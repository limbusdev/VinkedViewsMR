using UnityEngine;

public class MetaHeatMapUpdater : AMetaVisUpdater
{
    protected override MetaVisType Type()
    {
        return MetaVisType.HEATMAP;
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
        if(signedAngle > 0)
        {
            var rot = new Quaternion();
            rot.SetLookRotation(spanningAxes.A.GetAxisDirectionGlobal(), normal);
            metaVisualization.transform.rotation = rot;
        } else
        {
            var rot = new Quaternion();
            rot.SetLookRotation(spanningAxes.B.GetAxisDirectionGlobal(), normal);
            metaVisualization.transform.rotation = rot;
        }
    }
}
