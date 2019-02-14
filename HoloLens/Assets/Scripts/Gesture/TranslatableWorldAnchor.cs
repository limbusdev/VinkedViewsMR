/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
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
