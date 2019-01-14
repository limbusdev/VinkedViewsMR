using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Collections;
using Model;
using System.Linq;
using static CubeIconVariable;
using HoloToolkit.Unity.Buttons;

/// <summary>
/// Handles interaction of the VisualizationFactory's buttons.
/// </summary>
public class VisFactoryInteractionReceiver : InteractionReceiver
{
    public GameObject HoloButtonPrefab;
    public GameObject screenAnchor;
    public GameObject newETVPlatform;
    public GameObject CubeIconVariablePrefab;
    public GameObject ObjectCollection;
    public GameObject VisTypeChoicePanel;

    public IDictionary<string, GameObject> currentlyChosenAttributes = new Dictionary<string, GameObject>();
    public int currentlyChosenDataBase = 0;
    public IList<GameObject> currentlyActiveVisChoicePanelButtons = new List<GameObject>();

    protected override void InputDown(GameObject obj, InputEventData eventData)
    {
        Debug.Log(obj.name + " : InputDown");

        switch(obj.name)
        {
            case "ButtonDataSets":
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 1,2,3,4,5 });
                break;
            case "Back":
                ClearSelection();
                ClearVisTypeButtons();
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 0, 5 });
                break;
            case "ButtonDataSet1":
                currentlyChosenDataBase = 0;
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 1,4,5 });
                SetupGalery(currentlyChosenDataBase);
                break;
            case "ButtonDataSet2":
                currentlyChosenDataBase = 1;
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 2,4,5 });
                SetupGalery(currentlyChosenDataBase);
                break;
            case "ButtonDataSet3":
                currentlyChosenDataBase = 2;
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 3,4,5 });
                SetupGalery(currentlyChosenDataBase);
                break;
            case "CubeIconVariable":
                HideAllIconSubButtons();
                if(obj.GetComponent<CubeIconVariable>() != null)
                {
                    var cubecon = obj.GetComponent<CubeIconVariable>();
                    cubecon.Select();

                    if(cubecon.selected)
                    {
                        currentlyChosenAttributes.Add(cubecon.varNames[0], obj);
                    }else
                    {
                        currentlyChosenAttributes.Remove(cubecon.varNames[0]);
                    }

                    ShowVisTypeButtons();
                }
                break;
            case "VisTypeButton":
                Services.VisFactory().GenerateVisFrom(
                    currentlyChosenDataBase,
                    currentlyChosenAttributes.Keys.ToArray(),
                    obj.GetComponent<VisTypeButton>().visType
                    );
                break;
            case "ButtonQuitApp":
                Debug.Log("Quitting Application");
                Application.Quit();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Hides sub buttons of the shelf items.
    /// </summary>
    private void HideAllIconSubButtons()
    {
        VisTypeChoicePanel.SetActive(false);
    }

    /// <summary>
    /// Hides all shelf items except the given one.
    /// </summary>
    /// <param name="exception"></param>
    public void ClearAllBut(GameObject exception)
    {
        for(int i = 0; i < ObjectCollection.transform.childCount; i++)
        {
            var g = ObjectCollection.transform.GetChild(i);
            if(!g.gameObject.Equals(exception))
                Destroy(g.gameObject);
        }
    }

    /// <summary>
    /// Removes all shelf items from the gallery.
    /// </summary>
    private void ClearGallery()
    {
        for(int i = 0; i < ObjectCollection.transform.childCount; i++)
        {
            var g = ObjectCollection.transform.GetChild(i);
            interactables.Remove(g.gameObject);
            Destroy(g.gameObject);
        }
    }

    /// <summary>
    /// Deactivates all interactables of this receiver.
    /// </summary>
    private void DeactivateAllInteractibles()
    {
        foreach(GameObject g in interactables)
        {
            if(g != null) g.SetActive(false);
        }
    }

    /// <summary>
    /// Activates the interactables with the given IDs.
    /// </summary>
    /// <param name="IDs"></param>
    private void ActivateInteractables(int[] IDs)
    {
        foreach(var id in IDs)
        {
            interactables[id].SetActive(true);
        }
    }

    /// <summary>
    /// Assembles a variable galery from the DataBase of the given ID and provides all
    /// combinations of the given dimension.
    /// </summary>
    /// <param name="dataBaseID"></param>
    /// <param name="dimension"></param>
    private void SetupGalery(int dataBaseID)
    {
        Setup1DGalery(dataBaseID);
        ObjectCollection.GetComponent<ObjectCollection>().UpdateCollection();
    }

    /// <summary>
    /// Creates a galery of all attributes.
    /// </summary>
    /// <param name="dbID"></param>
    private void Setup1DGalery(int dbID)
    {
        DataSet ds = Services.DataBase().dataSets[dbID];

        var ms = new List<AttributeStats>();
        ms.AddRange(ds.nominalStatistics.Values);
        ms.AddRange(ds.ordinalStatistics.Values);
        ms.AddRange(ds.intervalStatistics.Values);
        ms.AddRange(ds.rationalStatistics.Values);
        

        foreach(AttributeStats m in ms)
        {
            interactables.Add(Create1DIconAndInsert(m.name, m.type));
        }
    }
   

    private GameObject Create1DIconAndInsert(string name, LoM lom)
    {
       return CreateIconAndInsertInCollection(new string[] { name }, new LoM[] { lom });
    }
    

    private GameObject CreateIconAndInsertInCollection(string[] names, LoM[] loms)
    {
        GameObject etvIcon = Instantiate(CubeIconVariablePrefab);
        CubeIconVariable civ = etvIcon.GetComponent<CubeIconVariable>();
        civ.Init(names, loms, currentlyChosenDataBase);
        etvIcon.transform.parent = ObjectCollection.transform;

        return etvIcon;
    }

    private GameObject CreateMultiIconAndInsertInCollection(string[] names, LoM[] loms, string name)
    {
        GameObject etvIcon = Instantiate(CubeIconVariablePrefab);
        CubeIconVariable civ = etvIcon.GetComponent<CubeIconVariable>();
        civ.InitMulti(names, loms, currentlyChosenDataBase, name);
        etvIcon.transform.parent = ObjectCollection.transform;

        return etvIcon;
    }

    private void ClearSelection()
    {
        currentlyChosenAttributes.Clear();
    }

    private void ClearVisTypeButtons()
    {
        foreach(var b in currentlyActiveVisChoicePanelButtons)
        {
            interactables.Remove(b);
            Destroy(b);
        }

        currentlyActiveVisChoicePanelButtons.Clear();
    }

    /// <summary>
    /// Initializes the sub buttons to choose possible visualization type from.
    /// </summary>
    public void ShowVisTypeButtons()
    {
        ClearVisTypeButtons();

        if(currentlyChosenAttributes.Count > 0)
        {
            // Show matching buttons
            var suitableVisTypes = Services.VisFactory().ListPossibleVisualizations(currentlyChosenDataBase, currentlyChosenAttributes.Keys.ToArray());
            VisTypeChoicePanel.SetActive(true);
            
            for(int i = 0; i < suitableVisTypes.Length; i++)
            {
                var visType = suitableVisTypes[i];
                var button = Instantiate(HoloButtonPrefab);

                button.AddComponent<VisTypeButton>().visType = visType;

                var cb = button.GetComponent<CompoundButtonIcon>();
                switch(visType)
                {
                    case VisType.Histogram2D:
                        cb.IconName = "Icon2D";
                        break;
                    case VisType.Histogram3D:
                        cb.IconName = "Icon3D";
                        break;
                    case VisType.HistogramHeatmap3D:
                        cb.IconName = "Icon3D";
                        break;
                    case VisType.LineChart2D:
                        cb.IconName = "Icon2D";
                        break;
                    case VisType.PCP2D:
                        cb.IconName = "IconPCP";
                        break;
                    case VisType.PCP3D:
                        cb.IconName = "IconPCP";
                        break;
                    case VisType.ScatterPlot2D:
                        cb.IconName = "Icon2D";
                        break;
                    case VisType.ScatterPlot3D:
                        cb.IconName = "Icon3D";
                        break;
                    case VisType.SingleAxis3D:
                        cb.IconName = "Icon1D";
                        break;
                    default: cb.IconName = "Anchor"; break;
                }

                

                button.name = visType.ToString();
                button.GetComponent<CompoundButtonText>().Text = visType.ToString();

                button.transform.parent = VisTypeChoicePanel.transform;
                button.transform.localPosition = new Vector3(.15f * i - suitableVisTypes.Length*.15f/2f, 0, 0);
                button.transform.localRotation = Quaternion.Euler(Vector3.zero);
                
                interactables.Add(button);
                currentlyActiveVisChoicePanelButtons.Add(button);
            }
        }
    }


    // ........................................................................ Inner Classes

    private class IconKey2D
    {
        AttributeStats[] ks;

        public IconKey2D(AttributeStats k1, AttributeStats k2)
        {
            ks = new AttributeStats[2];
            ks[0] = k1;
            ks[0] = k2;
        }
    }

    private class IconKey
    {
        AttributeStats[] ks;

        public IconKey(AttributeStats k1, AttributeStats k2, AttributeStats k3)
        {
            ks = new AttributeStats[3];
            ks[0] = k1;
            ks[0] = k2;
            ks[0] = k3;
        }
    }
}