using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGraphicalPrimitiveFactory  : MonoBehaviour
{
    public abstract GameObject CreateBar(float value, float width, float depth);

    public abstract GameObject CreateGrid(Color color, Vector3 axisDir, Vector3 expansionDir, float length, float width, float min, float max);

    public abstract GameObject CreateLabel(string labelText);

    public abstract GameObject CreateScatterDot();

    public abstract GameObject CreateAxis(Color color, string variableName, string variableUnit, AxisDirection axisDirection, float length, float width = 0.01f, bool tipped = true, bool ticked = false);

    public virtual GameObject CreateAutoTickedAxis(string name, AxisDirection direction, DataSet data) { return new GameObject("Dummy Axis"); }
}
