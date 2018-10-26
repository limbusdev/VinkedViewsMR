using ETV;
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public class ETV2DPCP : AETV2D
    {
        // ........................................................................ PARAMETERS
        // Hook
        private APCPLineGenerator pcpLineGen;

        private int[]
                nominalIDs,
                ordinalIDs,
                intervalIDs,
                ratioIDs;

        private GameObject allAxesGO;

        private IDictionary<int, AAxis> PCPAxes;

        private APCPLine[] lines;


        // ........................................................................ CONSTRUCTOR / INITIALIZER

        public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs)
        {
            base.Init(data);
            this.pcpLineGen = new PCP2DLineGenerator();

            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;

            SetUpAxes();
            DrawGraph();
        }

        public override void SetUpAxes()
        {
            PCPAxes = new Dictionary<int, AAxis>();
            AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;
            var lineGen = new PCP2DLineGenerator();

            int counter = 0;
            allAxesGO = new GameObject("Axes-Set");
            string attName;
            GameObject xAxis;
            var offset = .5f;

            // Setup nominal axes
            foreach(int attID in nominalIDs)
            {
                attName = data.nomAttribNames[attID];
                xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attName, 1f, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

                counter++;
            }

            // Setup ordinal axes
            foreach(int attID in ordinalIDs)
            {
                attName = data.ordAttribNames[attID];
                xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attName, 1f, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

                counter++;
            }

            // Setup interval axes
            foreach(int attID in intervalIDs)
            {
                attName = data.ivlAttribNames[attID];
                xAxis = factory2D.CreateAutoTickedAxis(attName, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

                counter++;
            }


            // Setup ratio axes
            foreach(int attID in ratioIDs)
            {
                attName = data.ratAttribNames[attID];
                xAxis = factory2D.CreateAutoTickedAxis(attName, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

                counter++;
            }

            allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);
            allAxesGO.transform.parent = Anchor.transform;
        }

        // ........................................................................ DRAW CALLS

        public override void DrawGraph()
        {
            var notNaNPrimitives = new List<APCPLine>();

            int counter = 0;
            foreach(var infoO in data.infoObjects)
            {
                var line = pcpLineGen.CreateLine(
                    infoO,
                    data.colorTable[infoO],
                    data,
                    nominalIDs,
                    ordinalIDs,
                    intervalIDs,
                    ratioIDs,
                    PCPAxes,
                    false,
                    counter * .0001f);
                if(line != null)
                {

                    line.transform.parent = Anchor.transform;
                    notNaNPrimitives.Add(line);
                }
                counter++;
            }

            lines = notNaNPrimitives.ToArray();
        }

        public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
        {
            return ServiceLocator.PrimitivePlant2D();
        }

        public override void UpdateETV()
        {
            // Nothing
        }
    }
}