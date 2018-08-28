using GraphicalPrimitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEuclideanTransformableView
{
    void ChangeColoringScheme(ETVColorSchemes scheme);
    void SetUpAxis();
    void UpdateETV();
    void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit);
}

public enum ETVColorSchemes
{
    Grayscale,
    Zebra,
    GrayZebra,
    Rainbow,
    LightRainbow,
    DarkRainbow
}
