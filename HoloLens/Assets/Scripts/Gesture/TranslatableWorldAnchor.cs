using HoloToolkit.Unity;
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
        {
            WorldAnchorManager.Instance.RemoveAnchor(hostTransform.gameObject);
            Debug.Log("World Anchor removed for translating.");
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        WorldAnchorManager.Instance.AttachAnchor(hostTransform.gameObject, hostTransform.gameObject.name);
        Debug.Log("World Anchor re-added");
    }
}
