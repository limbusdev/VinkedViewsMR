using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 3D Euclidean transformable view: 3D Bar Map
/// 
/// A 3D bar chart, which visualizes two nominal attributes and their
/// distribution.
/// </summary>
public class ETV3DBarMap : AETV3D
{
    // ........................................................................ Populate in Editor
    public GameObject Anchor;

    // ........................................................................ Private properties
    private DataSet data;
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
    

    private IDictionary<AxisDirection, GameObject> axis;



    // ........................................................................ Initializers
    public void Init(DataSet data, string attributeNameX, string attributeNameY)
    {
        this.data = data;
        this.attributeNameA = attributeNameX;
        this.attributeNameB = attributeNameY;
        this.attributeIDA = data.GetIDOf(attributeNameX);
        this.attributeIDB = data.GetIDOf(attributeNameY);
        this.lomA = data.GetTypeOf(attributeNameX);
        this.lomB = data.GetTypeOf(attributeNameY);
        this.max = 0;
        
        dictA1 = new Dictionary<int, string>();
        dictB1 = new Dictionary<int, string>();
        dictA2 = new Dictionary<string, int>();
        dictB2 = new Dictionary<string, int>();
       
        switch(lomA)
        {
            case LoM.NOMINAL:
                var mN = data.nominalAttribStats[attributeNameX];
                uniqueValsA = mN.uniqueValues;
                distributionA = mN.GetDistributionValues();
                dictA1 = mN.idValues;
                dictA2 = mN.valueIDs;
                break;
            case LoM.ORDINAL:
                var mO = data.ordinalAttribStats[attributeNameX];
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
                var mN = data.nominalAttribStats[attributeNameY];
                uniqueValsB = mN.uniqueValues;
                distributionB = mN.GetDistributionValues();
                dictB1 = mN.idValues;
                dictB2 = mN.valueIDs;
                break;
            case LoM.ORDINAL:
                var mO = data.ordinalAttribStats[attributeNameY];
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

        var factory3D = ServiceLocator.instance.Factory3DPrimitives;
        
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
            SetUpAxis();
            DrawGraph();

            foreach(var o in data.infoObjects)
            {
                bool valAMissing = data.IsValueMissing(o, attributeNameA, lomA);
                bool valBMissing = data.IsValueMissing(o, attributeNameB, lomB);

                if(!(valAMissing || valBMissing))
                {
                    Bar3D bar;

                    if(lomA == LoM.NOMINAL && lomB == LoM.NOMINAL)
                    {
                        bar = bars[
                            dictA2[o.GetNomValue(attributeNameA)],
                            dictB2[o.GetNomValue(attributeNameB)]
                            ];

                    } else if(lomA == LoM.ORDINAL && lomB == LoM.ORDINAL)
                    {
                        bar = bars[
                            o.GetOrdValue(attributeNameA),
                            o.GetOrdValue(attributeNameB)
                            ];

                    } else if(lomA == LoM.NOMINAL && lomB == LoM.ORDINAL)
                    {
                        bar = bars[
                            dictA2[o.GetNomValue(attributeNameA)],
                            o.GetOrdValue(attributeNameB)
                            ];

                    } else if(lomA == LoM.ORDINAL && lomB == LoM.NOMINAL)
                    {
                        bar = bars[
                            o.GetOrdValue(attributeNameA),
                            dictB2[o.GetNomValue(attributeNameB)]
                            ];

                    } else
                    {
                        bar = new Bar3D();
                    }

                    o.AddRepresentativeObject(attributeNameA, bar.gameObject);
                    o.AddRepresentativeObject(attributeNameB, bar.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Draws the graph by using values calculated in the Init() method
    /// </summary>
    public void DrawGraph()
    {
        bars = new Bar3D[dimA, dimB];

        for(int i = 0; i < dimA; i++)
        {
            for(int ii = 0; ii < dimB; ii++)
            {
                bars[i, ii] = CreateBar(barHeights[i,ii], max);
                bars[i, ii].SetLabelText(absMapValues[i, ii].ToString());
                GameObject barGO = bars[i, ii].gameObject;
                barGO.transform.localPosition = new Vector3(((i+1) * .15f), 0, ((ii+1) * .15f));
                barGO.transform.parent = Anchor.transform;
            }
        }
    }

    /// <summary>
    /// Generates axes from the calculated values.
    /// </summary>
    public override void SetUpAxis()
    {
        var factory3D = ServiceLocator.instance.Factory3DPrimitives;

        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

        var xAxis = factory2D.CreateAutoTickedAxis(attributeNameA, AxisDirection.X, data);
        xAxis.transform.parent = Anchor.transform;
        xAxis.transform.localRotation = Quaternion.Euler(90, 0, 0);

        //var xAxis2 = factory2D.CreateAutoTickedAxis(attributeNameA, AxisDirection.X, data);
        //xAxis2.transform.parent = Anchor.transform;
        //xAxis2.transform.localPosition = new Vector3(0, 0, (dimB + 1) * .15f);

        //var xAxis3 = factory2D.CreateAutoTickedAxis(attributeNameA, AxisDirection.X, data);
        //xAxis3.transform.parent = Anchor.transform;
        //xAxis3.transform.localPosition = new Vector3(0, 1, (dimB + 1) * .15f);

        //var xAxis4 = factory2D.CreateAutoTickedAxis(attributeNameA, AxisDirection.X, data);
        //xAxis4.transform.parent = Anchor.transform;
        //xAxis4.transform.localPosition = new Vector3(0, 1, 0);

        var zAxis = factory2D.CreateAutoTickedAxis(attributeNameB, AxisDirection.Z, data);
        zAxis.transform.parent = Anchor.transform;

        //var zAxis2 = factory2D.CreateAutoTickedAxis(attributeNameB, AxisDirection.Z, data);
        //zAxis2.transform.parent = Anchor.transform;
        //zAxis2.transform.localPosition = new Vector3((dimA + 1) * .15f, 0, 0);

        //var zAxis3 = factory2D.CreateAutoTickedAxis(attributeNameB, AxisDirection.Z, data);
        //zAxis3.transform.parent = Anchor.transform;
        //zAxis3.transform.localPosition = new Vector3((dimA + 1) * .15f, 1, 0);

        //var zAxis4 = factory2D.CreateAutoTickedAxis(attributeNameB, AxisDirection.Z, data);
        //zAxis4.transform.parent = Anchor.transform;
        //zAxis4.transform.localPosition = new Vector3(0, 1, 0);

        var yAxis = factory2D.CreateAutoTickedAxis("Amount", max);
        yAxis.transform.parent = Anchor.transform;
        
        //var yAxis2 = factory2D.CreateAutoTickedAxis("", max);
        //yAxis2.transform.parent = Anchor.transform;
        //yAxis2.transform.localPosition = new Vector3((dimA + 1) * .15f, 0, (dimB + 1) * .15f);

        //var yAxis3 = factory2D.CreateAutoTickedAxis("", max);
        //yAxis3.transform.parent = Anchor.transform;
        //yAxis3.transform.localPosition = new Vector3(0, 0, (dimB + 1) * .15f);

        //var yAxis4 = factory2D.CreateAutoTickedAxis("", max);
        //yAxis4.transform.parent = Anchor.transform;
        //yAxis4.transform.localPosition = new Vector3((dimA + 1) * .15f, 0, 0);
    }

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private Bar3D CreateBar(float value, float range)
    {
        var factory3D = ServiceLocator.instance.Factory3DPrimitives;
        
        Bar3D bar = factory3D.CreateBar(value, .1f, .1f).GetComponent<Bar3D>();

        bar.SetLabelText(value.ToString());

        return bar;
    }



    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        switch(scheme)
        {
            default: // case SplitHSV
                float H = 0f;
                for(int row=0; row<dimA; row++)
                {
                    float S = 0f;
                    for(int col=0; col<dimB; col++)
                    {
                        var color = Color.HSVToRGB(
                            (H/dimA)/2f+.5f, 
                            (S/dimB)/2f+.5f, 
                            1);
                        bars[row, col].SetColor(color);
                        bars[row, col].ApplyColor(color);
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
