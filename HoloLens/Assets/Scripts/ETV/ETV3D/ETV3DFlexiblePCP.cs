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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public class ETV3DFlexiblePCP : AETV
    {
        // Hook
        private APCPLineGenerator pcpLineGenerator;

        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

        private IDictionary<string, AAxis> axes;

        public string attributeA;
        public string attributeB;
        
        
        public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs, AAxis axisA, AAxis axisB, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.pcpLineGenerator = new PCP3DLineGenerator();

            this.attributeA = axisA.attributeName;
            this.attributeB = axisB.attributeName;

            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;

            axes = new Dictionary<string, AAxis>();
            axes.Add(axisA.attributeName, axisA);
            axes.Add(axisB.attributeName, axisB);
        }

        public override void DrawGraph()
        {
            foreach(var infO in Data.infoObjects)
            {
                var line = pcpLineGenerator.CreateLine(
                    infO, 
                    Data.colorTable[infO], 
                    Data.colorTableBrushing[infO], 
                    Data, 
                    nominalIDs, ordinalIDs, intervalIDs, ratioIDs, 
                    axes, true);
                if(line != null)
                {
                    RememberRelationOf(infO, line);
                }
            }
        }

        public override void UpdateETV()
        {
            foreach(var key in infoObject2primitive.Keys)
            {
                if(infoObject2primitive[key] != null && infoObject2primitive[key].gameObject != null)
                APCPLineGenerator.UpdatePolyline((APCPLine)infoObject2primitive[key], axes, true);
            }
        }

        public override void SetVisibility(bool visible)
        {
            base.SetVisibility(visible);

            try
            {
                foreach(var key in infoObject2primitive.Keys)
                {
                    ((APCPLine)infoObject2primitive[key]).LR.enabled = visible;
                }
            } catch(Exception e)
            {
                Debug.Log(e.Message + "Already destroyed");
            }
        }





        // .................................................................... Useless in this MetaVis
        public override void SetUpAxes()
        {
            // A flexible axes PCP doesn't need it's own axes.
            // Nothing to do here.
        }
    }
}