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

    public override GameObject CreateGrid(Color color, Vector3 axisDir, Vector3 expansionDir, float length, float width, float min, float max)
    {
        GameObject grid = new GameObject("grid");

        var ticks = new List<GameObject>();

        float range = Mathf.Abs(max - min);
        int tickCount = 11;
        float unroundedTickSize = range / (tickCount - 1);
        float x = Mathf.Ceil(Mathf.Log10(unroundedTickSize) - 1);
        float pow10x = Mathf.Pow(10, x);
        float tickResolution = Mathf.Ceil(unroundedTickSize / pow10x) * pow10x;

        float diameter = .005f;

        int tickCounter = 0;
        for(float i = min; i <= max; i += tickResolution)
        {
            GameObject tick = new GameObject("Tick");
            var lineRend = tick.AddComponent<LineRenderer>();
            lineRend.useWorldSpace = false;
            lineRend.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lineRend.startColor = color;
            lineRend.endColor = color;
            lineRend.startWidth = diameter;
            lineRend.endWidth = diameter;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, axisDir * length);
            
            tick.transform.parent = grid.transform;
            tick.transform.localPosition = expansionDir * (tickCounter * tickResolution) / (max-min);
            ticks.Add(tick);
            tickCounter++;
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
