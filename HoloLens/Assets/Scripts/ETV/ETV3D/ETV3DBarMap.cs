using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;
using System;

/// <summary>
/// 3D Euclidean transformable view: 3D Bar Map
/// 
/// A 3D bar chart, which visualizes two nominal attributes and their
/// distribution.
/// </summary>
namespace ETV
{
    public class ETV3DBarMap : AETVHeatMap
    {
        // ........................................................................ Populate in Editor

        // ........................................................................ Private properties

        private string attributeNameA, attributeNameB;
        int attributeIDA, attributeIDB;
        private LoM lomA, lomB;
        string[] uniqueValsA, uniqueValsB;
        int[] distributionA, distributionB;
        int dimA, dimB;

        private Bar3D[,] bars;

        private int[,] absMapValues;
        private float[,] barHeights;
        private IDictionary<int, string> dictA1, dictB1;
        private IDictionary<string, int> dictA2, dictB2;

        float max;
        private float lengthA, lengthB;


        private IDictionary<AxisDirection, GameObject> axis;



        // ........................................................................ Initializers
        public override void Init(DataSet data, string attributeNameX, string attributeNameY, float lengthA=1f, float lengthB=1f, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.attributeNameA = attributeNameX;
            this.attributeNameB = attributeNameY;
            this.attributeIDA = data.IDOf(attributeNameX);
            this.attributeIDB = data.IDOf(attributeNameY);
            this.lomA = data.TypeOf(attributeNameX);
            this.lomB = data.TypeOf(attributeNameY);
            this.max = 0;
            this.lengthA = lengthA;
            this.lengthB = lengthB;

            dictA1 = new Dictionary<int, string>();
            dictB1 = new Dictionary<int, string>();
            dictA2 = new Dictionary<string, int>();
            dictB2 = new Dictionary<string, int>();

            switch(lomA)
            {
                case LoM.NOMINAL:
                    var mN = data.nominalStatistics[attributeNameX];
                    uniqueValsA = mN.uniqueValues;
                    distributionA = mN.GetDistributionValues();
                    dictA1 = mN.idValues;
                    dictA2 = mN.valueIDs;
                    break;
                case LoM.ORDINAL:
                    var mO = data.ordinalStatistics[attributeNameX];
                    uniqueValsA = mO.uniqueValues;
                    distributionA = mO.GetDistributionValues();
                    dictA1 = mO.orderedValueIDs;
                    dictA2 = mO.orderedIDValues;
                    break;
                default:
                    throw new Exception("Illegal LoM");
            }

            switch(lomB)
            {
                case LoM.NOMINAL:
                    var mN = data.nominalStatistics[attributeNameY];
                    uniqueValsB = mN.uniqueValues;
                    distributionB = mN.GetDistributionValues();
                    dictB1 = mN.idValues;
                    dictB2 = mN.valueIDs;
                    break;
                case LoM.ORDINAL:
                    var mO = data.ordinalStatistics[attributeNameY];
                    uniqueValsB = mO.uniqueValues;
                    distributionB = mO.GetDistributionValues();
                    dictB1 = mO.orderedValueIDs;
                    dictB2 = mO.orderedIDValues;
                    break;
                default:
                    throw new Exception("Illegal LoM");
            }

            dimA = uniqueValsA.Length;
            dimB = uniqueValsB.Length;

            this.absMapValues = new int[dimA, dimB];
            this.barHeights = new float[dimA, dimB];

            // For every possible value of attribute 1
            for(int vID1 = 0; vID1 < dimA; vID1++)
            {
                string v1 = uniqueValsA[vID1];

                // For every possible value of attribute 2
                for(int vID2 = 0; vID2 < dimB; vID2++)
                {
                    string v2 = uniqueValsB[vID2];

                    // Count how many object match both values
                    int count;

                    if(lomA == LoM.NOMINAL && lomB == LoM.NOMINAL)
                    {
                        count = AttributeProcessor.Nominal.CountObjectsMatchingTwoCattegories(
                            data.infoObjects, attributeIDA, attributeIDB, v1, v2);

                    } else if(lomA == LoM.ORDINAL && lomB == LoM.ORDINAL)
                    {
                        count = AttributeProcessor.Ordinal.CountObjectsMatchingTwoCattegories(
                            data.infoObjects, attributeIDA, attributeIDB, dictA2[v1], dictB2[v2]);

                    } else if(lomA == LoM.NOMINAL && lomB == LoM.ORDINAL)
                    {
                        count = AttributeProcessor.Categorial.CountObjectsMatchingTwoCategoriesNomOrd(
                            data.infoObjects, attributeNameX, attributeNameY, v1, dictB2[v2]);

                    } else if(lomA == LoM.ORDINAL && lomB == LoM.NOMINAL)
                    {
                        count = AttributeProcessor.Categorial.CountObjectsMatchingTwoCategoriesNomOrd(
                            data.infoObjects, attributeNameY, attributeNameX, v2, dictA2[v1]);

                    } else
                    {
                        count = 0;
                    }


                    if(count > max)
                    {
                        max = count;
                    }

                    absMapValues[vID1, vID2] = count;
                }
            }

            for(int vID1 = 0; vID1 < dimA; vID1++)
            {
                for(int vID2 = 0; vID2 < dimB; vID2++)
                {
                    barHeights[vID1, vID2] = absMapValues[vID1, vID2] / ((float)max);
                }
            }

            if(max > 0)
            {
                SetUpAxes();
                DrawGraph();

                foreach(var o in data.infoObjects)
                {
                    bool valAMissing = data.IsValueMissing(o, attributeNameA);
                    bool valBMissing = data.IsValueMissing(o, attributeNameB);

                    if(!(valAMissing || valBMissing))
                    {
                        Bar3D bar;

                        if(lomA == LoM.NOMINAL && lomB == LoM.NOMINAL)
                        {
                            bar = bars[
                                dictA2[o.NomValueOf(attributeNameA)],
                                dictB2[o.NomValueOf(attributeNameB)]
                                ];

                        } else if(lomA == LoM.ORDINAL && lomB == LoM.ORDINAL)
                        {
                            bar = bars[
                                o.OrdValueOf(attributeNameA),
                                o.OrdValueOf(attributeNameB)
                                ];

                        } else if(lomA == LoM.NOMINAL && lomB == LoM.ORDINAL)
                        {
                            bar = bars[
                                dictA2[o.NomValueOf(attributeNameA)],
                                o.OrdValueOf(attributeNameB)
                                ];

                        } else if(lomA == LoM.ORDINAL && lomB == LoM.NOMINAL)
                        {
                            bar = bars[
                                o.OrdValueOf(attributeNameA),
                                dictB2[o.NomValueOf(attributeNameB)]
                                ];

                        } else
                        {
                            bar = new Bar3D();
                        }

                        RememberRelationOf(o, bar);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the graph by using values calculated in the Init() method
        /// </summary>
        public override void DrawGraph()
        {
            float barGap = .01f;
            float gapWidthA = (dimA - 1) * barGap;
            float gapWidthB = (dimB - 1) * barGap;
            float barWidthA = (lengthA-gapWidthA-.1f) / (dimA);
            float barWidthB = (lengthB-gapWidthB-.1f) / (dimB);


            bars = new Bar3D[dimA, dimB];

            float marginA = .05f + barWidthA/2;
            float marginB = .05f + barWidthB/2;

            for(int i = 0; i < dimA; i++)
            {
                for(int ii = 0; ii < dimB; ii++)
                {
                    bars[i, ii] = CreateBar(barHeights[i, ii], max, barWidthA, barWidthB);
                    bars[i, ii].SetLabelText(absMapValues[i, ii].ToString());

                    // Set bar's position
                    var barGO = bars[i, ii].gameObject;
                    barGO.transform.localPosition = new Vector3(
                        marginA + i * (barWidthA+barGap), 
                        0, 
                        marginB + ii * (barWidthB+barGap));
                    barGO.transform.parent = Anchor.transform;
                }
            }
        }

        /// <summary>
        /// Generates axes from the calculated values.
        /// </summary>
        public override void SetUpAxes()
        {
            var factory2D = Services.PrimFactory2D();

            // Categorical Axis A
            AddAxis(attributeNameA, AxisDirection.X, lengthA);
            var xAxis = GetAxis(AxisDirection.X);
            xAxis.transform.parent = Anchor.transform;
            xAxis.transform.localRotation = Quaternion.Euler(90, 0, 0);

            // Categorical Axis B
            AddAxis(attributeNameB, AxisDirection.Z, lengthB);
            
            var yAxis = factory2D.CreateAutoTickedAxis("Amount", max);
            yAxis.transform.parent = Anchor;
        }

        /**
         * Creates a colored bar. 
         * @param range         maximum - minimum value of this attribute
         * @param attributeID   which attribute
         * */
        private Bar3D CreateBar(float value, float range, float widthA=.1f, float widthB=.1f)
        {
            var factory3D = Services.instance.Factory3DPrimitives;

            Bar3D bar = factory3D.CreateBar(value, widthA, widthB).GetComponent<Bar3D>();
            bar.Assign(this);

            bar.SetLabelText(value.ToString());

            return bar;
        }



        public override void ChangeColoringScheme(ETVColorSchemes scheme)
        {
            switch(scheme)
            {
                default: // case SplitHSV
                    float H = 0f;
                    for(int row = 0; row < dimA; row++)
                    {
                        float S = 0f;
                        for(int col = 0; col < dimB; col++)
                        {
                            var color = Color.HSVToRGB(
                                (H / dimA) / 2f + .5f,
                                (S / dimB) / 2f + .5f,
                                1);
                            bars[row, col].SetColor(color, Color.green);
                            S++;
                        }
                        H++;
                    }
                    break;
            }
        }

        public override void UpdateETV()
        {

        }
    }
}
