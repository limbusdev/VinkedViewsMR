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

public abstract class AGraphicalPrimitiveFactory  : MonoBehaviour
{
    // ........................................................................ Populate in Editor

    public LineRenderer lineRenderer2DPrefab;
    public LineRenderer lineRenderer3DPrefab;
    public LineRenderer axisLineRendererPrefab;
    public AGraphicalPrimitive PhantomPrimitivePrefab;
    public AGraphicalPrimitive BoxPrimitivePrefab;


    // ........................................................................ Explicitly implemented methods

    public AGraphicalPrimitive CreatePhantomPrimitive()
    {
        return Instantiate(PhantomPrimitivePrefab);
    }

    public AGraphicalPrimitive CreateBoxPrimitive()
    {
        return Instantiate(BoxPrimitivePrefab);
    }

    /// <summary>
    /// Creates a new blank PCP line of the given width and color.
    /// </summary>
    /// <param name="color">color to tint the line</param>
    /// <param name="width">width of the line, defaults to 0.01</param>
    /// <returns>blank PCP line</returns>
    public APCPLine CreatePCPLine(Color color, Color brushingColor, float width = .01f)
    {
        var line = CreatePCPLine();
        line.SetWidth(width);
        line.SetColor(color, brushingColor);

        return line;
    }

    /// <summary>
    /// Creates a new PCP line of the given width and color.
    /// </summary>
    /// <param name="color">color to tint the line</param>
    /// <param name="points">3D positions of the lines polygon</param>
    /// <param name="width">width of the line, defaults to 0.01</param>
    /// <returns>3D polygon PCP line</returns>
    public APCPLine CreatePCPLine(Color color, Color brushingColor, Vector3[] points, IDictionary<string, float> values, float width = .01f)
    {
        var line = CreatePCPLine(color, brushingColor, width);
        line.Init(points, values);

        return line;
    }

    public LineRenderer GetNew2DLineRenderer()
    {
        return Instantiate(lineRenderer2DPrefab);
    }
    

    public LineRenderer GetNew3DLineRenderer()
    {
        return Instantiate(lineRenderer3DPrefab);
    }

    
    public LineRenderer GetNewAxisLineRenderer()
    {
        return Instantiate(axisLineRendererPrefab);
    }
    


    // ........................................................................ Virtual Methods
    
    public virtual GameObject CreateAutoTickedAxis(string name, float max, AxisDirection dir = AxisDirection.Y) { return new GameObject("Dummy Axis"); }

    public virtual GameObject CreateAutoTickedAxis(string name, AxisDirection direction, DataSet data) { return new GameObject("Dummy Axis"); }

    public virtual GameObject CreateFixedLengthAutoTickedAxis(string name, float length, AxisDirection direction, DataSet data) { return new GameObject("Dummy Axis"); }

    public virtual GameObject CreateAutoGrid(float max, Vector3 axisDir, Vector3 expansionDir, float length) { return new GameObject("Dummy Grid"); }


    // ........................................................................ Abstract Methods

    public abstract ABar CreateBar(float value, float width, float depth);

    public abstract GameObject CreateGrid(Color color, Vector3 axisDir, Vector3 expansionDir, float length, float width, float min, float max);

    public abstract GameObject CreateLabel(string labelText);

    public abstract AScatterDot CreateScatterDot();

    public abstract GameObject CreateAxis(Color color, string variableName, string variableUnit, AxisDirection axisDirection, float length, float width = 0.01f, bool tipped = true, bool ticked = false);
    
    public abstract APCPLine CreatePCPLine();
}
