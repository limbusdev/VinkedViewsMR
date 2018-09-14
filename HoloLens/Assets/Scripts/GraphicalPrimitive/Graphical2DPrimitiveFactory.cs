using GraphicalPrimitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphical2DPrimitiveFactory : AGraphicalPrimitiveFactory
{
    public GameObject bar2D;
    public GameObject label;
    public GameObject Axis2DPrefab;
    public GameObject PCPLine2DPrefab;
    public GameObject XYLine2DPrefab;
    public GameObject ScatterDot2DPrefab;

    public override GameObject CreateAxis(Color color, string variableName, string variableEntity, 
        AxisDirection axisDirection, float length, float width = 0.01F, bool tipped = true, bool ticked = false)
    {
        GameObject axis = Instantiate(Axis2DPrefab);
        Axis2D axis2Dcomp = axis.GetComponent<Axis2D>();
        axis2Dcomp.diameter = width;
        axis2Dcomp.color = color;
        axis2Dcomp.labelVariableText = variableName;
        axis2Dcomp.labelUnitText = variableEntity;
        axis2Dcomp.AxisDirection = axisDirection;
        axis2Dcomp.tipped = tipped;
        axis2Dcomp.length = length;
        axis2Dcomp.ticked = ticked;
        axis2Dcomp.UpdateAxis();
        

        return axis;
    }

    public override GameObject CreateGrid(Color color, AxisDirection rowDirection, AxisDirection colDir,
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

                GameObject gridRow = CreateAxis(color, "", "", rowDirection, xAxisLength, width, false, false);
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

                GameObject gridCol = CreateAxis(color, "", "", rowDirection, yAxisLength, width, false, false);
                gridCol.transform.localPosition = start;
                gridCol.transform.parent = grid.transform;
            }
        }

        return grid;
    }

    public override GameObject CreateBar(float value, float rangeToNormalizeTo, float width, float depth)
    {
        GameObject bar = Instantiate(bar2D);
        bar.GetComponent<Bar2D>().SetSize(width, value/rangeToNormalizeTo);

        return bar;
    }

    public GameObject CreatePCPLine()
    {
        return Instantiate(PCPLine2DPrefab);
    }

    public GameObject CreateXYLine()
    {
        return Instantiate(XYLine2DPrefab);
    }

    public override GameObject CreateLabel(string labelText)
    {
        GameObject newLabel = Instantiate(label);
        newLabel.GetComponent<TextMesh>().text = labelText;
        return newLabel;
    }

    public override GameObject CreateScatterDot()
    {
        return Instantiate(ScatterDot2DPrefab);
    }
}
