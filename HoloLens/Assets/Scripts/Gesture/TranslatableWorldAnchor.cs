using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.XR.WSA;

public class TranslatableWorldAnchor : MonoBehaviour, IManipulationHandler
{
    private WorldAnchor worldAnchor;

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        gameObject.AddComponent<WorldAnchor>();
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        gameObject.AddComponent<WorldAnchor>();
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        var anchor = gameObject.GetComponent<WorldAnchor>();
        if(anchor != null)
            DestroyImmediate(anchor);
       
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        
    }
}
