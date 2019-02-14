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
using ETV;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace MetaVisualization
{
    public class MetaVisFactory : AMetaVisFactory
    {
        [SerializeField]
        public ETV3DFlexiblePCP ETV3DFlexiblePCPPrefab;
        
        public override AETV CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            var flexPCP = Instantiate(ETV3DFlexiblePCPPrefab);

            // Get attribute IDs of the given attributes.
            int[] nomIDs, ordIDs, ivlIDs, ratIDs;
            AttributeProcessor.ExtractAttributeIDs(data, attIDs, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);
            
            flexPCP.Init(data, nomIDs, ordIDs, ivlIDs, ratIDs, axisA, axisB, true);
            flexPCP.DrawGraph();

            return flexPCP;
        }

        public override AETV CreateMetaFlexibleLinedAxes(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            throw new System.NotImplementedException();
        }

        public override AETV CreateMetaHeatmap3D(DataSet data, string[] attIDs, bool manualLength = false, float lengthA = 1f, float lengthB = 1f)
        {
            // Create Visualization
            var factory = Services.ETVFactory3D();
            var mVis = factory.CreateETVBarMap(data, attIDs[0], attIDs[1], manualLength, lengthA, lengthB, true);

            return mVis;
        }

        public override AETV CreateMetaScatterplot2D(DataSet data, string[] attIDs)
        {
            // Create Visualization
            var factory = Services.ETVFactory2D();
            var mVis = factory.CreateScatterplot(data, attIDs, true);
            
            return mVis;
        }
    }
}