using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ SUB CLASSES

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
    [XmlAttribute] public int dataSetID;
    [XmlAttribute] public VisType visType;

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



// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ MAIN CLASS

/// <summary>
/// GameManager manages game sessions by saving session data and restoring it.
/// </summary>
public class PersistenceManager : APersistenceManager
{
    // ........................................................................ Properties
    // .................................................... public
    public static string TAG = "PersistenceManager";
    public Dictionary<GameObject, SerializedETV> ETVs
         = new Dictionary<GameObject, SerializedETV>();


    // .................................................... private
    private static string SaveGameFileName = "setup.dat";
    private string saveGamePath;
    private bool initialized = false;


    // ........................................................................ PersistenceManager
    override public void Initialize()
    {
        if(!initialized)
        {
            saveGamePath = Path.Combine(Application.persistentDataPath, SaveGameFileName);
            initialized = true;
        }
    }

    void OnApplicationQuit()
    {
        Initialize();
        Save();
    }
    

    /// <summary>
    /// Enters the given ETV into a list of observed ETVs. All ETVs entered here,
    /// will be persisted, when the app is closed.
    /// </summary>
    /// <param name="etv">ETV to persist later</param>
    /// <param name="dataSetID">data set ID of it</param>
    /// <param name="variables">variables which it visualizes</param>
    /// <param name="visType">visualization type used</param>
    override public void PersistETV(GameObject etv, int dataSetID, string[] variables, VisType visType)
    {
        var persistableETV = new SerializedETV();
        persistableETV.dataSetID = dataSetID;
        persistableETV.variables = variables;
        persistableETV.visType = visType;

        ETVs.Add(etv, persistableETV);
    }

    /// <summary>
    /// Restores an ETV from the persisted information (position, rotation, variables,
    /// data set ID and Visualization Type).
    /// </summary>
    /// <param name="etv">serialized ETV information</param>
    override public void LoadPersistentETV(SerializedETV etv)
    {
        // Restore Visualization from loaded information
        var loadedETV = Services.VisFactory().GenerateVisFrom(etv.dataSetID, etv.variables, etv.visType);

        // Restore position and rotation
        var p = etv.position;
        var r = etv.rotation;

        loadedETV.transform.localPosition = new Vector3(p[0], p[1], p[2]);
        loadedETV.GetComponent<ETVAnchor>().Rotatable.transform.localRotation = Quaternion.Euler(r[0], r[1], r[2]);
    }

    
    /// <summary>
    /// Serializes all ETVs in the temporal storage to a file in the persistent
    /// data path of this app. The document is XML structured and human readable.
    /// </summary>
    override public void Save()
    {
        // Create save game container
        var saveData = new Setup();

        // Serialize ETV's anchors
        saveData.ETVs = new SerializedETV[ETVs.Keys.Count];

        int i = 0;
        foreach(var key in ETVs.Keys)
        {
            var pos = key.transform.localPosition;
            var rot = key.GetComponent<ETVAnchor>().GetVisEulerAngles();

            ETVs[key].position = new float[] { pos.x, pos.y, pos.z };
            ETVs[key].rotation = new float[] { rot.x, rot.y, rot.z };

            saveData.ETVs[i] = ETVs[key];
            i++;
        }

        // Serialize Visualization Factory's anchor
        saveData.visFactory = new SerializedAnchor();

        var vfAnchor = Services.VisFactory().GetComponentInChildren<ETVAnchor>();
        
        var vfPos = vfAnchor.transform.localPosition;
        var vfRot = vfAnchor.GetVisEulerAngles();

        saveData.visFactory.position = new float[] { vfPos.x, vfPos.y, vfPos.z };
        saveData.visFactory.rotation = new float[] { vfRot.x, vfRot.y, vfRot.z};
        

        using(var writer = new StreamWriter(File.Create(saveGamePath)))
        {
            var serializer = new XmlSerializer(typeof(Setup));
            serializer.Serialize(writer, saveData);
        }

        Debug.Log(TAG + ": save data serialized and written to save file.");
    }


    /// <summary>
    /// Loads information about persistent ETVs from an XML file in the 
    /// persistent data path to restore the last session.
    /// </summary>
    override public void Load()
    {
        var newETVs = new List<SerializedETV>();
        var vfPos = Vector3.up;
        var vfRot = Vector3.zero;

        if(File.Exists(saveGamePath))
        {
            Debug.Log(TAG + ": save file " + saveGamePath + " found.");

            using(var reader = new StreamReader(File.OpenRead(saveGamePath)))
            {
                var serializer = new XmlSerializer(typeof(Setup));
                var saveGame = (Setup)serializer.Deserialize(reader);

                var pos = saveGame.visFactory.position;
                var rot = saveGame.visFactory.rotation;
                vfPos = new Vector3(pos[0], pos[1], pos[2]);
                vfRot = new Vector3(rot[0], rot[1], rot[2]);

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
        vf.GetComponentInChildren<ETVAnchor>().transform.localPosition = vfPos;
        vf.GetComponentInChildren<ETVAnchor>().Rotatable.transform.localRotation = Quaternion.Euler(vfRot);
            
    }

}
