using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public interface IPCPLineGenerator
    {
        PCPLine2D CreateLine(
            InfoObject o, 
            Color color, 
            DataSet data, 
            int[] nominalIDs, 
            int[] ordinalIDs, 
            int[] intervalIDs, 
            int[] ratioIDs,
            IDictionary<int, AAxis> axes,
            bool global=false,
            LineAlignment align = LineAlignment.TransformZ
            );
    }
}
