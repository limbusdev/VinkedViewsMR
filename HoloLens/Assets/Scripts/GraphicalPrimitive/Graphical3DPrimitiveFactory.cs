using GraphicalPrimitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphical3DPrimitiveFactory : AGraphicalPrimitiveFactory
{
    public GameObject cone;
    public GameObject bar3D;
    public GameObject axis3D;
    public GameObject label;
    public GameObject ScatterDot3DPrefab;

    public override GameObject CreateAxis(Color color, string variableName, string variableEntity,
        AxisDirection axisDirection, float length, float width = 0.01F, bool tipped = true, bool ticked = false)
    {
        GameObject axis;

        axis = Instantiate(axis3D);
        axis.GetComponent<AAxis>().labelUnitText = variableEntity;
        axis.GetComponent<AAxis>().labelVariableText = variableName;
        axis.GetComponent<AAxis>().tipped = tipped;
        axis.GetComponent<AAxis>().ticked = ticked;
        axis.GetComponent<AAxis>().diameter = width;
        axis.GetComponent<AAxis>().length = length;
        axis.GetComponent<AAxis>().color = color;
        axis.GetComponent<AAxis>().axisDirection = axisDirection;
        axis.GetComponent<AAxis>().UpdateAxis();
        

        return axis;
    }

    public override GameObject CreateGrid(Color color, AxisDirection rowDir, AxisDirection colDir, bool rows = true, int rowCount = 10, float rowResolution = 0.1F, float xAxisLength = 1, bool cols = true, int colCount = 10, float colResolution = 0.1F, float yAxisLength = 1, float width = 0.005F)
    {
        throw new System.NotImplementedException();
    }

    public override GameObject CreateBar(float value, float rangeToNormalizeTo, float width, float depth)
    {
        GameObject bar = Instantiate(bar3D);
        bar.GetComponent<Bar3D>().SetSize(width, value / rangeToNormalizeTo, depth);
        
        return bar;
    }
   

    public override GameObject CreateLabel(string labelText)
    {
        GameObject newLabel = Instantiate(label);
        newLabel.GetComponent<TextMesh>().text = labelText;
        return newLabel;
    }

    public override GameObject CreateScatterDot()
    {
        return Instantiate(ScatterDot3DPrefab);
    }
}
