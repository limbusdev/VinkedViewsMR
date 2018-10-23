using ETV;
using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaVisFactory : MonoBehaviour
{
    [SerializeField]
    public GameObject ETV3DFlexiblePCPPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
    {
        var metaVis = Instantiate(ETV3DFlexiblePCPPrefab);

        int[] nomIDs, ordIDs, ivlIDs, ratIDs;

        AttributeProcessor.ExtractAttributeIDs(data, attIDs, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

        metaVis.GetComponent<ETV3DFlexiblePCP>().Init(data, nomIDs, ordIDs, ivlIDs, ratIDs, axisA, axisB);
        metaVis.GetComponent<ETV3DFlexiblePCP>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);
        metaVis.GetComponent<ETV3DFlexiblePCP>().DrawGraph();

        return metaVis;
    }
}
