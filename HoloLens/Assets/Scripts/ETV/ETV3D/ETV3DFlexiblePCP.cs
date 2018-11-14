using GraphicalPrimitive;
using Model;
using System.Collections.Generic;

namespace ETV
{
    public class ETV3DFlexiblePCP : MetaVis
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

            axisA.Subscribe(this);
            axisB.Subscribe(this);
        }

        public override void DrawGraph()
        {
            foreach(var infoO in Data.infoObjects)
            {
                var line = pcpLineGenerator.CreateLine(infoO, Data.colorTable[infoO], Data, nominalIDs, ordinalIDs, intervalIDs, ratioIDs, axes, true);
                if(line != null)
                {
                    RememberRelationOf(infoO, line);
                }
            }
        }

        public override void UpdateETV()
        {
            foreach(var key in infoObject2primitive.Keys)
            {
                APCPLineGenerator.UpdatePolyline((APCPLine)infoObject2primitive[key], axes, true);
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