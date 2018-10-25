using DigitalRuby.FastLineRenderer;
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
            FastLineRenderer fastLR,
            Color color, 
            DataSet data, 
            int[] nominalIDs, 
            int[] ordinalIDs, 
            int[] intervalIDs, 
            int[] ratioIDs,
            IDictionary<int, AAxis> axes,
            bool global,
            float zOffset=0f
            );
    }
}
