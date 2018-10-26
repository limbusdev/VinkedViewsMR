using GraphicalPrimitive;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public class ETV3DFlexiblePCP : AETV3D, IObserver<AAxis>
    {
        // Hook
        private APCPLineGenerator pcpLineGenerator;

        private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

        private APCPLine[] linePrimitives;

        private AAxis axisA, axisB;
        

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

            axisA.Subscribe(this);
            axisB.Subscribe(this);
        }

        public override void DrawGraph()
        {
            var notNaNPrimitives = new List<APCPLine>();
            var axes = new Dictionary<int, AAxis>();
            axes.Add(0, axisA);
            axes.Add(1, axisB);

            int counter = 0;
            foreach(var infoO in data.infoObjects)
            {
                var line = pcpLineGenerator.CreateLine(infoO, Color.white, data, nominalIDs, ordinalIDs, intervalIDs, ratioIDs, axes, true);
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


        // .................................................................... Useless in this MetaVis
        public override void SetUpAxes() { /*Unneccessary*/ }


        // .................................................................... IObserver
        public void OnCompleted()
        {
            // Nothing
        }

        public void OnError(Exception error)
        {
            // Nothing
        }

        public void OnNext(AAxis value)
        {
            foreach(var line in linePrimitives)
            {
                // Remove objects from representative elements in InfoObject first
                Destroy(line.gameObject);
            }

            DrawGraph();
            ChangeColoringScheme(ETVColorSchemes.SplitHSV);
        }
    }
}