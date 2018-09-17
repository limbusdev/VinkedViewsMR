using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DScatterPlot : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    private Vector2Int floatAttIDs;
    
    private ScatterDot2D[] dots;

    public void Init(DataSet data, int floatAttributeIDX, int floatAttributeIDY)
    {
        this.data = data;
        this.floatAttIDs = new Vector2Int(floatAttributeIDX, floatAttributeIDY);
        SetUpAxis();
        DrawGraph();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        foreach(ScatterDot2D dot in dots)
        {
            Color color = Color.HSVToRGB(.5f, 1, 1);
            dot.SetColor(color);
            dot.ApplyColor(color);
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
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.ratAttributes[floatAttIDs.x], "", AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        xAxis2D.min = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.x]].zeroBoundMin;
        xAxis2D.max = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.x]].zeroBoundMax;
        xAxis2D.CalculateTickResolution();
        xAxis2D.UpdateAxis();
        bounds[0] += 1f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.ratAttributes[floatAttIDs.y], "", AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        yAxis2D.min = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.y]].zeroBoundMin;
        yAxis2D.max = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.y]].zeroBoundMax;
        yAxis2D.CalculateTickResolution();
        yAxis2D.UpdateAxis();


        bounds[1] += 1f + .5f;

    }

    public void DrawGraph()
    {
        var dotArray = new List<ScatterDot2D>();
        

        RatioDataDimensionMeasures measuresX = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.x]];
        RatioDataDimensionMeasures measuresY = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.y]];

        for(int i = 0; i < data.informationObjects.Count; i++)
        {
            InformationObject o = data.informationObjects[i];

            float x = 0, y = 0;
            x = measuresX.NormalizeToZeroBoundRange(o.ratioAtt[floatAttIDs.x].value);
            y = measuresY.NormalizeToZeroBoundRange(o.ratioAtt[floatAttIDs.y].value);


            if(!float.IsNaN(x) && !float.IsNaN(y))
            {
                GameObject dot = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateScatterDot();
                dot.transform.position = new Vector3(x, y, 0);
                dot.transform.parent = Anchor.transform;
                dotArray.Add(dot.GetComponent<ScatterDot2D>());

                o.AddRepresentativeObject(data.ratAttributes[floatAttIDs.x], dot);
                o.AddRepresentativeObject(data.ratAttributes[floatAttIDs.y], dot);
            }
        }

        dots = dotArray.ToArray();
    }   
}






