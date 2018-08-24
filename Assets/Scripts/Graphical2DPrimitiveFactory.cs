using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphical2DPrimitiveFactory : AGraphicalPrimitiveFactory
{
    public GameObject bar2D;

    public override GameObject CreateAxis(Color color, string variableName, string variableEntity, Vector3 direction, float length, bool tipped = true, float width = 0.01F)
    {
        Vector3 start = new Vector3();
        Vector3 end = new Vector3(direction.x, direction.y, direction.z);
        end.Normalize();
        end *= length;

        GameObject axis = new GameObject("Axis");

        // Base
        GameObject axisBase = new GameObject();
        axisBase.transform.localPosition = start;
        axisBase.AddComponent<LineRenderer>();
        LineRenderer lr = axisBase.GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        axisBase.transform.parent = axis.transform;

        // Tip
        if (tipped)
        {
            GameObject axisTip = new GameObject();
            axisTip.AddComponent<LineRenderer>();
            LineRenderer lrT = axisTip.GetComponent<LineRenderer>();
            lrT.useWorldSpace = false;
            lrT.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lrT.startColor = color;
            lrT.endColor = color;
            lrT.startWidth = 3 * width;
            lrT.endWidth = 0;
            lrT.SetPosition(0, end);
            lrT.SetPosition(1, new Vector3(end.x, end.y + 4 * width, end.z));
            axisTip.transform.parent = axis.transform;
        }

        return axis;
    }

    public override GameObject CreateGrid(Color color, Vector3 rowDir, Vector3 colDir,
        bool rows = true, int rowCount = 10, float rowResolution = 0.1f, float xAxisLength = 1f,
        bool cols = true, int colCount = 10, float colResolution = 0.1f, float yAxisLength = 1f,
        float width = 0.005f)
    {
        GameObject grid = new GameObject("grid");

        Vector3 start = new Vector3();

        if (rows)
        {
            for (int row = 1; row <= rowCount; row++)
            {
                float rowValue = row * rowResolution;
                start.y = rowValue;

                GameObject gridRow = CreateAxis(color, "", "", rowDir, xAxisLength, false, width);
                gridRow.transform.localPosition = start;
                gridRow.transform.parent = grid.transform;
            }
        }

        if (cols)
        {
            for (int col = 1; col <= colCount; col++)
            {
                float colValue = col * colResolution;
                start.x = colValue;

                GameObject gridCol = CreateAxis(color, "", "", colDir, yAxisLength, false, width);
                gridCol.transform.localPosition = start;
                gridCol.transform.parent = grid.transform;
            }
        }

        return grid;
    }

    public override GameObject CreateBar(float value, float rangeToNormalizeTo, float width, float depth)
    {
        GameObject bar = Instantiate(bar2D);
        bar.GetComponent<GraphicalPrimitive.Bar2D>().SetSize(width, value/rangeToNormalizeTo);

        return bar;
    }
}
