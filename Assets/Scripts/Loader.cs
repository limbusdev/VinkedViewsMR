using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject serviceLocator;

    void Awake()
    {
        //Check if a ServiceLocator has already been assigned to static variable ServiceLocator.instance or if it's still null
        if (ServiceLocator.instance == null)
        {

            //Instantiate serviceLocator prefab
            Instantiate(serviceLocator);
        }
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
