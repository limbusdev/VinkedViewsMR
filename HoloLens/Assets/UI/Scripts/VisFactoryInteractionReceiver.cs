using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.Collections;
using System.Linq;

public class VisFactoryInteractionReceiver : InteractionReceiver
{
    public GameObject HoloButtonPrefab;
    public GameObject screenAnchor;
    public DataProvider dataProvider;
    public GameObject newETVPlatform;
    public GameObject CubeIconVariablePrefab;
    public GameObject ObjectCollection;

    private int currentlyChosenDataBase = 0;

    protected override void InputDown(GameObject obj, InputEventData eventData)
    {
        Debug.Log(obj.name + " : InputDown");

        switch(obj.name)
        {
            case "ButtonDataSets":
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 1, 2, 3, 4 });
                break;
            case "Back":
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 0 });
                break;
            case "ButtonDataSet1":
                currentlyChosenDataBase = 0;
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 5,6,7,8 });
                break;
            case "ButtonDataSet2":
                currentlyChosenDataBase = 1;
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 5, 6, 7, 8 });
                break;
            case "ButtonDataSet3":
                currentlyChosenDataBase = 2;
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 5, 6, 7, 8 });
                break;
            case "Button1D":
                SetupGalery(currentlyChosenDataBase, 1);
                break;
            case "Button2D":
                SetupGalery(currentlyChosenDataBase, 2);
                break;
            case "Button3D":
                SetupGalery(currentlyChosenDataBase, 3);
                break;
            case "ButtonND":
                SetupGalery(currentlyChosenDataBase, 4);
                break;
            case "Back2":
                DeactivateAllInteractibles();
                ActivateInteractables(new int[] { 1,2,3,4 });

                ClearGalery();
                break;
            case "CubeIconVariable":
                HideAllIconSubButtons();
                if(obj.GetComponent<CubeIconVariable>() != null)
                {
                    obj.GetComponent<CubeIconVariable>().ShowButtons();
                }
                break;

            default:
                break;
        }
    }

    private void HideAllIconSubButtons()
    {
        for(int i = 0; i < ObjectCollection.transform.childCount; i++)
        {
            var g = ObjectCollection.transform.GetChild(i);
            if(g.gameObject.GetComponent<CubeIconVariable>() != null)
            {
                g.gameObject.GetComponent<CubeIconVariable>().HideButtons();
            }
        }
    }

    public void ClearAllBut(GameObject exception)
    {
        for(int i = 0; i < ObjectCollection.transform.childCount; i++)
        {
            var g = ObjectCollection.transform.GetChild(i);
            if(!g.gameObject.Equals(exception))
                Destroy(g.gameObject);
        }
    }

    private void ClearGalery()
    {
        for(int i = 0; i < ObjectCollection.transform.childCount; i++)
        {
            var g = ObjectCollection.transform.GetChild(i);
            Destroy(g.gameObject);
        }
    }

    private void DeactivateAllInteractibles()
    {
        foreach(GameObject g in interactables)
        {
            g.SetActive(false);
        }
    }

    private void ActivateInteractables(int[] IDs)
    {
        foreach(var id in IDs)
        {
            interactables[id].SetActive(true);
        }
    }

    private void SetupGalery(int dataBaseID, int dimension)
    {
        switch(dimension)
        {
            case 3: Setup3DGalery(dataBaseID); break;
            case 2: Setup2DGalery(dataBaseID); break;
            default: Setup1DGalery(dataBaseID); break;
        }

        ObjectCollection.GetComponent<ObjectCollection>().UpdateCollection();
    }

    private void Setup1DGalery(int dbID)
    {
        DataSet ds = dataProvider.dataSets[dbID];

        var ms = new List<DataDimensionMeasures>();
        ms.AddRange(ds.dataMeasuresNominal.Values);
        ms.AddRange(ds.dataMeasuresOrdinal.Values);
        ms.AddRange(ds.dataMeasuresInterval.Values);
        ms.AddRange(ds.dataMeasuresRatio.Values);

        var keys = new List<IconKey>();

        foreach(DataDimensionMeasures m in ms)
        {
            interactables.Add(Create1DIconAndInsert(m.variableName, m.type));
        }
    }
    
    private void Setup2DGalery(int dbID)
    {
        DataSet ds = dataProvider.dataSets[dbID];

        var ms = new List<DataDimensionMeasures>();
        ms.AddRange(ds.dataMeasuresNominal.Values);
        ms.AddRange(ds.dataMeasuresOrdinal.Values);
        ms.AddRange(ds.dataMeasuresInterval.Values);
        ms.AddRange(ds.dataMeasuresRatio.Values);

        var keys = new List<IconKey2D>();

        foreach(var m1 in ms)
        {
            foreach(var m2 in ms)
            {
                var key = new IconKey2D(m1, m2);
                if(!keys.Contains(key) && !m1.Equals(m2))
                {
                    keys.Add(key);
                    Create2DIconAndInsert(m1.variableName, m2.variableName, m1.type, m2.type);
                }
            }
        }
    }
    

    private void Setup3DGalery(int dbID)
    {
        DataSet ds = dataProvider.dataSets[dbID];
        
        var ms = new List<DataDimensionMeasures>();
        ms.AddRange(ds.dataMeasuresNominal.Values);
        ms.AddRange(ds.dataMeasuresOrdinal.Values);
        ms.AddRange(ds.dataMeasuresInterval.Values);
        ms.AddRange(ds.dataMeasuresRatio.Values);

        var keys = new List<IconKey>();


        foreach(var m1 in ms)
        {
            foreach(var m2 in ms)
            {
                foreach(var m3 in ms)
                {
                    var key = new IconKey(m1, m2, m3);
                    if(!keys.Contains(key) && !(m1.Equals(m2) || m2.Equals(m3) || m3.Equals(m1)))
                    {
                        keys.Add(key);
                        Create3DIconAndInsert(m1.variableName, m2.variableName, m3.variableName, m1.type, m2.type, m3.type);
                    }
                }
            }
        }
    }

    private GameObject Create1DIconAndInsert(string name, LevelOfMeasurement lom)
    {
       return CreateIconAndInsertInCollection(new string[] { name }, new LevelOfMeasurement[] { lom });
    }

    private GameObject Create2DIconAndInsert(string name1, string name2, LevelOfMeasurement lom1, LevelOfMeasurement lom2)
    {
        return CreateIconAndInsertInCollection(new string[] { name1, name2 }, new LevelOfMeasurement[] { lom1, lom2 });
    }

    private GameObject Create3DIconAndInsert(string name1, string name2, string name3, LevelOfMeasurement lom1, LevelOfMeasurement lom2, LevelOfMeasurement lom3)
    {
        return CreateIconAndInsertInCollection(new string[] { name1, name2, name3 }, new LevelOfMeasurement[] { lom1, lom2, lom3 });
    }

    private GameObject CreateIconAndInsertInCollection(string[] names, LevelOfMeasurement[] loms)
    {
        GameObject etvIcon = Instantiate(CubeIconVariablePrefab);
        CubeIconVariable civ = etvIcon.GetComponent<CubeIconVariable>();
        civ.Init(names, loms, currentlyChosenDataBase);
        etvIcon.transform.parent = ObjectCollection.transform;

        return etvIcon;
    }


    private class IconKey2D
    {
        DataDimensionMeasures[] ks;

        public IconKey2D(DataDimensionMeasures k1, DataDimensionMeasures k2)
        {
            ks = new DataDimensionMeasures[2];
            ks[0] = k1;
            ks[0] = k2;
        }

        public override bool Equals(object obj)
        {
            if(obj is IconKey2D)
            {
                var o = obj as IconKey2D;
                return (ks.Contains(o.ks[0]) && ks.Contains(o.ks[1]));
            } else
            {
                return false;
            }
        }
    }

    private class IconKey
    {
        DataDimensionMeasures[] ks;

        public IconKey(DataDimensionMeasures k1, DataDimensionMeasures k2, DataDimensionMeasures k3)
        {
            ks = new DataDimensionMeasures[3];
            ks[0] = k1;
            ks[0] = k2;
            ks[0] = k3;
        }

        public override bool Equals(object obj)
        {
            if(obj is IconKey)
            {
                var o = obj as IconKey;
                return (ks.Contains(o.ks[0]) && ks.Contains(o.ks[1]) && ks.Contains(o.ks[2]));
            } else
            {
                return false;
            }
        }
    }
}