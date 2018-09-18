using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour {

    public static ServiceLocator instance = null;

    public Graphical3DPrimitiveFactory PrimitiveFactory3Dservice;       // Populate in editor
    public Graphical2DPrimitiveFactory PrimitiveFactory2Dservice;       // Populate in editor
    public ETV3DFactory ETV3DFactoryService;                            // Populate in editor
    public ETV2DFactory ETV2DFactoryService;                            // populate in editor
    public VisualizationFactory visualizationFactory;                   // Populate in editor

    //Awake is always called before any Start functions
    void Awake()
    {
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

    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
