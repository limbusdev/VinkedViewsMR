using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public abstract class APCPLineGenerator
    {
        /// <summary>
        /// Creates a new line in the given FastLineRenderer. After
        /// the last call FastLineRenderer.Apply() must be called
        /// seperately.
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
            DataSet data,
            int[] nominalIDs,
            int[] ordinalIDs,
            int[] intervalIDs,
            int[] ratioIDs,
            IDictionary<int, AAxis> axes,
            bool global,
            float zOffset = 0f
            )
        {
            bool missing = data.IsInfoObjectCompleteRegarding(o, nominalIDs, ordinalIDs, intervalIDs, ratioIDs);
            if(missing)
            {
                return null;
            }

            IDictionary<string, float> axisValues = new Dictionary<string, float>();
            List<Vector3> points = new List<Vector3>();

            foreach(var id in nominalIDs)
            {
                var name = data.nomAttribNames[id];
                var value = data.GetValue(o, name, LoM.NOMINAL);
                axisValues.Add(name, value);
            }

            foreach(var id in ordinalIDs)
            {
                var name = data.ordAttribNames[id];
                var value = data.GetValue(o, name, LoM.ORDINAL);
                axisValues.Add(name, value);
            }

            foreach(var id in intervalIDs)
            {
                var name = data.ivlAttribNames[id];
                var value = data.GetValue(o, name, LoM.INTERVAL);
                axisValues.Add(name, value);
            }

            foreach(var id in ratioIDs)
            {
                var name = data.ratAttribNames[id];
                var value = data.GetValue(o, name, LoM.RATIO);
                axisValues.Add(name, value);
            }

            // Assemble Polyline
            Vector3[] polyline = new Vector3[axisValues.Count];

            int counter = 0;
            foreach(var name in axisValues.Keys)
            {
                float value;

                if(global)
                {
                    points.Add(axes[counter].GetLocalValueInGlobalSpace(axisValues[name]));
                } else
                {
                    value = axes[counter].TransformToAxisSpace(axisValues[name]);
                    points.Add(new Vector3(.5f * counter, value, zOffset));
                }
                counter++;
            }

            var factory = GetProperFactory();
            var pcpLine = factory.CreatePCPLine(color, points.ToArray(), .02f);
            pcpLine.LR.useWorldSpace = global;
            pcpLine.visBridgePort.transform.localPosition = points[0];

            foreach(var name in axisValues.Keys)
                o.AddRepresentativeObject(name, pcpLine.gameObject);

            return pcpLine;
        }

        public abstract AGraphicalPrimitiveFactory GetProperFactory();
    }
}
