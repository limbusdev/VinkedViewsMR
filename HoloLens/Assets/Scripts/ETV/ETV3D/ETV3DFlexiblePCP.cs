using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public class ETV3DFlexiblePCP : AETV3D
    {
        // Hook
        private IPCPLineGenerator pcpLineGenerator;

        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

        private PCPLine2D[] linePrimitives;

        private AAxis axisA, axisB;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateETV();
        }

        public override void UpdateETV()
        {

        }

        public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs, AAxis axisA, AAxis axisB)
        {
            base.Init(data);
            this.pcpLineGenerator = new PCP2DLineGenerator();

            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;

            this.axisA = axisA;
            this.axisB = axisB;
        }

        public override void DrawGraph()
        {
            var notNaNPrimitives = new List<PCPLine2D>();
            var axes = new Dictionary<int, AAxis>();
            axes.Add(0, axisA);
            axes.Add(1, axisB);

            int counter = 0;
            foreach(var infoO in data.infoObjects)
            {
                var line = pcpLineGenerator.CreateLine(infoO, Color.white, data, nominalIDs, ordinalIDs, intervalIDs, ratioIDs, axes, true, LineAlignment.View);
                if(line != null)
                {
                    notNaNPrimitives.Add(line);
                }
                counter++;
            }

            linePrimitives = notNaNPrimitives.ToArray();
        }

        public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
        {
            return ServiceLocator.instance.Factory2DPrimitives;
        }

        

        // .................................................................... Useless in this MetaVis
        public override void SetUpAxis() { /*Unneccessary*/ }
        public override void ChangeColoringScheme(ETVColorSchemes scheme) { /*Unneccessary*/ }
    }
}