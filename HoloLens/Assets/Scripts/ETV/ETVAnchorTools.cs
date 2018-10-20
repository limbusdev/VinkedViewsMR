using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tools
{
    ROTATE, SCALE, TRANSLATE, TRANSLATEFREELY
}

public class ETVAnchorTools : MonoBehaviour {

    public GameObject GadgetTranslate;
    public GameObject GadgetRotate;
    public GameObject GadgetScale;
    public GameObject GadgetTranslateFreely;

    public GameObject ToolAnchorSphere;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnableTool(Tools tool)
    {
        DisableAllTools();

        if(ToolAnchorSphere != null)
            ToolAnchorSphere.SetActive(true);

        switch (tool)
        {
            case Tools.ROTATE:
                if(GadgetRotate != null)
                    GadgetRotate.SetActive(true);
                break;
            case Tools.SCALE:
                if(GadgetScale != null)
                    GadgetScale.SetActive(true);
                break;
            case Tools.TRANSLATEFREELY:
                if(GadgetTranslateFreely != null)
                    GadgetTranslateFreely.SetActive(true);
                break;
            default:
                if(GadgetTranslate != null)
                    GadgetTranslate.SetActive(true);
                break;
                
        }
    }

    public void DisableAllTools()
    {
        if(GadgetTranslate != null)
            GadgetTranslate.SetActive(false);
        if(GadgetScale != null)
            GadgetScale.SetActive(false);
        if(GadgetRotate != null)
            GadgetRotate.SetActive(false);
        if(GadgetTranslateFreely != null)
            GadgetTranslateFreely.SetActive(false);
        if(ToolAnchorSphere != null)
            ToolAnchorSphere.SetActive(false);
    }
}
