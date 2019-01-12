using GraphicalPrimitive;
using Model;

namespace ETV
{
    /// <summary>
    /// Interface for ETV implementations.
    /// 
    /// T is the class of graphical primitives, the visualization is made of
    /// </summary>
    public interface IEuclideanTransformableView
    {
        void ChangeColoringScheme(ETVColorSchemes scheme);
        void SetUpAxes();
        void AddBarChartAxis(string attributeName, AxisDirection dir);
        AAxis AddAxis(string attributeName, AxisDirection dir, float length=1f);
        void AddAggregatedAxis(string attributeName, AxisDirection dir, out float max, out float length);
        void UpdateETV();
        void SetAxisLabels(AxisDirection axisDirection, string axisVariable);

        /// <summary>
        /// Remove all components, dispose them as well and inform
        /// observers about disposal.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Setup of the graphical visualization. Do not forget to register graphical
        /// primitives with their represented information objects with
        /// ServiceLocator.VisBridgeSystem.RegisterGraphicalPrimitiveFor(InfoObject o);
        /// </summary>
        void DrawGraph();

        /// <summary>
        /// Keeps list of relations for updating
        /// </summary>
        /// <param name="o"></param>
        /// <param name="t"></param>
        void RememberRelationOf(InfoObject o, AGraphicalPrimitive p);
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
}