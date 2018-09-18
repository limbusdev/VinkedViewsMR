using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;

public class VisFactoryInteractionReceiver : InteractionReceiver
{
    public GameObject HoloButtonPrefab;
    public GameObject screenAnchor;
    public DataProvider dataProvider;

    protected override void InputDown(GameObject obj, InputEventData eventData)
    {
        Debug.Log(obj.name + " : InputDown");

        switch(obj.name)
        {
            case "ButtonDataSets":
                interactables[0].SetActive(false);
                interactables[1].SetActive(true);
                interactables[2].SetActive(true);
                interactables[3].SetActive(true);
                interactables[4].SetActive(true);
                break;
            case "Back":
                interactables[0].SetActive(true);
                interactables[1].SetActive(false);
                interactables[2].SetActive(false);
                interactables[3].SetActive(false);
                interactables[4].SetActive(false);
                break;
            

            default:
                break;
        }
    }
}