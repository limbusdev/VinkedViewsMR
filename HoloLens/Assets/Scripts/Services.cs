﻿/*
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
using ETV;
using MetaVisualization;
using UnityEngine;
using VisBridges;

/// <summary>
/// Service locator. Loose implementation of the design pattern 
/// Service Locator. Gives access to all important system services
/// and singletons. Populate in editor!
/// </summary>
public class Services : MonoBehaviour
{
    public static Services instance = null;

    [SerializeField]
    public Graphical3DPrimitiveFactory Factory3DPrimitives;
    public Graphical2DPrimitiveFactory Factory2DPrimitives;
    public ETV3DFactory Factory3DETV;                      
    public ETV2DFactory Factory2DETV;
    public VisualizationFactory visualizationFactory;
    public AMetaVisSystem metaVisSystem;
    public AMetaVisFactory FactoryMetaVis;
    public AVisBridgeSystem VisBridgeSystem;
    public APersistenceManager PersistenceManager;
    public DataProvider dataProvider;

    public ClientManager clientManager;

    public static AVisBridgeSystem VisBridgeSys()
    {
        if(instance.VisBridgeSystem == null)
        {
            instance.VisBridgeSystem = new NullVisBridgeSystem();
        }
        return instance.VisBridgeSystem;
    }

    public static Graphical3DPrimitiveFactory PrimFactory3D()
    {
        return instance.Factory3DPrimitives;
    }

    public static DataProvider DataBase()
    {
        if(instance.dataProvider == null)
        {
            throw new System.Exception("Data Provider must be provided to the ServiceLocator.");
        }
        instance.dataProvider.Initialize();
        return instance.dataProvider;
    }

    public static Graphical2DPrimitiveFactory PrimFactory2D()
    {
        return instance.Factory2DPrimitives;
    }

    public static ETV3DFactory ETVFactory3D()
    {
        return instance.Factory3DETV;
    }

    public static ETV2DFactory ETVFactory2D()
    {
        return instance.Factory2DETV;
    }

    public static AMetaVisSystem MetaVisSys()
    {
        return instance.metaVisSystem;
    }

    public static AMetaVisFactory MetaVisFactory()
    {
        return instance.FactoryMetaVis;
    }

    public static VisualizationFactory VisFactory()
    {
        instance.visualizationFactory.Initialize();
        return instance.visualizationFactory;
    }

    public static APersistenceManager Persistence()
    {
        instance.PersistenceManager.Initialize();
        return instance.PersistenceManager;
    }


    void Awake()
    {
        //Physics.autoSimulation = false;
        // SINGLETON

        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
}
