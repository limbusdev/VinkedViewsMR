using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicalPrimitive;
using Model;
using Model.Attributes;
using UnityEngine;

namespace ETV.ETV3D
{
    public class ETV3DParallelCoordinatesPlot : AETV3D
    {
        public GameObject Anchor;

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
            this.data = data;
            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;

            this.linePrimitives = new PCPLine2D[data.informationObjects.Count];


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
            int dimension = ratioIDs.Length + nominalIDs.Length;

            int i = 0;
            foreach(InfoObject o in data.informationObjects)
            {
                linePrimitives[i] = CreateLine(o, Color.white);
                linePrimitives[i].transform.localPosition = new Vector3(0, 0, i * pcpLength - .002f);
                i++;
            }
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
            var replacement = (new GameObject("Information object contained NaN")).AddComponent<PCPLine2D>();

            Graphical2DPrimitiveFactory factory = ServiceLocator.instance.Factory2DPrimitives;
            var pcpLine = factory.CreatePCPLine();
            var pcpComp = pcpLine.GetComponent<PCPLine2D>();
            pcpComp.lineRenderer.startColor = color;
            pcpComp.lineRenderer.endColor = color;
            pcpComp.lineRenderer.startWidth = 0.02f;
            pcpComp.lineRenderer.endWidth = 0.02f;
            int dimension = ratioIDs.Length + nominalIDs.Length + ordinalIDs.Length + intervalIDs.Length;
            pcpComp.lineRenderer.positionCount = dimension;

            // Assemble Polyline
            Vector3[] polyline = new Vector3[dimension];

            int counter = 0;
            foreach(int attID in nominalIDs)
            {
                var m = data.dataMeasuresNominal[data.nomAttributes[attID]];
                var a = o.nominalAtt[attID];

                polyline[counter] = new Vector3(.5f * counter, PCPAxesFront[counter].TransformToAxisSpace(m.valueIDs[a.value]), 0);
                o.AddRepresentativeObject(a.name, pcpLine);
                counter++;
            }

            foreach(var attID in ordinalIDs)
            {
                var m = data.dataMeasuresOrdinal[data.ordAttributes[attID]];
                var a = o.ordinalAtt[attID];

                // If NaN
                if(a.value == int.MinValue)
                {
                    return replacement;
                }

                polyline[counter] = new Vector3(.5f * counter, PCPAxesFront[counter].TransformToAxisSpace(a.value), 0);
                o.AddRepresentativeObject(a.name, pcpLine);
                counter++;
            }

            foreach(var attID in intervalIDs)
            {
                var m = data.dataMeasuresInterval[data.ivlAttributes[attID]];
                var a = o.intervalAtt[attID];

                // If NaN
                if(a.value == int.MinValue)
                {
                    return replacement;
                }

                polyline[counter] = new Vector3(.5f * counter, PCPAxesFront[counter].TransformToAxisSpace(a.value), 0);
                o.AddRepresentativeObject(a.name, pcpLine);
                counter++;
            }

            foreach(var attID in ratioIDs)
            {
                var m = data.dataMeasuresRatio[data.ratAttributes[attID]];
                var a = o.ratioAtt[attID];

                // If NaN
                if(float.IsNaN(a.value))
                {
                    return replacement;
                }

                polyline[counter] = new Vector3(.5f * counter, PCPAxesFront[counter].TransformToAxisSpace(a.value), 0);
                o.AddRepresentativeObject(a.name, pcpLine);
                counter++;
            }

            pcpComp.visBridgePort.transform.localPosition = polyline[0];
            pcpComp.lineRenderer.SetPositions(polyline);
            pcpLine.transform.parent = Anchor.transform;

            return pcpComp;
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
