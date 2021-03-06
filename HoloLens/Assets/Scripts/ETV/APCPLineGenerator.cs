﻿/*
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
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ETV
{
    /// <summary>
    /// Generates PCP lines for visualizations.
    /// </summary>
    public abstract class APCPLineGenerator
    {
        /// <summary>
        /// Creates a new PCP line with a LineRenderer and colliders.
        /// </summary>
        /// <param name="o">InfoObject to represent</param>
        /// <param name="etv">Base ETV</param>
        /// <param name="color">color for this line</param>
        /// <param name="data">data set this InfoObject came from</param>
        /// <param name="nominalIDs">nominal IDs to map</param>
        /// <param name="ordinalIDs">ordinal IDs to map</param>
        /// <param name="intervalIDs">interval IDs to map</param>
        /// <param name="ratioIDs">rational IDs to map</param>
        /// <param name="axes">axes to map the values to</param>
        /// <param name="global">will lines reside in local or global space</param>
        /// <param name="zOffset">in which z plane, relative to linerenderer, should the line reside</param>
        /// <returns></returns>
        public APCPLine CreateLine(
            InfoObject o,
            Color color,
            Color brushingColor,
            DataSet data,
            int[] nominalIDs,
            int[] ordinalIDs,
            int[] intervalIDs,
            int[] ratioIDs,
            IDictionary<string, AAxis> axes,
            bool global,
            float zOffset = 0f
            )
        {
            // Check whether a value of one dimension is missing. If yes, abort.
            bool missing = data.IsInfoObjectCompleteRegarding(o, nominalIDs, ordinalIDs, intervalIDs, ratioIDs);
            if(missing) return null;


            // ...................................................... geometry calculation .. START
            // Generate a list of positions, to be connected with the PCP line

            var axisValues = CalculatePCPPoints(o, data, nominalIDs, ordinalIDs, intervalIDs, ratioIDs);
            var points = AssemblePolyline(axes, axisValues, global, zOffset);
            // ...................................................... geometry calculation .. END

            var factory = GetProperFactory();
            var pcpLine = factory.CreatePCPLine(color, brushingColor, points.ToArray(), axisValues, .01f);
            pcpLine.LR.useWorldSpace = global;
            pcpLine.visBridgePort.transform.localPosition = points[0];

            return pcpLine;
        }
        

        /// <summary>
        /// Converts every attribute instance into a float value and concatenates them to a list.
        /// </summary>
        /// <param name="o">information object to use</param>
        /// <param name="data">data set the information object originates from</param>
        /// <param name="nominalIDs">IDs of the nominal attributes to represent</param>
        /// <param name="ordinalIDs">IDs of the ordinal attributes to represent</param>
        /// <param name="intervalIDs">IDs of the interval attributes to represent</param>
        /// <param name="ratioIDs">IDs of the rational attributes to represent</param>
        /// <returns>a dictionary of attribute instances by attribute names</returns>
        private static IDictionary<string, float> CalculatePCPPoints(
            InfoObject o,
            DataSet data,
            int[] nominalIDs,
            int[] ordinalIDs,
            int[] intervalIDs,
            int[] ratioIDs)
        {
            var axisValues = new Dictionary<string, float>();

            foreach(var id in nominalIDs)
            {
                var name = data.nomAttribNames[id];
                var value = data.ValueOf(o, name);
                axisValues.Add(name, value);
            }

            foreach(var id in ordinalIDs)
            {
                var name = data.ordAttribNames[id];
                var value = data.ValueOf(o, name);
                axisValues.Add(name, value);
            }

            foreach(var id in intervalIDs)
            {
                var name = data.ivlAttribNames[id];
                var value = data.ValueOf(o, name);
                axisValues.Add(name, value);
            }

            foreach(var id in ratioIDs)
            {
                var name = data.ratAttribNames[id];
                var value = data.ValueOf(o, name);
                axisValues.Add(name, value);
            }

            return axisValues;
        }

        /// <summary>
        /// Takes a list of attribute instances as floats and transforms them into the axis space of their axis.
        /// </summary>
        /// <param name="axes">list of dimensional axes</param>
        /// <param name="axisValues">list of attribute instances as floats</param>
        /// <param name="global">whether to reside in global or local space</param>
        /// <param name="zOffset">offset from PCP to PCP line</param>
        /// <returns>list of 3D vector positions, resembling the anchor points of the pcp line at the various axes</returns>
        private static IList<Vector3> AssemblePolyline(
            IDictionary<string, AAxis> axes, 
            IDictionary<string, float> axisValues, 
            bool global,
            float zOffset
            )
        {
            var points = new List<Vector3>();

            int counter = 0;
            foreach(var name in axisValues.Keys)
            {
                float value;
                var axis = axes[name];

                if(global)
                {
                    var point = axis.GetLocalValueInGlobalSpace(axisValues[name]);
                    points.Add(point);
                } else
                {
                    value = axis.TransformToAxisSpace(axisValues[name]);
                    points.Add(new Vector3(.5f * counter, value, zOffset));
                }
                counter++;
            }

            return points;
        }

        public static void UpdatePolyline(
            APCPLine line,
            IDictionary<string, AAxis> axes,
            bool global,
            float zOffset = 0f
            )
        {
            var points = new Vector3[line.Values.Count];
            // Assemble Polyline

            int counter = 0;
            foreach(var key in line.Values.Keys)
            {
                var value = line.Values[key];
                if(global)
                {
                    points[counter] = axes[key].GetLocalValueInGlobalSpace(value);
                } else
                {
                    points[counter] = new Vector3(.5f * counter, value, zOffset);
                }
                counter++;
            }

            line.UpdatePoints(points);

            if(global)
            {
                line.visBridgePort.transform.position = points[0];
            } else
            {
                line.visBridgePort.transform.localPosition = points[0];
            }
        }

        /// <summary>
        /// Abstract method for inheriting classes to override and provide the best primitive factory.
        /// Simply call ServiceLocator.
        /// </summary>
        /// <returns></returns>
        public abstract AGraphicalPrimitiveFactory GetProperFactory();
    }
}
