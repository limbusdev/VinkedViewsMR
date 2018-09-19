using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 3D Euclidean transformable view: 3D Bar Map
/// 
/// A 3D bar chart, which visualizes two nominal attributes and their
/// distribution.
/// </summary>
public class ETV3DBarMap : AETV3D
{
    public GameObject Anchor;

    private DataSet data;
    private Bar3D[,] bars;

    private int[,] absMapValues;
    private float[,] barHeights;

    int attributeX;
    int attributeY;

    float max;

    private NominalDataDimensionMeasures measuresX, measuresY;

    private IDictionary<AxisDirection, GameObject> axis;

    
    /// <summary>
    /// Initializes the visualization with a dataset and which
    /// attributes to use.
    /// </summary>
    /// <param name="data">DataSet containing the attribute distribution.</param>
    /// <param name="sAtt1">First nominal string attribute to use.</param>
    /// <param name="sAtt2">Second nominal string attribute to use.</param>
    public void Init(DataSet data, int sAtt1, int sAtt2)
    {
        this.data = data;
        max = 0;

        attributeX = sAtt1;
        attributeY = sAtt2;
        var nameX = data.nomAttributes[sAtt1];
        var nameY = data.nomAttributes[sAtt2];
        measuresX = data.dataMeasuresNominal[nameX];
        measuresY = data.dataMeasuresNominal[nameY];
        var keysX = measuresX.distribution.Keys.ToArray();
        var keysY = measuresY.distribution.Keys.ToArray();

        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;
        axis = new Dictionary<AxisDirection, GameObject>();
        
        absMapValues = new int[keysX.Length, keysY.Length];
        barHeights = new float[keysX.Length, keysY.Length];

        // For every possible value of attribute 1
        for(int vID1 = 0; vID1 < keysX.Length; vID1++)
        {
            string v1 = keysX[vID1];

            // For every possible value of attribute 2
            for(int vID2 = 0; vID2 < keysY.Length; vID2++)
            {
                string v2 =keysY[vID2];

                // Count how many object match both values
                var count = DataProcessor.NominalAttribute
                    .CountObjectsMatchingTwoCattegories(
                    data.informationObjects, sAtt1, sAtt2, v1, v2);

                if(count > max)
                    max = count;

                absMapValues[vID1, vID2] = count;
            }
        }

        for(int vID1 = 0; vID1 < keysX.Length; vID1++)
        {
            for(int vID2 = 0; vID2 < keysY.Length; vID2++)
            {
                barHeights[vID1, vID2] = absMapValues[vID1, vID2] / max;
            }
        }

        if(max > 0)
        {
            SetUpAxis();
            DrawGraph();

            foreach(var o in data.informationObjects)
            {
                var bar = bars[
                    Array.IndexOf(measuresX.uniqueValues, o.nominalAtt[attributeX].value),
                    Array.IndexOf(measuresY.uniqueValues, o.nominalAtt[attributeY].value)
                    ];
                o.AddRepresentativeObject(o.nominalAtt[attributeX].name, bar.gameObject);
                o.AddRepresentativeObject(o.nominalAtt[attributeY].name, bar.gameObject);
            }
        }
    }

    /// <summary>
    /// Draws the graph by using values calculated in the Init() method
    /// </summary>
    public void DrawGraph()
    {
        bars = new Bar3D[measuresX.numberOfUniqueValues, measuresY.numberOfUniqueValues];

        for(int i = 0; i < measuresX.numberOfUniqueValues; i++)
        {
            for(int ii = 0; ii < measuresY.numberOfUniqueValues; ii++)
            {
                bars[i, ii] = CreateBar(absMapValues[i,ii], max);
                GameObject barGO = bars[i, ii].gameObject;
                barGO.transform.localPosition = new Vector3(i * .15f + .1f, 0, ii * .15f + .1f);
                barGO.transform.parent = Anchor.transform;
            }
        }
    }

    /// <summary>
    /// Generates axes from the calculated values.
    /// </summary>
    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        float xAxisLength = measuresX.distribution.Count * 0.15f;
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, xAxisLength + .1f, .01f, false, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        yAxis.transform.parent = Anchor.transform;

        // z-Axis
        float yAxisLength = measuresY.distribution.Count * 0.15f;
        GameObject zAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Z, yAxisLength + .1f, .01f, false, false);
        zAxis.transform.parent = Anchor.transform;

        // Configure Axes
        Axis2D axis2DY = yAxis.GetComponent<Axis2D>();
        axis2DY.ticked = true;
        axis2DY.min = 0;
        axis2DY.max = this.max;
        axis2DY.CalculateTickResolution();

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);
        axis.Add(AxisDirection.Z, zAxis);

        SetAxisLabels(AxisDirection.X, measuresX.name, "");
        SetAxisLabels(AxisDirection.Z, measuresY.name, "");
        SetAxisLabels(AxisDirection.Y, "Amount", "");

        // Write Categories next to Axes
        var catsX = measuresX.distribution.Keys.ToArray();
        for(int catX = 0; catX < catsX.Length; catX++)
        {
            GameObject label = ServiceLocator.instance.PrimitiveFactory3Dservice.CreateLabel(catsX[catX]);
            TextMesh textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleLeft;
            textMesh.alignment = TextAlignment.Left;
            label.transform.position = new Vector3(.15f * catX + .1f, 0, -.05f);
            label.transform.localRotation = Quaternion.Euler(90, 90, 0);
            label.transform.parent = xAxis.transform;
        }

        var catsY = measuresY.distribution.Keys.ToArray();
        for (int catY = 0; catY < catsY.Length; catY++)
        {
            GameObject label = ServiceLocator.instance.PrimitiveFactory3Dservice.CreateLabel(catsY[catY]);
            TextMesh textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleRight;
            textMesh.alignment = TextAlignment.Left;
            label.transform.position = new Vector3(-.05f, 0, .15f * catY + .1f);
            label.transform.localRotation = Quaternion.Euler(90,0,0);
            label.transform.parent = zAxis.transform;
        }
    }

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private Bar3D CreateBar(float value, float range)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;
        
        Bar3D bar = factory3D.CreateBar(value, range, .1f, .1f).GetComponent<Bar3D>();

        bar.SetLabelText(value.ToString());
        bar.SetLabelCategoryText("");

        return bar;
    }



    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        switch(scheme)
        {
            default: // case SplitHSV
                float H = 0f;
                for(int row=0; row<measuresX.numberOfUniqueValues; row++)
                {
                    float S = 0f;
                    for(int col=0; col<measuresY.numberOfUniqueValues; col++)
                    {
                        var color = Color.HSVToRGB(
                            (H/measuresX.numberOfUniqueValues)/2f+.5f, 
                            (S/measuresY.numberOfUniqueValues)/2f+.5f, 
                            1);
                        bars[row, col].SetColor(color);
                        bars[row, col].ApplyColor(color);
                        S++;
                    }
                    H++;
                }
                break;
        }
    }

    public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        axis[axisDirection].GetComponent<Axis2D>().labelVariableText = axisVariable;
        axis[axisDirection].GetComponent<Axis2D>().UpdateAxis();
    }

    public override void UpdateETV()
    {

    }
}
