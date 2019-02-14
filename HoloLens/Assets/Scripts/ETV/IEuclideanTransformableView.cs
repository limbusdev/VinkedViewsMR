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