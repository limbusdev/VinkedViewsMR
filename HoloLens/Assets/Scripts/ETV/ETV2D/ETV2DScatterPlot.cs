using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DScatterPlot : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    private Vector2Int attIDs;
    
    private ScatterDot2D[] dots;
    private LoM type1, type2;

    public void Init(DataSet data, int attID1, int attID2, LoM type1, LoM type2)
    {
        this.data = data;
        this.attIDs = new Vector2Int(attID1, attID2);
        this.type1 = type1;
        this.type2 = type2;
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
        SetUpXAxis();
        SetUpYAxis();

        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.ratAttributes[attIDs.x], "", AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        xAxis2D.min = data.dataMeasuresRatio[data.ratAttributes[attIDs.x]].zeroBoundMin;
        xAxis2D.max = data.dataMeasuresRatio[data.ratAttributes[attIDs.x]].zeroBoundMax;
        xAxis2D.CalculateTickResolution();
        xAxis2D.UpdateAxis();
        bounds[0] += 1f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.ratAttributes[attIDs.y], "", AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        yAxis2D.min = data.dataMeasuresRatio[data.ratAttributes[attIDs.y]].zeroBoundMin;
        yAxis2D.max = data.dataMeasuresRatio[data.ratAttributes[attIDs.y]].zeroBoundMax;
        yAxis2D.CalculateTickResolution();
        yAxis2D.UpdateAxis();


        bounds[1] += 1f + .5f;

    }

    private void SetUpXAxis()
    {
        var factory2D = ServiceLocator.instance.Factory2DPrimitives;

        GameObject axis;
        string attName;

        switch(type1)
        {
            case LoM.NOMINAL:
                attName = data.nomAttributes[attIDs.x];
                axis = factory2D.CreateAxis(Color.white, attName, "", AxisDirection.X, 1f, .01f, true, true);
                var axisComp = axis.GetComponent<Axis2D>();
                axisComp.min = 0;
                axisComp.max = data.dataMeasuresNominal[attName].numberOfUniqueValues;
                break;
            case LoM.ORDINAL:
                attName = data.ordAttributes[attIDs.x];
                axis = factory2D.CreateAxis(Color.white, attName, "", AxisDirection.X, 1f, .01f, true, true);
                // TODO
                break;
            case LoM.INTERVAL:
                attName = data.ivlAttributes[attIDs.x];
                axis = factory2D.CreateAxis(Color.white, attName, "", AxisDirection.X, 1f, .01f, true, true);
                // TODO
                break;
            default: // RATIO
                attName = data.ratAttributes[attIDs.x];
                axis = factory2D.CreateAxis(Color.white, attName, "", AxisDirection.X, 1f, .01f, true, true);
                // TODO
                break;
        }

        axis.transform.parent = Anchor.transform;
    }

    private void SetUpYAxis()
    {

    }

    public void DrawGraph()
    {
        var dotArray = new List<ScatterDot2D>();
        

        RatioDataDimensionMeasures measuresX = data.dataMeasuresRatio[data.ratAttributes[attIDs.x]];
        RatioDataDimensionMeasures measuresY = data.dataMeasuresRatio[data.ratAttributes[attIDs.y]];

        for(int i = 0; i < data.informationObjects.Count; i++)
        {
            InfoObject o = data.informationObjects[i];

            float x = 0, y = 0;
            x = measuresX.NormalizeToZeroBoundRange(o.ratioAtt[attIDs.x].value);
            y = measuresY.NormalizeToZeroBoundRange(o.ratioAtt[attIDs.y].value);


            if(!float.IsNaN(x) && !float.IsNaN(y))
            {
                GameObject dot = ServiceLocator.instance.Factory2DPrimitives.CreateScatterDot();
                dot.transform.position = new Vector3(x, y, 0);
                dot.transform.parent = Anchor.transform;
                dotArray.Add(dot.GetComponent<ScatterDot2D>());

                o.AddRepresentativeObject(data.ratAttributes[attIDs.x], dot);
                o.AddRepresentativeObject(data.ratAttributes[attIDs.y], dot);
            }
        }

        dots = dotArray.ToArray();
    }   
}






