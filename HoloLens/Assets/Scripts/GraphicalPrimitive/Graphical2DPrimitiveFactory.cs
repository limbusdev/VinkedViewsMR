/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

public class Graphical2DPrimitiveFactory : AGraphicalPrimitiveFactory
{
    public ABar bar2D;
    public GameObject label;
    public GameObject Axis2DPrefab;
    public APCPLine PCPLine2DPrefab;
    public GameObject XYLine2DPrefab;
    public AScatterDot ScatterDot2DPrefab;
    public GameObject TickPrefab;
    public GameObject GridLinePrefab;

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
        axis2Dcomp.Init(new AttributeStats(LoM.RATIO, variableName));
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
        
        switch(data.TypeOf(name))
        {
            case LoM.NOMINAL:
                axis2Dcomp.Init(data.nominalStatistics[name], direction);
                break;
            case LoM.ORDINAL:
                axis2Dcomp.Init(data.ordinalStatistics[name], direction);
                break;
            case LoM.INTERVAL:
                axis2Dcomp.Init(data.intervalStatistics[name], direction);
                break;
            default: // RATIO
                axis2Dcomp.Init(data.rationalStatistics[name], direction);
                break;
        }
        
        return axis;
    }

    public override GameObject CreateFixedLengthAutoTickedAxis(string name, float length, AxisDirection direction, DataSet data)
    {
        if(data.TypeOf(name) == LoM.INTERVAL || data.TypeOf(name) == LoM.RATIO)
        {
            return CreateAutoTickedAxis(name, direction, data);
        } else
        {

            GameObject axis = Instantiate(Axis2DPrefab);
            Axis2D axis2Dcomp = axis.GetComponent<Axis2D>();

            switch(data.TypeOf(name))
            {
                case LoM.NOMINAL:
                    axis2Dcomp.Init(data.nominalStatistics[name], direction, true, length);
                    break;
                case LoM.ORDINAL:
                    axis2Dcomp.Init(data.ordinalStatistics[name], direction, true, length);
                    break;
                default:
                    axis = new GameObject("Creation Failed");
                    break;
            }

            return axis;
        }
    }

    public override GameObject CreateAutoGrid(float max, Vector3 axisDir, Vector3 expansionDir, float length)
    {
        GameObject grid = new GameObject("grid");

        var ticks = new List<GameObject>();

        float range = Mathf.Abs(max);
        int tickCount = 11;
        float unroundedTickSize = range / (tickCount - 1);
        float x = Mathf.Ceil(Mathf.Log10(unroundedTickSize) - 1);
        float pow10x = Mathf.Pow(10, x);
        float tickResolution = Mathf.Ceil(unroundedTickSize / pow10x) * pow10x;
        
        int tickCounter = 0;
        for(float i = 0; i <= max; i += tickResolution)
        {
            GameObject tick = Instantiate(GridLinePrefab);
            var lineRend = tick.GetComponent<LineRenderer>();
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
        
        int tickCounter = 0;
        for(float i = min; i <= max; i += tickResolution)
        {
            GameObject tick = Instantiate(GridLinePrefab);
            var lineRend = tick.GetComponent<LineRenderer>();
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, axisDir * length);

            tick.transform.parent = grid.transform;
            tick.transform.localPosition = expansionDir * (tickCounter * tickResolution) / (max-min);
            ticks.Add(tick);
            tickCounter++;
        }

        return grid;
    }

    public override ABar CreateBar(float value, float width, float depth=1f)
    {
        var bar = Instantiate(bar2D);
        bar.SetSize(width, value, depth);

        return bar;
    }

    public override APCPLine CreatePCPLine()
    {
        var line = Instantiate(PCPLine2DPrefab);
        return line;
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

    public override AScatterDot CreateScatterDot()
    {
        return Instantiate(ScatterDot2DPrefab);
    }

    public Tick CreateTick(bool withLabel)
    {
        if(withLabel)
            return Instantiate(TickPrefab).GetComponent<Tick>();
        else
        {
            var tick = Instantiate(TickPrefab).GetComponent<Tick>();
            tick.label.gameObject.SetActive(false);
            return tick;
        }
    }
}
