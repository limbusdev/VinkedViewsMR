﻿using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public class ETV2DPCP : AETVPCP
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

        private IDictionary<string, AAxis> PCPAxes;
       

        // ........................................................................ CONSTRUCTOR / INITIALIZER

        public override void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
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
            PCPAxes = new Dictionary<string, AAxis>();
            var factory2D = Services.PrimFactory2D();

            int counter = 0;
            allAxesGO = new GameObject("Axes-Set");
            string attName;
            GameObject xAxis;
            var offset = .5f;

            // Setup nominal axes
            foreach(int attID in nominalIDs)
            {
                attName = Data.nomAttribNames[attID];
                xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attName, 1f, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(Data.nomAttribNames[attID], xAxis.GetComponent<Axis2D>());
                RegisterAxis(xAxis.GetComponent<AAxis>());

                counter++;
            }

            // Setup ordinal axes
            foreach(int attID in ordinalIDs)
            {
                attName = Data.ordAttribNames[attID];
                xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attName, 1f, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(Data.ordAttribNames[attID], xAxis.GetComponent<Axis2D>());
                RegisterAxis(xAxis.GetComponent<AAxis>());

                counter++;
            }

            // Setup interval axes
            foreach(int attID in intervalIDs)
            {
                attName = Data.ivlAttribNames[attID];
                xAxis = factory2D.CreateAutoTickedAxis(attName, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(Data.ivlAttribNames[attID], xAxis.GetComponent<Axis2D>());
                RegisterAxis(xAxis.GetComponent<AAxis>());

                counter++;
            }


            // Setup ratio axes
            foreach(int attID in ratioIDs)
            {
                attName = Data.ratAttribNames[attID];
                xAxis = factory2D.CreateAutoTickedAxis(attName, AxisDirection.Y, Data);
                xAxis.transform.parent = allAxesGO.transform;
                xAxis.transform.localPosition = new Vector3(offset * counter, 0, 0);
                PCPAxes.Add(Data.ratAttribNames[attID], xAxis.GetComponent<Axis2D>());
                RegisterAxis(xAxis.GetComponent<AAxis>());

                counter++;
            }

            allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);
            allAxesGO.transform.parent = Anchor.transform;
        }


        // ........................................................................ DRAW CALLS

        public override void DrawGraph()
        {
            int counter = 0;
            foreach(var infO in Data.infoObjects)
            {
                var line = pcpLineGen.CreateLine(
                    infO,
                    Data.colorTable[infO],
                    Data,
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
                    RememberRelationOf(infO, line);
                }
                counter++;
            }
        }
        
        public override void UpdateETV()
        {
            // Nothing
        }
    }
}