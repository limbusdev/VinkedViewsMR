/*
Copyright 2018 Georg Eckert

(Lincensed under MIT license)

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
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
*/

using ETV;
using GraphicalPrimitive;
using System.Collections.Generic;
using UnityEngine;

namespace MetaVisualization
{
    /// <summary>
    /// Meta-visualization system. Observes lots of axes and checks, whether they
    /// are in a constellation, which implicitly indicates a meta-visualization
    /// to be spanned between them.
    /// </summary>
    public abstract class AMetaVisSystem : MonoBehaviour
    {
        // ........................................................................ INNER CLASSES
        /// <summary>
        /// Various meta-visualization forms.
        /// </summary>
        public enum MetaVisType
        {
            FlexibleLinkedAxis, ImmersiveAxis, Scatterplot2D, Scatterplot3D, HeatMap
        }

        /// <summary>
        /// Key to identify a metavisualization. Keys which contain the same axis
        /// in arbitrary order, are considered equal.
        /// </summary>
        public class MetaVisKey
        {
            public AAxis axisA, axisB;

            public MetaVisKey(AAxis axisA, AAxis axisB)
            {
                this.axisA = axisA;
                this.axisB = axisB;
            }

            public override bool Equals(object obj)
            {
                if(obj is MetaVisKey)
                {
                    var other = obj as MetaVisKey;
                    if(other.axisA.Equals(axisA) && other.axisB.Equals(axisB))
                        return true;
                    else if(other.axisA.Equals(axisB) && other.axisB.Equals(axisA))
                        return true;
                    else
                        return false;
                } else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                var hashCode = -624926263;
                hashCode = hashCode + EqualityComparer<AAxis>.Default.GetHashCode(axisA);
                hashCode = hashCode + EqualityComparer<AAxis>.Default.GetHashCode(axisB);
                return hashCode;
            }
        }


        /// <summary>
        /// Adds the given axis to the list of permanently observed axes.
        /// If observed, axis can span metavisualizations between them, if
        /// they fullfill given factors.
        /// </summary>
        /// <param name="axis"></param>
        public abstract void ObserveAxis(AETV etv, AAxis axis);

        /// <summary>
        /// Removes the given axis from the list of permanently observed axes.
        /// Use this, when destroying or disabling a visualization.
        /// </summary>
        /// <param name="axis"></param>
        public abstract void StopObservationOf(AETV etv, AAxis axis);

        /// <summary>
        /// Generate a metavisualization between the given axes.
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>A new metavisualization</returns>
        public abstract AETV SpanMetaVisBetween(AAxis axisA, AAxis axisB, int dataSetID);

        /// <summary>
        /// Checks whether the given axis tips and bases are both nearer to each other
        /// than a predefined border value.
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>If they are near enough to span a MetaVis.</returns>
        public abstract bool CheckIfNearEnough(AAxis axisA, AAxis axisB);

        /// <summary>
        /// Checks for a dataset which contains both represented attributes and 
        /// returns it's ID.
        /// Not compatible if they belong to the same ETV
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>Whether such a dataset exists</returns>
        public abstract bool CheckIfCompatible(AETV etva, AETV etvB, AAxis axisA, AAxis axisB, out int dataSetID);

        /// <summary>
        /// Checks, which meta-visualization form would be appropriate
        /// for the given axes.
        /// 
        /// For example the following MetaVis' are appropriate:
        /// 
        /// | Axis constellation                                                | MetaVis                       |
        /// +-------------------------------------------------------------------+-------------------------------+
        /// | two arbitrarily constellated axes                                 | immersive axes                |
        /// | two arbitrarily constellated axes in the same layer               | FLA                           |
        /// | two orthogonal constellated numerical axes in the same layer      | scatterplot                   |
        /// | two orthogonal constellated categorical axes in the same layer    | height map / 3D bar chart     |
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <returns>appropriate meta-visualization form</returns>
        public abstract MetaVisType WhichMetaVis(AAxis axisA, AAxis axisB, int dataSetID);

        /// <summary>
        /// Generates an immersive axis meta-visualization between the given axes of
        /// the given data set.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">secon axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <returns>new immersive axes meta-visualization</returns>
        public abstract AETV SpanMetaVisImmersiveAxis(AAxis axisA, AAxis axisB, int dataSetID);

        /// <summary>
        /// Generates a Flexible Linked Axes visualization between the given axes
        /// of the given data set.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set id of them</param>
        /// <returns>new FLA meta-visualization</returns>
        public abstract AETV SpanMetaVisFlexibleLinedAxes(AAxis axisA, AAxis axisB, int dataSetID);
        
        /// <summary>
        /// Generates a two dimensional scatterplot visualization between the
        /// given axes of the given data set. If axes origin's do not touch,
        /// axes will get duplicated and made perfectly orthogonal.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <param name="duplicateAxes">if axes should be ducplicated, because they do not touch</param>
        /// <returns>new 2d scatterplot meta-visualization</returns>
        public abstract AETV SpanMetaVisScatterplot2D(AAxis axisA, AAxis axisB, int dataSetID, bool duplicateAxes);

        /// <summary>
        /// Generates a three dimensional heat map / bar chart, representing the relative
        /// distribution of samples according to the given axes.
        /// </summary>
        /// <param name="axisA">first axis</param>
        /// <param name="axisB">second axis</param>
        /// <param name="dataSetID">data set ID of them</param>
        /// <param name="duplicateAxes">if axes should be ducplicated</param>
        /// <returns>new 3d histogramm meta-visualization</returns>
        public abstract AETV SpanMetaVisHeatmap3D(AAxis axisA, AAxis axisB, int dataSetID, bool duplicateAxes);



        ///////////////////////////////////////////////////////////////////////////////////////////
        // ........................................................................ MetaVis FACTORY

        /// <summary>
        /// Generates a 2D Parallel Coordinates Plot for n attributes.
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables">Attributes to be present in the PCP.</param>
        /// <returns>GameObject containing the anchored visualization.</returns>
        public abstract AETV GenerateImmersiveAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB);

        /// <summary>
        /// (HELPER METHOD)
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables"></param>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns></returns>
        public abstract AETV GenerateFlexibleLinkedAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB);

        /// <summary>
        /// (HELPER METHOD)
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables"></param>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <param name="duplicateAxes"></param>
        /// <returns></returns>
        public abstract AETV GenerateScatterplot2D(int dataSetID, string[] variables, AAxis axisA, AAxis axisB, bool duplicateAxes);

        /// <summary>
        /// (HELPER METHOD)
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables"></param>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <param name="duplicateAxes"></param>
        /// <returns></returns>
        public abstract AETV GenerateHeatmap3D(int dataSetID, string[] variables, AAxis axisA, AAxis axisB, bool duplicateAxes);
    }
}