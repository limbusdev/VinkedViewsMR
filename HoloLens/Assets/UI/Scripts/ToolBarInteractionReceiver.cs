using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarInteractionReceiver : InteractionReceiver
{
    protected override void InputDown(GameObject obj, InputEventData eventData)
    {
        Debug.Log(obj.name + " : InputDown");

        switch (obj.name)
        {
            case "HolographicButtonSwitchMode":
                Targets[0].SetActive(!Targets[0].activeSelf); // Toggle Toolbar
                break;
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

            default:
                break;
        }
    }
}
