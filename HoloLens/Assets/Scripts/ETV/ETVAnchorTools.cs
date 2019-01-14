/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
