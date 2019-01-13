using UnityEngine;

public class NullPersistenceManager : APersistenceManager
{
    public static readonly string TAG = "NullPersistenceManager";

    public override void Load()
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void Save()
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void Initialize()
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void LoadPersistentETV(SerializedETV etv)
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }

    public override void PersistETV(GameObject etv, int dataSetID, string[] variables, VisType visType)
    {
        Debug.Log(TAG + ": Dummy. Doing nothing.");
    }
}