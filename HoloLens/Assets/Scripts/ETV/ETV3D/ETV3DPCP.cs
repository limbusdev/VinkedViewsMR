using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public class ETV3DPCP : AETVPCP
    {
        // ........................................................................ PARAMETERS
        // Hook
        private APCPLineGenerator pcpLineGenerator;

        float accordionLength;
        
        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

        private IDictionary<string, AAxis> PCPAxesFront, PCPAxesBack;
        
        // ........................................................................ CONSTRUCTOR / INITIALIZER

        public override void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.pcpLineGenerator = new PCP3DLineGenerator();

            this.Data = data;
            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;
            
            int numberOfObjects = data.infoObjects.Count;
            if(numberOfObjects > 20)
                accordionLength = 2f / numberOfObjects;
            else if(numberOfObjects > 100)
                accordionLength = 3f / numberOfObjects;
            else if(numberOfObjects > 1000)
                accordionLength = 5f / numberOfObjects;
            else
                accordionLength = .1f;

            SetUpAxes();
            DrawGraph();
        }

        
        
        public override void SetUpAxes()
        {
            PCPAxesFront = new Dictionary<string, AAxis>();
            PCPAxesBack = new Dictionary<string, AAxis>();

            var axesFront = GenerateAxes(true, PCPAxesFront);
            axesFront.transform.parent = Anchor.transform;

            var axesBack = GenerateAxes(false, PCPAxesBack);
            axesBack.transform.parent = Anchor.transform;
            axesBack.transform.localPosition = new Vector3(0,0, Data.infoObjects.Count * accordionLength);
        }

        private GameObject GenerateAxes(bool withGrid, IDictionary<string, AAxis> axes)
        {
            AGraphicalPrimitiveFactory factory2D = Services.instance.Factory2DPrimitives;

            int counter = 0;
            GameObject allAxesGO = new GameObject("Axes-Set");

            // Setup nominal axes
            foreach(int attID in nominalIDs)
            {
                string attributeName = Data.nomAttribNames[attID];
                var xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(Data.nomAttribNames[attID], xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                RegisterAxis(xAxis.GetComponent<AAxis>());
                counter++;
            }

            // Setup ordinal axes
            foreach(int attID in ordinalIDs)
            {
                string attributeName = Data.ordAttribNames[attID];
                var xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(Data.ordAttribNames[attID], xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                RegisterAxis(xAxis.GetComponent<AAxis>());
                counter++;
            }

            // Setup interval axes
            foreach(int attID in intervalIDs)
            {
                string attributeName = Data.ivlAttribNames[attID];
                var xAxis = factory2D.CreateAutoTickedAxis(attributeName, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(Data.ivlAttribNames[attID], xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                RegisterAxis(xAxis.GetComponent<AAxis>());
                counter++;
            }


            // Setup ratio axes
            foreach(int attID in ratioIDs)
            {
                string attributeName = Data.ratAttribNames[attID];
                var xAxis = factory2D.CreateAutoTickedAxis(attributeName, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(Data.ratAttribNames[attID], xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                RegisterAxis(xAxis.GetComponent<AAxis>());
                counter++;
            }

            allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);

            return allAxesGO;
        }


        // ........................................................................ DRAW CALLS

        public override void DrawGraph()
        {
            int counter = 0;
            foreach(var infO in Data.infoObjects)
            {
                float zOffset = counter * accordionLength - .005f;
                var line = pcpLineGenerator.CreateLine(
                    infO,
                    Data.colorTable[infO], 
                    Data, 
                    nominalIDs, 
                    ordinalIDs, 
                    intervalIDs, 
                    ratioIDs, 
                    PCPAxesFront, 
                    false, 
                    zOffset);
                if(line != null)
                {
                    line.transform.parent = Anchor.transform;
                    RememberRelationOf(infO, line);
                }
                counter++;
            }
        }


        private GameObject GenerateGrid(float min, float max)
        {
            var factory2D = Services.PrimFactory2D();

                GameObject grid = factory2D.CreateGrid(
                    new Color(1, 1, 1, .5f),
                    Vector3.forward,
                    Vector3.up,
                    Data.infoObjects.Count * accordionLength,
                    .005f,
                    min,
                    max
                    );
                grid.transform.localPosition = Vector3.zero;

            return grid;
        }

        public override void UpdateETV()
        {
            
        }
    }
}
