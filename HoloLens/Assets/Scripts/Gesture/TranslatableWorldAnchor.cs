using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.XR.WSA;

public class TranslatableWorldAnchor : MonoBehaviour, IInputHandler
{
    [SerializeField]
    [Tooltip("Transform that will be dragged. Default is the components GameObject.")]
    private Transform hostTransform = null;

    public void OnInputDown(InputEventData eventData)
    {
        var anchor = hostTransform.gameObject.GetComponent<WorldAnchor>();
        if(anchor != null)
            DestroyImmediate(anchor);

        var anchorObject = hostTransform.gameObject.GetComponent<WorldAnchorObject>();
        if(anchorObject != null)
            DestroyImmediate(anchorObject);
    }

    public void OnInputUp(InputEventData eventData)
    {
        hostTransform.gameObject.AddComponent<WorldAnchor>();
        hostTransform.gameObject.AddComponent<WorldAnchorObject>();
    }
}
