﻿using GraphicalPrimitive;
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
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.ratAttribNames[floatAttributeX], "", AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        if(xBoundToZero)
        {
            xAxis2D.min = data.ratioAttribStats[data.ratAttribNames[floatAttributeX]].zeroBoundMin;
            xAxis2D.max = data.ratioAttribStats[data.ratAttribNames[floatAttributeX]].zeroBoundMax;
        } else
        {
            xAxis2D.min = data.ratioAttribStats[data.ratAttribNames[floatAttributeX]].min;
            xAxis2D.max = data.ratioAttribStats[data.ratAttribNames[floatAttributeX]].max;
        }
        xAxis2D.CalculateTickResolution();
        xAxis2D.UpdateAxis();
        bounds[0] += 1f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.ratAttribNames[floatAttributeY], "", AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        if(yBoundToZero)
        {
            yAxis2D.min = data.ratioAttribStats[data.ratAttribNames[floatAttributeY]].zeroBoundMin;
            yAxis2D.max = data.ratioAttribStats[data.ratAttribNames[floatAttributeY]].zeroBoundMax;
        } else
        {
            yAxis2D.min = data.ratioAttribStats[data.ratAttribNames[floatAttributeY]].min;
            yAxis2D.max = data.ratioAttribStats[data.ratAttribNames[floatAttributeY]].max;
        }
        yAxis2D.CalculateTickResolution();
        yAxis2D.UpdateAxis();
        

    }

    public void DrawGraph()
    {
        var line = ServiceLocator.instance.Factory2DPrimitives.CreateXYLine();
        var xyLineComp = line.GetComponent<XYLine2D>();
        var polyline = new Vector3[data.infoObjects.Count];
        
        xyLineComp.lineRenderer.startWidth = 0.02f;
        xyLineComp.lineRenderer.endWidth = 0.02f;

        RatioAttributeStats measuresX = data.ratioAttribStats[data.ratAttribNames[floatAttributeX]];
        RatioAttributeStats measuresY = data.ratioAttribStats[data.ratAttribNames[floatAttributeY]];

        for(int i = 0; i < data.infoObjects.Count; i++)
        {
            InfoObject o = data.infoObjects[i];

            float x=0, y=0;
            if(xBoundToZero && yBoundToZero)
            {
                x = measuresX.NormalizeToZeroBoundRange(o.ratAttribVals[floatAttributeX].value);
                y = measuresY.NormalizeToZeroBoundRange(o.ratAttribVals[floatAttributeY].value);
            } else if(xBoundToZero && !yBoundToZero)
            {
                x = measuresX.NormalizeToZeroBoundRange(o.ratAttribVals[floatAttributeX].value);
                y = measuresY.NormalizeToRange(o.ratAttribVals[floatAttributeY].value);
            } else if(!xBoundToZero && yBoundToZero)
            {
                x = measuresX.NormalizeToRange(o.ratAttribVals[floatAttributeX].value);
                y = measuresY.NormalizeToZeroBoundRange(o.ratAttribVals[floatAttributeY].value);
            } else if(!xBoundToZero && !yBoundToZero)
            {
                x = measuresX.NormalizeToRange(o.ratAttribVals[floatAttributeX].value);
                y = measuresY.NormalizeToRange(o.ratAttribVals[floatAttributeY].value);
            }

            polyline[i] = new Vector3(x,y,0);

            o.AddRepresentativeObject(data.ratAttribNames[floatAttributeX], line);
            o.AddRepresentativeObject(data.ratAttribNames[floatAttributeY], line);
        }

        xyLineComp.visBridgePort.transform.localPosition = polyline[0];
        xyLineComp.lineRenderer.positionCount = polyline.Length;
        xyLineComp.lineRenderer.SetPositions(polyline);
        line.transform.parent = Anchor.transform;

        primitive = xyLineComp;
    }
}
