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