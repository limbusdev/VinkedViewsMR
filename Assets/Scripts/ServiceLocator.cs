using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour {

    public static ServiceLocator instance = null;

    public Graphical3DPrimitiveFactory factory3Dservice;    // Populate in editor
    public Graphical2DPrimitiveFactory factory2Dservice;    // Populate in editor
    public ETV3DFactory factoryETV3Dservice;                // Populate in editor

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

    public void provideGraphical2DPrimitiveFactory(Graphical2DPrimitiveFactory factory)
    {
        factory2Dservice = factory;
    }

    public void provideGraphical3DPrimitiveFactory(Graphical3DPrimitiveFactory factory)
    {
        factory3Dservice = factory;
    }

    public Graphical2DPrimitiveFactory get2DFactory()
    {
        if(factory2Dservice == null)
        {
            //factory2Dservice = new Graphical2DPrimitiveFactory();
        }
        return factory2Dservice;
    }

    public Graphical3DPrimitiveFactory get3DFactory()
    {
        if (factory3Dservice == null)
        {
            //factory3Dservice = new Graphical3DPrimitiveFactory();
        }
        return factory3Dservice;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
