using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public class ETV3DBarMap : AETV3D
{
    public GameObject Anchor;

    private DataSetMatrix2x2Nominal data;
    private GameObject[,] bars;
    private IDictionary<AxisDirection, GameObject> axis;
    private float ticksY;

    public void Init(DataSetMatrix2x2Nominal data, float ticks)
    {
        this.data = data;
        this.ticksY = ticks;

        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;
        bars = new GameObject[data.categoriesX.Length, data.categoriesY.Length];
        axis = new Dictionary<AxisDirection, GameObject>();
        
        UpdateETV();
    }

    public void DrawGraph()
    {
        int catCounterX = 0;
        int catCounterY = 0;

        bars = new GameObject[data.categoriesX.Length, data.categoriesY.Length];

        foreach (string categoryX in data.categoriesX)
        {
            foreach(string categoryY in data.categoriesY)
            {
                GameObject bar = CreateBar(data.ordinalValues[catCounterX, catCounterY], data.zeroBoundRange);
                bars[catCounterX, catCounterY] = bar;

                bar.transform.localPosition = new Vector3(catCounterX * 0.15f + 0.1f, 0, catCounterY * 0.15f + 0.1f);

                bar.transform.parent = Anchor.transform;

                catCounterY++;
            }
            catCounterX++;
            catCounterY = 0;
        }
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, data.categoriesX.Length * 0.15f + .1f, .01f, false, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        yAxis.transform.parent = Anchor.transform;

        // z-Axis
        GameObject zAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Z, data.categoriesY.Length * 0.15f + .1f, .01f, false, false);
        zAxis.transform.parent = Anchor.transform;

        Axis2D axis2DY = yAxis.GetComponent<Axis2D>();
        axis2DY.min = data.zeroBoundMin;
        axis2DY.max = data.zeroBoundMax;
        axis2DY.tickResolution = ticksY;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);
        axis.Add(AxisDirection.Z, zAxis);

        SetAxisLabels(AxisDirection.X, "", "");
        SetAxisLabels(AxisDirection.Z, "", "");
        SetAxisLabels(AxisDirection.Y, data.ordinalVariable, data.ordinalUnit);

        // Write Categories next to Axes
        for(int catX = 0; catX < data.categoriesX.Length; catX++)
        {
            GameObject label = ServiceLocator.instance.PrimitiveFactory3Dservice.CreateLabel(data.categoriesX[catX]);
            TextMesh textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleLeft;
            textMesh.alignment = TextAlignment.Left;
            label.transform.position = new Vector3(.15f * catX + .1f, 0, -.05f);
            label.transform.localRotation = Quaternion.Euler(90, 90, 0);
            label.transform.parent = xAxis.transform;
        }

        for (int catY = 0; catY < data.categoriesY.Length; catY++)
        {
            GameObject label = ServiceLocator.instance.PrimitiveFactory3Dservice.CreateLabel(data.categoriesY[catY]);
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
    private GameObject CreateBar(float value, float range)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;
        
        GameObject bar = factory3D.CreateBar(value, range, .1f, .1f);

        bar.GetComponent<Bar3D>().SetLabelText(value.ToString());
        bar.GetComponent<Bar3D>().SetLabelCategoryText("");

        return bar;
    }



    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        for(int row = 0; row < data.ordinalValues.GetLength(0); row++)
        {
            for(int col = 0; col < data.ordinalValues.GetLength(1); col++)
            {
                GameObject bar = bars[row, col];
                Color color;
                switch (scheme)
                {
                    case ETVColorSchemes.Rainbow:
                        color = Color.HSVToRGB(data.ordinalValues[row, col] / data.zeroBoundRange, 1, 1);
                        break;
                    default:
                        color = Color.HSVToRGB(0, 0, data.ordinalValues[row, col] / data.zeroBoundRange);
                        break;
                }
                bar.GetComponent<Bar3D>().SetColor(color);
                bar.GetComponent<Bar3D>().ApplyColor(color);
            }
        }
    }

    public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        axis[axisDirection].GetComponent<Axis2D>().labelUnitText = axisUnit;
        axis[axisDirection].GetComponent<Axis2D>().labelVariableText = axisVariable;
        axis[axisDirection].GetComponent<Axis2D>().UpdateAxis();
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }
}
