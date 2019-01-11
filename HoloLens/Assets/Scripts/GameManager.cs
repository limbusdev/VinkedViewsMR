using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;


/// <summary>
/// Serializable XML-SubElement
/// </summary>
[Serializable]
public class SerializedETV
{
    [XmlAttribute]
    public int dataSetID;
    public VisType visType;

    public float[] position;
    public float[] rotation;
    public string[] variables;
    
}

/// <summary>
/// Simple serializable container to collect information, which should be
/// persistent between session. Use simple data types only. (int, string, 
/// float, simple arrays, ...)
/// </summary>
[Serializable]
[XmlRoot("Setup")]
public class Setup
{
    public SerializedETV[] ETVs;
}


/// <summary>
/// GameManager manages game sessions by saving session data and restoring it.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ........................................................................ Properties
    public static string TAG = "GameManager";

    private static string SaveGameFileName = "setup.dat";
    public string saveGamePath;

    // Singleton Instance
    public static GameManager gameManager;

    // temporal storage for persistent ETVs
    public Dictionary<GameObject,SerializedETV> ETVs;

    // ........................................................................ MonoBehaviour
    private void Awake()
    {
        // Singleton kind-of
        if(gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        } else if(gameManager != this)
        {
            Destroy(gameObject);
        }

        ETVs = new Dictionary<GameObject, SerializedETV>();
    }


    // ........................................................................ GameManager

    public void OnDataProviderFinishedLoading()
    {
        saveGamePath = Path.Combine(Application.persistentDataPath, SaveGameFileName);
        Load();
    }

    void OnApplicationQuit()
    {
        Save();
    }


    public void PersistETV(GameObject etv, int dataSetID, string[] variables, VisType visType)
    {
        if(GlobalSettings.scenario != GlobalSettings.Scenario.RELEASE)
            return;
        var persistableETV = new SerializedETV();
        persistableETV.dataSetID = dataSetID;
        persistableETV.variables = variables;
        persistableETV.visType = visType;

        ETVs.Add(etv, persistableETV);
    }

    public void LoadPersistentETV(SerializedETV etv)
    {
        if(GlobalSettings.scenario != GlobalSettings.Scenario.RELEASE)
            return;

        // Restore Visualization from loaded information
        var loadedETV = Services.VisFactory().GenerateVisFrom(etv.dataSetID, etv.variables, etv.visType);

        // Restore position and rotation
        loadedETV.transform.localPosition = new Vector3(etv.position[0], etv.position[1], etv.position[2]);
        loadedETV.GetComponent<ETVAnchor>().Rotatable.transform.localRotation 
            = new Quaternion(etv.rotation[0], etv.rotation[1], etv.rotation[2], etv.rotation[3]);
    }

    

    public void Save()
    {
        // Create save game container
        var saveData = new Setup();
        saveData.ETVs = new SerializedETV[ETVs.Keys.Count];

        int i = 0;
        foreach(var key in ETVs.Keys)
        {
            ETVs[key].position = new float[] {
                key.transform.localPosition.x,
                key.transform.localPosition.y,
                key.transform.localPosition.z
            };

            var rotatable = key.GetComponent<ETVAnchor>().Rotatable.transform.localRotation;
            ETVs[key].rotation = new float[]
            {
                rotatable.x,
                rotatable.y,
                rotatable.z,
                rotatable.w
            };
            saveData.ETVs[i] = ETVs[key];
            i++;
        }
        

        using(StreamWriter writer = new StreamWriter(File.Create(saveGamePath)))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Setup));
            serializer.Serialize(writer, saveData);
        }

        Debug.Log(TAG + ": save data serialized and written to save file.");
    }

    public void Load()
    {
        var newETVs = new List<SerializedETV>();

        if(File.Exists(saveGamePath))
        {
            Debug.Log(TAG + ": save file " + saveGamePath + " found.");

            using(StreamReader reader = new StreamReader(File.OpenRead(saveGamePath)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Setup));
                Setup saveGame = (Setup)serializer.Deserialize(reader);

                newETVs.AddRange(saveGame.ETVs);
            }

            Debug.Log(TAG + ": save file deserialized.");
        } else
        {
            Debug.Log(TAG + ": save file " + saveGamePath + " not found.");
        }

        foreach(var savedETV in newETVs)
        {
            // Generate ETV with VisualizationFactory
            LoadPersistentETV(savedETV);
        }
            
    }

}
