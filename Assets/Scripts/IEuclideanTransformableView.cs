using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEuclideanTransformableView
{
    void ChangeColoringScheme(ETVColorSchemes scheme);
    void SetUpAxis();
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
