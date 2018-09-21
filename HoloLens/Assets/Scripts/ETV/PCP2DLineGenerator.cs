﻿using GraphicalPrimitive;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETV
{
    public class PCP2DLineGenerator : IPCPLineGenerator
    {
        public PCPLine2D CreateLine(
            InfoObject o, 
            Color color, 
            DataSet data, 
            int[] nominalIDs, 
            int[] ordinalIDs, 
            int[] intervalIDs, 
            int[] ratioIDs,
            IDictionary<int, Axis2D> axes
            )
        {
            var factory = ServiceLocator.instance.Factory2DPrimitives;
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

                polyline[counter] = new Vector3(.5f * counter, axes[counter].TransformToAxisSpace(m.valueIDs[a.value]), 0);
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
                    return null;
                }

                polyline[counter] = new Vector3(.5f * counter, axes[counter].TransformToAxisSpace(a.value), 0);
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
                    return null;
                }

                polyline[counter] = new Vector3(.5f * counter, axes[counter].TransformToAxisSpace(a.value), 0);
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
                    return null;
                }

                polyline[counter] = new Vector3(.5f * counter, axes[counter].TransformToAxisSpace(a.value), 0);
                o.AddRepresentativeObject(a.name, pcpLine);
                counter++;
            }

            pcpComp.visBridgePort.transform.localPosition = polyline[0];
            pcpComp.lineRenderer.SetPositions(polyline);

            return pcpComp;
        }
    }
}
