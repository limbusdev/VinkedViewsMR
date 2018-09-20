using GraphicalPrimitive;
using Model;
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
    public GameObject TickPrefab;

    public GameObject CreateEmptyAxis()
    {
        GameObject axis = Instantiate(Axis2DPrefab);
        return axis;
    }

    public override GameObject CreateAxis(Color color, string variableName, string variableEntity, 
        AxisDirection axisDirection, float length, float width = 0.01F, bool tipped = true, bool ticked = false)
    {
        GameObject axis = Instantiate(Axis2DPrefab);
        Axis2D axis2Dcomp = axis.GetComponent<Axis2D>();
        axis2Dcomp.Init(new DataDimensionMeasures(LoM.RATIO, variableName));
        axis2Dcomp.diameter = width;
        axis2Dcomp.color = color;
        axis2Dcomp.labelVariableText = variableName;
        axis2Dcomp.tipped = tipped;
        axis2Dcomp.length = length;
        axis2Dcomp.ticked = ticked;
        axis2Dcomp.UpdateAxis();
        

        return axis;
    }

    public override GameObject CreateAutoTickedAxis(string name, float max, AxisDirection dir = AxisDirection.Y)
    {
        GameObject axis = Instantiate(Axis2DPrefab);
        Axis2D axis2Dcomp = axis.GetComponent<Axis2D>();
        axis2Dcomp.Init(name, max, dir);

        return axis;
    }

    public override GameObject CreateAutoTickedAxis(string name, AxisDirection direction, DataSet data)
    {
        GameObject axis = Instantiate(Axis2DPrefab);
        Axis2D axis2Dcomp = axis.GetComponent<Axis2D>();
        
        switch(data.GetTypeOf(name))
        {
            case LoM.NOMINAL:
                axis2Dcomp.Init(data.dataMeasuresNominal[name], direction);
                break;
            case LoM.ORDINAL:
                axis2Dcomp.Init(data.dataMeasuresOrdinal[name], direction);
                break;
            case LoM.INTERVAL:
                axis2Dcomp.Init(data.dataMeasuresInterval[name], direction);
                break;
            default: // RATIO
                axis2Dcomp.Init(data.dataMeasuresRatio[name], direction);
                break;
        }
        
        return axis;
    }

    public GameObject CreateAutoGrid(float max, Vector3 axisDir, Vector3 expansionDir, float length)
    {
        GameObject grid = new GameObject("grid");

        var ticks = new List<GameObject>();

        float range = Mathf.Abs(max);
        int tickCount = 11;
        float unroundedTickSize = range / (tickCount - 1);
        float x = Mathf.Ceil(Mathf.Log10(unroundedTickSize) - 1);
        float pow10x = Mathf.Pow(10, x);
        float tickResolution = Mathf.Ceil(unroundedTickSize / pow10x) * pow10x;

        float diameter = .005f;

        int tickCounter = 0;
        for(float i = 0; i <= max; i += tickResolution)
        {
            GameObject tick = new GameObject("Tick");
            var lineRend = tick.AddComponent<LineRenderer>();
            lineRend.useWorldSpace = false;
            lineRend.alignment = LineAlignment.TransformZ;
            lineRend.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lineRend.startColor = Color.white;
            lineRend.endColor = Color.white;
            lineRend.startWidth = diameter;
            lineRend.endWidth = diameter;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, axisDir * length);

            tick.transform.parent = grid.transform;
            tick.transform.localPosition = expansionDir * (tickCounter * tickResolution) / (range);
            ticks.Add(tick);
            tickCounter++;
        }

        return grid;
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

    public override GameObject CreateBar(float value, float width, float depth=1f)
    {
        GameObject bar = Instantiate(bar2D);
        bar.GetComponent<Bar2D>().SetSize(width, value);

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

    public Tick CreateTick()
    {
        return (Instantiate(TickPrefab).GetComponent<Tick>());
    }
}
