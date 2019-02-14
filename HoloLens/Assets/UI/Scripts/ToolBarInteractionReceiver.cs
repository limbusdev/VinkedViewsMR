/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System;
using UnityEngine;

public class ToolBarInteractionReceiver : InteractionReceiver
{
    public GameObject Rotatable;

    protected override void InputDown(GameObject obj, InputEventData eventData)
    {
        // Debug.Log(obj.name + " : InputDown");

        if(obj == null || obj.name == null)
            return;

        switch (obj.name)
        {
            // Enables / Disables Toolbar
            case "HolographicButtonSwitchMode":
                if(Targets[0] != null)
                {
                    Targets[0].SetActive(!Targets[0].activeSelf); // Toggle Toolbar
                }
                break;
            // Enables free translation tool
            case "HolographicButtonTranslateFreely":
                Targets[0].SetActive(false); // Disable Toolbar
                try
                {
                    GameObject tools = Targets[1];
                    ETVAnchorTools etvAT = tools.GetComponent<ETVAnchorTools>();
                    etvAT.EnableTool(Tools.TRANSLATEFREELY);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                break;
            // Enables constrained translation tool
            case "HolographicButtonTranslate":
                Targets[0].SetActive(false); // Disable Toolbar
                try
                {
                    GameObject tools = Targets[1];
                    ETVAnchorTools etvAT = tools.GetComponent<ETVAnchorTools>();
                    etvAT.EnableTool(Tools.TRANSLATE);
                } catch(Exception e)
                {
                    Debug.Log(e);
                }
                break;
            // Enables constrained rotation tool
            case "HolographicButtonRotate":
                Targets[0].SetActive(false); // Disable Toolbar
                try
                {
                    GameObject tools = Targets[1];
                    ETVAnchorTools etvAT = tools.GetComponent<ETVAnchorTools>();
                    etvAT.EnableTool(Tools.ROTATE);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                break;
            // Enables responsive visualization scaling tool
            case "HolographicButtonScale":
                Targets[0].SetActive(false); // Disable Toolbar
                try
                {
                    GameObject tools = Targets[1];
                    ETVAnchorTools etvAT = tools.GetComponent<ETVAnchorTools>();
                    etvAT.EnableTool(Tools.SCALE);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                break;
            // Resets ETVs rotation
            case "HolographicButtonReset":
                Targets[0].SetActive(false); // Disable Toolbar
                try
                {
                    try
                    {
                        GameObject tools = Targets[1];
                        ETVAnchorTools etvAT = tools.GetComponent<ETVAnchorTools>();
                        etvAT.DisableAllTools();
                        Targets[3].GetComponent<ETVAnchor>().resetRotation = true;
                    } catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                break;
            // Disables toolbar
            case "HolographicButtonClose":
                Targets[0].SetActive(false); // Disable Toolbar
                try
                {
                    GameObject tools = Targets[1];
                    ETVAnchorTools etvAT = tools.GetComponent<ETVAnchorTools>();
                    etvAT.DisableAllTools();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                break;
            // Deactivates anchored visualization
            case "HolographicButtonDeactivate":
                Targets[0].SetActive(false); // Disable ETV
                try
                {
                    GameObject vis = Targets[2];
                    vis.transform.GetChild(0).gameObject.SetActive(!vis.transform.GetChild(0).gameObject.activeSelf);
                } catch(Exception e)
                {
                    Debug.Log(e);
                }
                break;

            default:
                break;
        }
    }
}
