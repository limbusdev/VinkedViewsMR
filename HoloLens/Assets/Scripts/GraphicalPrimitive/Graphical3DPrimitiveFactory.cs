/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using GraphicalPrimitive;
using Model;
using UnityEngine;

public class Graphical3DPrimitiveFactory : AGraphicalPrimitiveFactory
{
    public GameObject cone;
    public ABar bar3D;
    public GameObject axis3D;
    public GameObject label;
    public AScatterDot ScatterDot3DPrefab;
    public GameObject Axis3DPrefab;
    public APCPLine PCPLine3DPrefab;

    public override GameObject CreateAxis(Color color, string variableName, string variableEntity,
        AxisDirection axisDirection, float length, float width = 0.01F, bool tipped = true, bool ticked = false)
    {
        GameObject axis;

        axis = Instantiate(axis3D);
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


    public override GameObject CreateAutoTickedAxis(string name, float max, AxisDirection dir = AxisDirection.Y)
    {
        GameObject axis = Instantiate(Axis3DPrefab);
        var axis3Dcomp = axis.GetComponent<Axis3D>();
        axis3Dcomp.Init(name, max, dir);

        return axis;
    }

    public override GameObject CreateAutoTickedAxis(string name, AxisDirection direction, DataSet data)
    {
        GameObject axis = Instantiate(Axis3DPrefab);
        var axis3Dcomp = axis.GetComponent<Axis3D>();
        
        switch(data.TypeOf(name))
        {
            case LoM.NOMINAL:
                axis3Dcomp.Init(data.nominalStatistics[name], direction);
                break;
            case LoM.ORDINAL:
                axis3Dcomp.Init(data.ordinalStatistics[name], direction);
                break;
            case LoM.INTERVAL:
                axis3Dcomp.Init(data.intervalStatistics[name], direction);
                break;
            default: // RATIO
                axis3Dcomp.Init(data.rationalStatistics[name], direction);
                break;
        }
        
        return axis;
    }

    public override GameObject CreateFixedLengthAutoTickedAxis(string name, float length, AxisDirection direction, DataSet data)
    {
        var axis = Instantiate(Axis3DPrefab);
        var axisComp = axis.GetComponent<AAxis>();

        switch(data.TypeOf(name))
        {
            case LoM.NOMINAL:
                axisComp.Init(data.nominalStatistics[name], direction, true, 1f);
                break;
            case LoM.ORDINAL:
                axisComp.Init(data.ordinalStatistics[name], direction, true, 1f);
                break;
            case LoM.INTERVAL:
                axisComp.Init(data.intervalStatistics[name], direction);
                break;
            default: // RATIO
                axisComp.Init(data.rationalStatistics[name], direction);
                break;
        }

        // TODO set length

        return axis;
    }



    public override GameObject CreateGrid(Color color, Vector3 axisDir, Vector3 expansionDir, float length, float width, float min, float max)
    { 
        throw new System.NotImplementedException();
    }

    public override ABar CreateBar(float value, float width=.1f, float depth=.1f)
    {
        var bar = Instantiate(bar3D);
        bar.SetSize(width, value, depth);
        
        return bar;
    }
   

    public override GameObject CreateLabel(string labelText)
    {
        GameObject newLabel = Instantiate(label);
        newLabel.GetComponent<TextMesh>().text = labelText;
        return newLabel;
    }

    public override AScatterDot CreateScatterDot()
    {
        return Instantiate(ScatterDot3DPrefab);
    }

    public override APCPLine CreatePCPLine()
    {
        var line = Instantiate(PCPLine3DPrefab);
        return line;
    }
}
