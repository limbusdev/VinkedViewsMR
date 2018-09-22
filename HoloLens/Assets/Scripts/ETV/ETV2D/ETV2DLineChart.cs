using ETV;
using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DLineChart : AETV2D
{
    private XYLine2D primitive;
    private string attributeA, attributeB;
    private LoM lomA, lomB;

    
    public void Init(DataSet data, string attributeA, string attributeB)
    {
        base.Init(data);
        this.attributeA = attributeA;
        this.attributeB = attributeB;
        this.lomA = data.GetTypeOf(attributeA);
        this.lomB = data.GetTypeOf(attributeB);

        SetUpAxis();
        DrawGraph();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        switch(scheme)
        {
            default:
                Color color = Color.HSVToRGB(.5f, 1, 1);
                primitive.SetColor(color);
                primitive.ApplyColor(color);
                break;
        }
    }

    public override void UpdateETV()
    {

    }

    public override void SetUpAxis()
    {
        AddAxis(attributeA, lomA, AxisDirection.X);
        AddAxis(attributeB, lomB, AxisDirection.Y);
    }

    public override void DrawGraph()
    {
        var line = ServiceLocator.instance.Factory2DPrimitives.CreateXYLine();
        var xyLineComp = line.GetComponent<XYLine2D>();
        Vector3[] polyline;
        
        xyLineComp.lineRenderer.startWidth = 0.02f;
        xyLineComp.lineRenderer.endWidth = 0.02f;
        
        var points = new List<Vector3>();

        for(int i = 0; i < data.infoObjects.Count; i++)
        {
            InfoObject o = data.infoObjects[i];

            bool valAMissing = data.IsValueMissing(o, attributeA, lomA);
            bool valBMissing = data.IsValueMissing(o, attributeB, lomB);

            if(!(valAMissing || valBMissing))
            {
                var valA = data.GetValue(o, attributeA, lomA);
                var x = GetAxis(AxisDirection.X).TransformToAxisSpace(valA);

                var valB = data.GetValue(o, attributeB, lomB);
                var y = GetAxis(AxisDirection.Y).TransformToAxisSpace(valB);

                points.Add(new Vector3(x, y, 0));

                o.AddRepresentativeObject(attributeA, line);
                o.AddRepresentativeObject(attributeB, line);
            }
        }

        polyline = points.ToArray();

        xyLineComp.visBridgePort.transform.localPosition = polyline[0];
        xyLineComp.lineRenderer.positionCount = polyline.Length;
        xyLineComp.lineRenderer.SetPositions(polyline);
        line.transform.parent = Anchor.transform;

        primitive = xyLineComp;
    }

    public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
    {
        return ServiceLocator.instance.Factory2DPrimitives;
    }
}
