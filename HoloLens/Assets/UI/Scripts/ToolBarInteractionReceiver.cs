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
            case "HolographicButtonSwitchMode":
                if(Targets[0] != null)
                {
                    Targets[0].SetActive(!Targets[0].activeSelf); // Toggle Toolbar
                }
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
            case "HolographicButtonDeactivate":
                Targets[0].SetActive(false); // Disable ETV
                try
                {
                    GameObject tools = Targets[2];
                    tools.SetActive(!tools.activeSelf);
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
