using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV.ETV3D
{
    public class ETV3DParallelCoordinatesPlot : AETV3D
    {
        public GameObject Anchor;

        // Hook
        private IPCPLineGenerator pcpLineGenerator;

        float pcpLength;

        private DataSet data;
        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

        private IDictionary<int, Axis2D> PCPAxesFront, PCPAxesBack;

        private PCPLine2D[] linePrimitives;

        public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs)
        {
            this.pcpLineGenerator = new PCP2DLineGenerator();

            this.data = data;
            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;
            
            int numberOfObjects = data.informationObjects.Count;
            if(numberOfObjects > 20)
                pcpLength = 2f / numberOfObjects;
            else if(numberOfObjects > 100)
                pcpLength = 3f / numberOfObjects;
            else if(numberOfObjects > 1000)
                pcpLength = 5f / numberOfObjects;
            else
                pcpLength = .1f;

            SetUpAxis();
            DrawGraph();
        }

        
        
        public override void SetUpAxis()
        {
            PCPAxesFront = new Dictionary<int, Axis2D>();
            PCPAxesBack = new Dictionary<int, Axis2D>();

            GameObject axesFront = GenerateAxes(true, PCPAxesFront);
            axesFront.transform.parent = Anchor.transform;

            GameObject axesBack = GenerateAxes(false, PCPAxesBack);
            axesBack.transform.parent = Anchor.transform;
            axesBack.transform.localPosition = new Vector3(0,0, data.informationObjects.Count * pcpLength);
        }

        private GameObject GenerateAxes(bool withGrid, IDictionary<int, Axis2D> axes)
        {
            AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

            int counter = 0;
            GameObject allAxesGO = new GameObject("Axes-Set");

            // Setup nominal axes
            foreach(int attID in nominalIDs)
            {
                string attributeName = data.nomAttributes[attID];
                var xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(counter, xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                counter++;
            }

            // Setup ordinal axes
            foreach(int attID in ordinalIDs)
            {
                string attributeName = data.ordAttributes[attID];
                var xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(counter, xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                counter++;
            }

            // Setup interval axes
            foreach(int attID in intervalIDs)
            {
                string attributeName = data.ivlAttributes[attID];
                var xAxis = factory2D.CreateAutoTickedAxis(attributeName, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(counter, xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                counter++;
            }


            // Setup ratio axes
            foreach(int attID in ratioIDs)
            {
                string attributeName = data.ratAttributes[attID];
                var xAxis = factory2D.CreateAutoTickedAxis(attributeName, AxisDirection.Y, data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axes.Add(counter, xAxis.GetComponent<Axis2D>());

                if(withGrid)
                {
                    var grid = GenerateGrid(xAxis.GetComponent<Axis2D>().min, xAxis.GetComponent<Axis2D>().max);
                    grid.transform.parent = xAxis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                counter++;
            }

            allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);

            return allAxesGO;
        }
        

        public void DrawGraph()
        {
            var notNaNPrimitives = new List<PCPLine2D>();

            int counter = 0;
            foreach(var infoO in data.informationObjects)
            {
                var line = CreateLine(infoO, Color.white);
                if(line != null)
                {
                    line.transform.localPosition = new Vector3(0, 0, counter * pcpLength - .002f);
                    notNaNPrimitives.Add(line);
                }
                counter++;
            }

            linePrimitives = notNaNPrimitives.ToArray();
        }


        private GameObject GenerateGrid(float min, float max)
        {
            AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

                GameObject grid = factory2D.CreateGrid(
                    new Color(1, 1, 1, .5f),
                    Vector3.forward,
                    Vector3.up,
                    data.informationObjects.Count * pcpLength,
                    .005f,
                    min,
                    max
                    );
                grid.transform.localPosition = Vector3.zero;

            return grid;
        }

        public override void UpdateETV()
        {
            SetUpAxis();
            DrawGraph();
        }

        private PCPLine2D CreateLine(InfoObject o, Color color)
        {
            var pcpLine = pcpLineGenerator.CreateLine(o, color, data, nominalIDs, ordinalIDs, intervalIDs, ratioIDs, PCPAxesFront);
            pcpLine.transform.parent = Anchor.transform;

            return pcpLine;
        }

        public override void ChangeColoringScheme(ETVColorSchemes scheme)
        {
            switch(scheme)
            {
                case ETVColorSchemes.Rainbow:
                    for(int i = 0; i < linePrimitives.Length; i++)
                    {
                        Color color = Color.HSVToRGB(((float)i) / linePrimitives.Length, 1, 1);
                        linePrimitives[i].SetColor(color);
                        linePrimitives[i].ApplyColor(color);
                    }
                    break;
                case ETVColorSchemes.SplitHSV:
                    for(int i = 0; i < linePrimitives.Length; i++)
                    {
                        Color color = Color.HSVToRGB((((float)i) / linePrimitives.Length) / 2f + .5f, 1, 1);
                        linePrimitives[i].SetColor(color);
                        linePrimitives[i].ApplyColor(color);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
