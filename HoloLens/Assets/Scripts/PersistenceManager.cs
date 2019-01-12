using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

[Serializable]
public class SerializedAnchor
{
    public float[] position;
    public float[] rotation;
}

/// <summary>
/// Serializable XML-SubElement
/// </summary>
[Serializable]
public class SerializedETV : SerializedAnchor
{
    [XmlAttribute]
    public int dataSetID;
    public VisType visType;

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
    public SerializedAnchor visFactory;
    public SerializedETV[] ETVs;
}


/// <summary>
/// GameManager manages game sessions by saving session data and restoring it.
/// </summary>
public class PersistenceManager : MonoBehaviour
{
    // ........................................................................ Properties
    public static string TAG = "PersistenceManager";

    private static string SaveGameFileName = "setup.dat";
    private string saveGamePath;

    // Singleton Instance
    public static PersistenceManager Instance;

    // temporal storage for persistent ETVs
    public Dictionary<GameObject,SerializedETV> ETVs;


    // ........................................................................ MonoBehaviour
    private void Awake()
    {
        // Singleton kind-of
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if(Instance != this)
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
        var persistableETV = new SerializedETV();
        persistableETV.dataSetID = dataSetID;
        persistableETV.variables = variables;
        persistableETV.visType = visType;

        ETVs.Add(etv, persistableETV);
    }

    public void LoadPersistentETV(SerializedETV etv)
    {
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

        // Serialize Visualization Factory's anchor
        var visFactoryAnchor = Services.VisFactory().GetComponentInChildren<ETVAnchor>();
        saveData.visFactory = new SerializedAnchor();
        var pos = visFactoryAnchor.transform.localPosition;
        saveData.visFactory.position = new float[] { pos.x, pos.y, pos.z };
        var rot = visFactoryAnchor.Rotatable.transform.localRotation;
        saveData.visFactory.rotation = new float[] { rot.x, rot.y, rot.z, rot.w };
        

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
        var VisFactoryPos = Vector3.zero;
        var VisFactoryRot = Quaternion.identity;

        if(File.Exists(saveGamePath))
        {
            Debug.Log(TAG + ": save file " + saveGamePath + " found.");

            using(StreamReader reader = new StreamReader(File.OpenRead(saveGamePath)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Setup));
                Setup saveGame = (Setup)serializer.Deserialize(reader);

                var p = saveGame.visFactory.position;
                var r = saveGame.visFactory.rotation;
                VisFactoryPos = new Vector3(p[0], p[1], p[2]);
                VisFactoryRot = new Quaternion(r[0], r[1], r[2], r[3]);

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

        // Restore Visualization Factory's position
        var vf = Services.VisFactory();
        vf.GetComponentInChildren<ETVAnchor>().transform.localPosition = VisFactoryPos;
        vf.GetComponentInChildren<ETVAnchor>().Rotatable.transform.localRotation = VisFactoryRot;
            
    }

}
