using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;

[Serializable]
public class SerializedETV
{
    public float[] position;
    public float[] rotation;
    public int dataSetID;
    public string[] variables;
    public VisType visType;
}

[Serializable]
class Setup
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

    private void OnEnable()
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
        var bf = new BinaryFormatter();
        var file = File.Open(Path.Combine(Application.persistentDataPath, SaveGameFileName), FileMode.OpenOrCreate);

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

        // write save game container to file and close file
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if(GlobalSettings.scenario != GlobalSettings.Scenario.RELEASE)
            return;
        var newETVs = new List<SerializedETV>();

        if(File.Exists(Path.Combine(Application.persistentDataPath, SaveGameFileName)))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Path.Combine(Application.persistentDataPath, SaveGameFileName), FileMode.Open);
            Setup saveGame = (Setup)bf.Deserialize(file);
            file.Close();

            newETVs.AddRange(saveGame.ETVs);
        }

        foreach(var savedETV in newETVs)
        {
            // Generate ETV with VisualizationFactory
            LoadPersistentETV(savedETV);
        }
            
    }


}
