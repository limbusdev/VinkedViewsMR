using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DLineChart : AETV2D
{
    public GameObject Anchor;

    public DataSet data;
    int floatAttributeX, floatAttributeY;
    bool xBoundToZero, yBoundToZero;

    private XYLine2D primitive;

    
    public void Init(DataSet data, int floatAttributeIDx, int floatAttributeIDy, bool xAxisBoundToZero, bool yAxisBoundToZero)
    {
        this.data = data;
        this.floatAttributeX = floatAttributeIDx;
        this.floatAttributeY = floatAttributeIDy;
        this.xBoundToZero = xAxisBoundToZero;
        this.yBoundToZero = yAxisBoundToZero;
        UpdateETV();
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
        SetUpAxis();
        DrawGraph();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.attributesFloat[floatAttributeX], "", AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        if(xBoundToZero)
        {
            xAxis2D.min = data.dataMeasuresFloat[data.attributesFloat[floatAttributeX]].zeroBoundMin;
            xAxis2D.max = data.dataMeasuresFloat[data.attributesFloat[floatAttributeX]].zeroBoundMax;
        } else
        {
            xAxis2D.min = data.dataMeasuresFloat[data.attributesFloat[floatAttributeX]].min;
            xAxis2D.max = data.dataMeasuresFloat[data.attributesFloat[floatAttributeX]].max;
        }
        xAxis2D.CalculateTickResolution();
        xAxis2D.UpdateAxis();
        bounds[0] += 1f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.attributesFloat[floatAttributeY], "", AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        if(yBoundToZero)
        {
            yAxis2D.min = data.dataMeasuresFloat[data.attributesFloat[floatAttributeY]].zeroBoundMin;
            yAxis2D.max = data.dataMeasuresFloat[data.attributesFloat[floatAttributeY]].zeroBoundMax;
        } else
        {
            yAxis2D.min = data.dataMeasuresFloat[data.attributesFloat[floatAttributeY]].min;
            yAxis2D.max = data.dataMeasuresFloat[data.attributesFloat[floatAttributeY]].max;
        }
        yAxis2D.CalculateTickResolution();
        yAxis2D.UpdateAxis();
        

    }

    public void DrawGraph()
    {
        var line = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateXYLine();
        var xyLineComp = line.GetComponent<XYLine2D>();
        var polyline = new Vector3[data.informationObjects.Count];
        
        xyLineComp.lineRenderer.startWidth = 0.02f;
        xyLineComp.lineRenderer.endWidth = 0.02f;

        FloatDataDimensionMeasures measuresX = data.dataMeasuresFloat[data.attributesFloat[floatAttributeX]];
        FloatDataDimensionMeasures measuresY = data.dataMeasuresFloat[data.attributesFloat[floatAttributeY]];

        for(int i = 0; i < data.informationObjects.Count; i++)
        {
            InformationObject o = data.informationObjects[i];

            float x=0, y=0;
            if(xBoundToZero && yBoundToZero)
            {
                x = measuresX.NormalizeToZeroBoundRange(o.attributesFloat[floatAttributeX].value);
                y = measuresY.NormalizeToZeroBoundRange(o.attributesFloat[floatAttributeY].value);
            } else if(xBoundToZero && !yBoundToZero)
            {
                x = measuresX.NormalizeToZeroBoundRange(o.attributesFloat[floatAttributeX].value);
                y = measuresY.NormalizeToRange(o.attributesFloat[floatAttributeY].value);
            } else if(!xBoundToZero && yBoundToZero)
            {
                x = measuresX.NormalizeToRange(o.attributesFloat[floatAttributeX].value);
                y = measuresY.NormalizeToZeroBoundRange(o.attributesFloat[floatAttributeY].value);
            } else if(!xBoundToZero && !yBoundToZero)
            {
                x = measuresX.NormalizeToRange(o.attributesFloat[floatAttributeX].value);
                y = measuresY.NormalizeToRange(o.attributesFloat[floatAttributeY].value);
            }

            polyline[i] = new Vector3(x,y,0);

            o.AddRepresentativeObject(data.attributesFloat[floatAttributeX], line);
            o.AddRepresentativeObject(data.attributesFloat[floatAttributeY], line);
        }

        xyLineComp.visBridgePort.transform.localPosition = polyline[0];
        xyLineComp.lineRenderer.positionCount = polyline.Length;
        xyLineComp.lineRenderer.SetPositions(polyline);
        line.transform.parent = Anchor.transform;

        primitive = xyLineComp;
    }
}
