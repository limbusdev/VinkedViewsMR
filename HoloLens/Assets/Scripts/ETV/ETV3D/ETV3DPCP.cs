using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public class ETV3DPCP : AETV3D
    {
        // Hook
        private APCPLineGenerator pcpLineGenerator;

        float accordionLength;
        
        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

        private IDictionary<int, AAxis> PCPAxesFront, PCPAxesBack;

        private APCPLine[] lines;

        public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs)
        {
            base.Init(data);
            this.pcpLineGenerator = new PCP3DLineGenerator();

            this.data = data;
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
            PCPAxesFront = new Dictionary<int, AAxis>();
            PCPAxesBack = new Dictionary<int, AAxis>();

            GameObject axesFront = GenerateAxes(true, PCPAxesFront);
            axesFront.transform.parent = Anchor.transform;

            GameObject axesBack = GenerateAxes(false, PCPAxesBack);
            axesBack.transform.parent = Anchor.transform;
            axesBack.transform.localPosition = new Vector3(0,0, data.infoObjects.Count * accordionLength);
        }

        private GameObject GenerateAxes(bool withGrid, IDictionary<int, AAxis> axes)
        {
            AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

            int counter = 0;
            GameObject allAxesGO = new GameObject("Axes-Set");

            // Setup nominal axes
            foreach(int attID in nominalIDs)
            {
                string attributeName = data.nomAttribNames[attID];
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
                string attributeName = data.ordAttribNames[attID];
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
                string attributeName = data.ivlAttribNames[attID];
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
                string attributeName = data.ratAttribNames[attID];
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
        

        public override void DrawGraph()
        {
            var notNaNPrimitives = new List<APCPLine>();

            int counter = 0;
            foreach(var infoO in data.infoObjects)
            {
                float zOffset = counter * accordionLength - .005f;
                var line = pcpLineGenerator.CreateLine(
                    infoO,
                    Color.white, 
                    data, 
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
                    notNaNPrimitives.Add(line);
                }
                counter++;
            }

            lines = notNaNPrimitives.ToArray();
        }


        private GameObject GenerateGrid(float min, float max)
        {
            var factory2D = ServiceLocator.instance.Factory2DPrimitives;

                GameObject grid = factory2D.CreateGrid(
                    new Color(1, 1, 1, .5f),
                    Vector3.forward,
                    Vector3.up,
                    data.infoObjects.Count * accordionLength,
                    .005f,
                    min,
                    max
                    );
                grid.transform.localPosition = Vector3.zero;

            return grid;
        }

        public override void UpdateETV()
        {
            SetUpAxes();
            DrawGraph();
        }
        
        public override void ChangeColoringScheme(ETVColorSchemes scheme)
        {
            switch(scheme)
            {
                case ETVColorSchemes.Rainbow:
                    for(int i = 0; i < lines.Length; i++)
                    {
                        Color color = Color.HSVToRGB(((float)i) / lines.Length, 1, 1);
                        lines[i].SetColor(color);
                        lines[i].ApplyColor(color);
                    }
                    break;
                case ETVColorSchemes.SplitHSV:
                    for(int i = 0; i < lines.Length; i++)
                    {
                        Color color = Color.HSVToRGB((((float)i) / lines.Length) / 2f + .5f, 1, 1);
                        lines[i].SetColor(color);
                        lines[i].ApplyColor(color);
                    }
                    break;
                default:
                    break;
            }
        }

        public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
        {
            return ServiceLocator.instance.Factory2DPrimitives;
        }
    }
}
