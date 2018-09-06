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
        ToolAnchorSphere.SetActive(true);

        switch (tool)
        {
            case Tools.ROTATE: GadgetRotate.SetActive(true); break;
            case Tools.SCALE: GadgetScale.SetActive(true); break;
            case Tools.TRANSLATEFREELY: GadgetTranslateFreely.SetActive(true); break;
            default: GadgetTranslate.SetActive(true); break;
                
        }
    }

    public void DisableAllTools()
    {
        GadgetTranslate.SetActive(false);
        GadgetScale.SetActive(false);
        GadgetRotate.SetActive(false);
        GadgetTranslateFreely.SetActive(false);
        ToolAnchorSphere.SetActive(false);
    }
}
