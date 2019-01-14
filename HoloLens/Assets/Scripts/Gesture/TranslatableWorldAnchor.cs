/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
