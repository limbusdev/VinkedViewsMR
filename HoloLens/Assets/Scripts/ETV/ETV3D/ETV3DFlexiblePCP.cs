using GraphicalPrimitive;
using Model;
using System.Collections.Generic;

namespace ETV
{
    public class ETV3DFlexiblePCP : AETV, IGPObserver<AAxis>
    {
        // Hook
        private APCPLineGenerator pcpLineGenerator;

        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;
        
        private AAxis axisA, axisB;
        
        
        public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs, AAxis axisA, AAxis axisB, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.pcpLineGenerator = new PCP3DLineGenerator();

            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;

            this.axisA = axisA;
            this.axisB = axisB;

            axisA.Subscribe(this);
            axisB.Subscribe(this);
        }

        public override void DrawGraph()
        {
            var axes = new Dictionary<int, AAxis>();
            axes.Add(0, axisA);
            axes.Add(1, axisB);
            
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
            if(disposed)
                return;

            var axes = new Dictionary<int, AAxis>();
            axes.Add(0, axisA);
            axes.Add(1, axisB);

            foreach(var key in infoObject2primitive.Keys)
            {
                APCPLineGenerator.UpdatePolyline((APCPLine)infoObject2primitive[key], axes, true);
            }
        }
        

        public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
        {
            return ServiceLocator.PrimitivePlant2D();
        }
        

        // .................................................................... Useless in this MetaVis
        public override void SetUpAxes() { /*Unneccessary*/ }

        public void OnDispose(AAxis observable)
        {
            // do Nothing
        }

        public void Notify(AAxis observable)
        {
            UpdateETV();
        }


        // .................................................................... IObserver

    }
}