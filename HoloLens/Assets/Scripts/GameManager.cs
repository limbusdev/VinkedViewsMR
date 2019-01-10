using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

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

[Serializable]
[XmlRoot("Setup")]
public class Setup
{
    public SerializedETV[] ETVs;
}

public class GameManager : MonoBehaviour
{
    private static string SaveGameFileName = "setup.dat";
    private static string FolderName = "binarySaveData";

    public static GameManager gameManager;
    public Dictionary<GameObject,SerializedETV> ETVs;

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
        var loadedETV = Services.VisFactory().GenerateVisFrom(etv.dataSetID, etv.variables, etv.visType);
        loadedETV.transform.localPosition = new Vector3(etv.position[0], etv.position[1], etv.position[2]);
        loadedETV.GetComponent<ETVAnchor>().Rotatable.transform.localRotation = new Quaternion(etv.rotation[0], etv.rotation[1], etv.rotation[2], etv.rotation[3]);
    }

    void Start()
    {
        if(GlobalSettings.scenario != GlobalSettings.Scenario.RELEASE)
            return;
        Load();
    }

    void OnApplicationQuit()
    {
        if(GlobalSettings.scenario != GlobalSettings.Scenario.RELEASE)
            return;
        Save();
    }

    public void Save()
    {
        // Only save in release builds
        if(GlobalSettings.scenario != GlobalSettings.Scenario.RELEASE)
            return;

        // open save game file
        var serializer = new XmlSerializer(typeof(Setup));
        var writer = new StreamWriter(Path.Combine(Application.persistentDataPath, SaveGameFileName));
        
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

        serializer.Serialize(writer, saveData);
        writer.Close();
    }

    public void Load()
    {
        if(GlobalSettings.scenario != GlobalSettings.Scenario.RELEASE)
            return;
        var newETVs = new List<SerializedETV>();

        if(File.Exists(Path.Combine(Application.persistentDataPath, SaveGameFileName)))
        {
            var serializer = new XmlSerializer(typeof(Setup));

            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            var fs = new FileStream(Path.Combine(Application.persistentDataPath, SaveGameFileName), FileMode.Open);
            Setup saveGame = (Setup)serializer.Deserialize(fs);

            fs.Close();

            newETVs.AddRange(saveGame.ETVs);
        }

        foreach(var savedETV in newETVs)
        {
            // Generate ETV with VisualizationFactory
            LoadPersistentETV(savedETV);
        }
            
    }

    // Handles unknown nodes
    private void serializer_UnknownNode(object sender, XmlNodeEventArgs e) { }

    // handles unknown attributes
    private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e) { }
 }
