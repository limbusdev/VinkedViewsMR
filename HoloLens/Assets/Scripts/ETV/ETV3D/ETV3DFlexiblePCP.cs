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
                Debug.Log("Already destroyed");
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