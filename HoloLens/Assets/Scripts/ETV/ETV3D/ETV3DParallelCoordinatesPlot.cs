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

        private DataSet data;
        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;
        

        private PCPLine2D[] linePrimitives;

        public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs)
        {
            this.data = data;
            this.nominalIDs = nominalIDs;
            this.ordinalIDs = ordinalIDs;
            this.intervalIDs = intervalIDs;
            this.ratioIDs = ratioIDs;

            this.linePrimitives = new PCPLine2D[data.informationObjects.Count];

            SetUpAxis();
            DrawGraph();
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

        public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
        {
            throw new NotImplementedException();
        }

        public override void SetUpAxis()
        {
            GameObject axesFront = GenerateAxes(true);
            axesFront.transform.parent = Anchor.transform;

            GameObject axesBack = GenerateAxes(false);
            axesBack.transform.parent = Anchor.transform;
            axesBack.transform.localPosition = new Vector3(0,0, data.informationObjects.Count * .1f);
        }

        public void DrawGraph()
        {
            int i = 0;
            foreach(InformationObject o in data.informationObjects)
            {
                //GameObject newAxes = Instantiate(allAxesGO);
                //newAxes.transform.localPosition = new Vector3(0, 0, i * .25f - .002f);
                linePrimitives[i] = CreateLine(o, Color.white);
                linePrimitives[i].transform.parent = Anchor.transform;
                linePrimitives[i].transform.localPosition = new Vector3(0,0,i*.1f-.002f);
                //newAxes.transform.parent = Anchor.transform;
                //newAxes.SetActive(true);
                i++;
            }
        }

        private GameObject GenerateAxes(bool withGrid)
        {
            AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

            int counter = 0;
            GameObject allAxesGO = new GameObject("Axes-Set");
            //allAxesGO.SetActive(false);

            // Setup nominal axes
            for(int i = 0; i < nominalIDs.Length; i++)
            {
                string attributeName = data.informationObjects[0].nominalAtt[nominalIDs[i]].name;
                GameObject axis = factory2D.CreateAxis(
                    Color.white, attributeName, "", AxisDirection.Y, 1f, .01f, true, true);
                axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axis.transform.parent = allAxesGO.transform;
                Axis2D axis2D = axis.GetComponent<Axis2D>();
                axis2D.ticked = true;
                axis2D.min = 0;
                axis2D.max = data.dataMeasuresNominal[attributeName].numberOfUniqueValues;

                axis2D.CalculateTickResolution();
                axis2D.UpdateAxis();

                if(withGrid)
                {
                    var grid = GenerateGrid(axis2D.min, axis2D.max);
                    grid.transform.parent = axis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }


                counter++;
            }

            // Setup ordinal axes
            for(int i = 0; i < ordinalIDs.Length; i++)
            {
                string attributeName = data.informationObjects[0].ordinalAtt[ordinalIDs[i]].name;
                GameObject axis = factory2D.CreateAxis(
                    Color.white, attributeName, "", AxisDirection.Y, 1f, .01f, true, true);
                axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axis.transform.parent = allAxesGO.transform;
                Axis2D axis2D = axis.GetComponent<Axis2D>();
                axis2D.ticked = true;
                axis2D.min = data.dataMeasuresOrdinal[attributeName].min;
                axis2D.max = data.dataMeasuresOrdinal[attributeName].max;

                axis2D.CalculateTickResolution();
                axis2D.UpdateAxis();

                if(withGrid)
                {
                    var grid = GenerateGrid(axis2D.min, axis2D.max);
                    grid.transform.parent = axis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                counter++;
            }

            // Setup interval axes
            for(int i = 0; i < intervalIDs.Length; i++)
            {
                string attributeName = data.informationObjects[0].intervalAtt[intervalIDs[i]].name;
                GameObject axis = factory2D.CreateAxis(
                    Color.white, attributeName, "", AxisDirection.Y, 1f, .01f, true, true);
                axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axis.transform.parent = allAxesGO.transform;
                Axis2D axis2D = axis.GetComponent<Axis2D>();
                axis2D.ticked = true;
                axis2D.min = data.dataMeasuresInterval[attributeName].min;
                axis2D.max = data.dataMeasuresInterval[attributeName].max;

                axis2D.CalculateTickResolution();
                axis2D.UpdateAxis();

                if(withGrid)
                {
                    var grid = GenerateGrid(axis2D.min, axis2D.max);
                    grid.transform.parent = axis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                counter++;
            }


            // Setup ratio axes
            for(int i = 0; i < ratioIDs.Length; i++)
            {
                string attributeName = data.informationObjects[0].ratioAtt[ratioIDs[i]].name;
                string attributeUnit = "";
                GameObject axis = factory2D.CreateAxis(
                    Color.white, attributeName, attributeUnit,
                    AxisDirection.Y, 1f, .01f, true, true);

                axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
                axis.transform.parent = allAxesGO.transform;
                Axis2D axis2D = axis.GetComponent<Axis2D>();
                axis2D.ticked = true;
                axis2D.min = data.dataMeasuresRatio[attributeName].zeroBoundMin;
                axis2D.max = data.dataMeasuresRatio[attributeName].zeroBoundMax;

                axis2D.CalculateTickResolution();
                axis2D.UpdateAxis();

                if(withGrid)
                {
                    var grid = GenerateGrid(axis2D.min, axis2D.max);
                    grid.transform.parent = axis.transform;
                    grid.transform.localPosition = Vector3.zero;
                }

                counter++;
            }

            allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);

            return allAxesGO;
        }

        private GameObject GenerateGrid(float min, float max)
        {
            AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

                GameObject grid = factory2D.CreateGrid(
                    new Color(1, 1, 1, .5f),
                    Vector3.forward,
                    Vector3.up,
                    data.informationObjects.Count * .1f,
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

        private PCPLine2D CreateLine(InformationObject o, Color color)
        {
            Graphical2DPrimitiveFactory factory = ServiceLocator.instance.PrimitiveFactory2Dservice;
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
            for(int variable = 0; variable < nominalIDs.Length; variable++)
            {
                GenericAttribute<string> attribute = o.nominalAtt[variable];
                polyline[counter] = new Vector3(.5f * counter,
                    ((float)data.dataMeasuresNominal[attribute.name].valueIDs[attribute.value]) /
                    data.dataMeasuresNominal[attribute.name].numberOfUniqueValues, 0);
                o.AddRepresentativeObject(attribute.name, pcpLine);
                counter++;
            }

            for(int variable = 0; variable < ordinalIDs.Length; variable++)
            {
                GenericAttribute<int> attribute = o.ordinalAtt[variable];
                polyline[counter] = new Vector3(.5f * counter, ((float)attribute.value - data.dataMeasuresInterval[attribute.name].min) / data.dataMeasuresOrdinal[attribute.name].range, 0);
                o.AddRepresentativeObject(attribute.name, pcpLine);
                counter++;
            }

            for(int variable = 0; variable < intervalIDs.Length; variable++)
            {
                GenericAttribute<int> attribute = o.intervalAtt[variable];
                polyline[counter] = new Vector3(.5f * counter, ((float)attribute.value - data.dataMeasuresInterval[attribute.name].min) / data.dataMeasuresInterval[attribute.name].range, 0);
                o.AddRepresentativeObject(attribute.name, pcpLine);
                counter++;
            }

            for(int variable = 0; variable < ratioIDs.Length; variable++)
            {
                GenericAttribute<float> attribute = o.ratioAtt[variable];
                polyline[counter] = new Vector3(.5f * counter, attribute.value / data.dataMeasuresRatio[attribute.name].zeroBoundRange, 0);
                o.AddRepresentativeObject(attribute.name, pcpLine);
                counter++;
            }

            pcpComp.visBridgePort.transform.localPosition = polyline[0];
            pcpComp.lineRenderer.SetPositions(polyline);
            pcpLine.transform.parent = Anchor.transform;

            return pcpComp;
        }
    }
}
