using GraphicalPrimitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGraphicalPrimitiveFactory  : MonoBehaviour
{
    public abstract GameObject CreateBar(float value, float rangeToNormalizeTo, float width, float depth);

    public abstract GameObject CreateGrid(Color color, AxisDirection rowDirection, AxisDirection colDir,
        bool rows = true, int rowCount = 10, float rowResolution = 0.1f, float xAxisLength = 1f,
        bool cols = true, int colCount = 10, float colResolution = 0.1f, float yAxisLength = 1f,
        float width = 0.005f);

    public abstract GameObject CreateLabel(string labelText);

    public abstract GameObject CreateAxis(Color color, string variableName, string variableUnit, AxisDirection axisDirection, float length, float width = 0.01f, bool tipped = true, bool ticked = false);
}
