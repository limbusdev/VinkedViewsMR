using UnityEngine;

public abstract class APersistenceManager : MonoBehaviour
{
    public abstract void Load();
    public abstract void Save();
    public abstract void Initialize();
    public abstract void LoadPersistentETV(SerializedETV etv);
    public abstract void PersistETV(GameObject etv, int dataSetID, string[] variables, VisType visType);
}