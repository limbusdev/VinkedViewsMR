using UnityEngine;

public class ServiceLocator : MonoBehaviour {

    public static ServiceLocator instance = null;

    public Graphical3DPrimitiveFactory Factory3DPrimitives; // Populate in editor
    public Graphical2DPrimitiveFactory Factory2DPrimitives; // Populate in editor
    public ETV3DFactory Factory3DETV;                       // Populate in editor
    public ETV2DFactory Factory2DETV;                       // populate in editor
    public VisualizationFactory visualizationFactory;       // Populate in editor
    

    void Awake()
    {
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
