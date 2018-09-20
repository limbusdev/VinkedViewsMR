using GraphicalPrimitive;

public interface IEuclideanTransformableView
{
    void ChangeColoringScheme(ETVColorSchemes scheme);
    void SetUpAxis();
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
