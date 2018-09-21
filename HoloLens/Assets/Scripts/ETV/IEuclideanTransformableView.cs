using GraphicalPrimitive;
using Model;
using UnityEngine;

public interface IEuclideanTransformableView
{
    void ChangeColoringScheme(ETVColorSchemes scheme);
    void SetUpAxis();
    void AddBarChartAxis(string attributeName, AxisDirection dir, DataSet data, Transform parent);
    void AddAxis(string attributeName, LoM lom, AxisDirection dir, DataSet data, Transform parent);
    void AddAggregatedAxis(string attributeName, LoM lom, AxisDirection dir, DataSet data, Transform parent, out float max, out float length);
    void DrawGraph();
    void UpdateETV();
    void SetAxisLabels(AxisDirection axisDirection, string axisVariable);
}

public enum ETVColorSchemes
{
    Grayscale,
    Zebra,
    GrayZebra,
    Rainbow,
    LightRainbow,
    DarkRainbow,
    SplitHSV
}
