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

using GraphicalPrimitive;
using System.Collections.Generic;
using UnityEngine;

namespace MetaVisualization
{
    public abstract class AMetaVisSystem : MonoBehaviour
    {
        // ........................................................................ INNER CLASSES
        public enum MetaVisType
        {
            FlexibleLinkedAxis, ImmersiveAxis,
        }

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
        public abstract void ObserveAxis(AAxis axis);

        /// <summary>
        /// Removes the given axis from the list of permanently observed axes.
        /// Use this, when destroying or disabling a visualization.
        /// </summary>
        /// <param name="axis"></param>
        public abstract void StopObservationOf(AAxis axis);

        /// <summary>
        /// Generate a metavisualization between the given axes.
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>A new metavisualization</returns>
        public abstract GameObject SpanMetaVisBetween(AAxis axisA, AAxis axisB, int dataSetID);

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
        /// </summary>
        /// <param name="axisA"></param>
        /// <param name="axisB"></param>
        /// <returns>Whether such a dataset exists</returns>
        public abstract bool CheckIfCompatible(AAxis axisA, AAxis axisB, out int dataSetID);

        public abstract MetaVisType WhichMetaVis(AAxis axisA, AAxis axisB, int dataSetID);

        public abstract GameObject SpanMetaVisImmersiveAxis(AAxis axisA, AAxis axisB, int dataSetID);



        ///////////////////////////////////////////////////////////////////////////////////////////
        // ........................................................................ MetaVis FACTORY

        /// <summary>
        /// Generates a 2D Parallel Coordinates Plot for n attributes.
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="variables">Attributes to be present in the PCP.</param>
        /// <returns>GameObject containing the anchored visualization.</returns>
        public abstract GameObject GenerateImmersiveAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB);
    }
}