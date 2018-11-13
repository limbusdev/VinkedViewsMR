using ETV;
using MetaVisualization;
using UnityEngine;
using VisBridge;

/// <summary>
/// Service locator. Loose implementation of the design pattern 
/// Service Locator. Gives access to all important system services
/// and singletons. Populate in editor!
/// </summary>
public class ServiceLocator : MonoBehaviour
{ 
    public static ServiceLocator instance = null;

    [SerializeField]
    public Graphical3DPrimitiveFactory Factory3DPrimitives;
    public Graphical2DPrimitiveFactory Factory2DPrimitives;
    public ETV3DFactory Factory3DETV;                      
    public ETV2DFactory Factory2DETV;
    public VisualizationFactory visualizationFactory;       
    public AETVManager etvManager;
    public AMetaVisSystem metaVisSystem;
    public AMetaVisFactory FactoryMetaVis;
    public AVisBridgeSystem VisBridgeSystem;

    public ClientManager clientManager;

    public static AVisBridgeSystem VisBridges()
    {
        if(instance.VisBridgeSystem == null)
        {
            instance.VisBridgeSystem = new NullVisBridgeSystem();
        }
        return instance.VisBridgeSystem;
    }

    public static Graphical3DPrimitiveFactory PrimitivePlant3D()
    {
        return instance.Factory3DPrimitives;
    }

    public static Graphical2DPrimitiveFactory PrimitivePlant2D()
    {
        return instance.Factory2DPrimitives;
    }

    public static ETV3DFactory ETVPlant3D()
    {
        return instance.Factory3DETV;
    }

    public static ETV2DFactory ETVPlant2D()
    {
        return instance.Factory2DETV;
    }

    public static AMetaVisSystem MetaVisSystem()
    {
        return instance.metaVisSystem;
    }

    public static AMetaVisFactory MetaVisPlant()
    {
        return instance.FactoryMetaVis;
    }

    public static VisualizationFactory VisPlant()
    {
        return instance.visualizationFactory;
    }

    public static AETVManager ETVMan()
    {
        return instance.etvManager;
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
