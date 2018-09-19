using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public class ETV3DScatterPlot : AETV3D
{
    public GameObject Anchor;

    private DataSet data;
    private Vector3Int floatAttIDs;
    private bool initialized = false;

    private ScatterDot3D[] dots;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(initialized)
        {
            foreach(ScatterDot3D dot in dots)
            {
                dot.gameObject.transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
            }
        }
    }

    public void Init(DataSet data, int att1ID, int att2ID, int att3ID,
        LevelOfMeasurement type1, LevelOfMeasurement type2, LevelOfMeasurement type3)
    {
        this.data = data;
        this.floatAttIDs = new Vector3Int(att1ID, att2ID, att3ID);
        SetUpAxis();
        DrawGraph();
        this.initialized = true;
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        foreach(ScatterDot3D dot in dots)
        {
            Color color = Color.HSVToRGB(.5f, 1, 1);
            dot.SetColor(color);
            dot.ApplyColor(color);
        }
    }

    public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        throw new System.NotImplementedException();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.ratAttributes[floatAttIDs.x], "", AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, 0);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        xAxis2D.min = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.x]].zeroBoundMin;
        xAxis2D.max = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.x]].zeroBoundMax;
        xAxis2D.CalculateTickResolution();
        xAxis2D.UpdateAxis();

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.ratAttributes[floatAttIDs.y], "", AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, 0);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        yAxis2D.min = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.y]].zeroBoundMin;
        yAxis2D.max = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.y]].zeroBoundMax;
        yAxis2D.CalculateTickResolution();
        yAxis2D.UpdateAxis();

        // z-Axis
        GameObject zAxis = factory2D.CreateAxis(Color.white, data.ratAttributes[floatAttIDs.z], "", AxisDirection.Z, 1f, .01f, true, true);
        zAxis.transform.localPosition = new Vector3(0, 0, 0);
        zAxis.transform.parent = Anchor.transform;
        Axis2D zAxis2D = zAxis.GetComponent<Axis2D>();
        zAxis2D.ticked = true;
        zAxis2D.min = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.z]].zeroBoundMin;
        zAxis2D.max = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.z]].zeroBoundMax;
        zAxis2D.CalculateTickResolution();
        zAxis2D.UpdateAxis();
    }

    public void DrawGraph()
    {
        var dotArray = new List<ScatterDot3D>();

        RatioDataDimensionMeasures measuresX = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.x]];
        RatioDataDimensionMeasures measuresY = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.y]];
        RatioDataDimensionMeasures measuresZ = data.dataMeasuresRatio[data.ratAttributes[floatAttIDs.z]];

        for(int i = 0; i < data.informationObjects.Count; i++)
        {
            InformationObject o = data.informationObjects[i];

            float x = 0, y = 0, z = 0;
            x = measuresX.NormalizeToZeroBoundRange(o.ratioAtt[floatAttIDs.x].value);
            y = measuresY.NormalizeToZeroBoundRange(o.ratioAtt[floatAttIDs.y].value);
            z = measuresZ.NormalizeToZeroBoundRange(o.ratioAtt[floatAttIDs.z].value);

            if(!float.IsNaN(x) && !float.IsNaN(y) && !float.IsNaN(z))
            {
                GameObject dot = ServiceLocator.instance.PrimitiveFactory3Dservice.CreateScatterDot();
                dot.transform.position = new Vector3(x, y, z);
                dot.transform.parent = Anchor.transform;
                dotArray.Add(dot.GetComponent<ScatterDot3D>());

                o.AddRepresentativeObject(data.ratAttributes[floatAttIDs.x], dot);
                o.AddRepresentativeObject(data.ratAttributes[floatAttIDs.y], dot);
                o.AddRepresentativeObject(data.ratAttributes[floatAttIDs.z], dot);
            }
        }

        dots = dotArray.ToArray();
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }

    
}
