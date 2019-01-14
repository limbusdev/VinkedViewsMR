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

using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace Gestures
{
    public class GestureRotationAction : 
        AConstrainedGesture,
        INavigationHandler, 
        IFocusable, 
        IInputHandler
    {
        [SerializeField]
        [Tooltip("Transform that will be dragged. Default is the components GameObject.")]
        private Transform hostTransform = null;

        [SerializeField]
        [Tooltip("Constrain rotation about an axis.")]
        private AxisAndPlaneConstraint rotationConstraint = AxisAndPlaneConstraint.Y_AXIS_ONLY;

        [Tooltip("Rotation max speed controls amount of rotation.")]
        [SerializeField]
        private float RotationSensitivity = 10.0f;

        public bool hasFocus = false;
        public bool isTapped = false;

        private Vector3 initialEulerAngles;

        // .................................................................... Unity Methods

        void Awake()
        {
            if(hostTransform == null)
            {
                hostTransform = transform;
            }
        }


        // .................................................................... INTERFACE IManipulationHandler


        public void OnNavigationStarted(NavigationEventData eventData)
        {
            if(isTapped)
            {
                initialEulerAngles = new Vector3();
                InputManager.Instance.PushModalInputHandler(gameObject);
                eventData.Use();
            }
        }

        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            if(isTapped)
            {
                var rotation = new Vector3();

                var cumulativeUpdate = eventData.NormalizedOffset.x;

                switch(rotationConstraint)
                {
                    case AxisAndPlaneConstraint.X_AXIS_ONLY:
                        rotation.x = cumulativeUpdate * 180f - initialEulerAngles.x;
                        break;
                    case AxisAndPlaneConstraint.Z_AXIS_ONLY:
                        rotation.z = cumulativeUpdate * 180f - initialEulerAngles.z;
                        break;
                    default: // case: Y_AXIS_ONLY:
                        rotation.y = cumulativeUpdate * 180f - initialEulerAngles.y;
                        break;
                }

                initialEulerAngles += rotation;
                hostTransform.Rotate(rotation, Space.Self);
            }
        }

        public void OnNavigationCompleted(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
            eventData.Use();
        }

        public void OnNavigationCanceled(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
            eventData.Use();
        }

        public void OnInputDown(InputEventData eventData)
        {
            if(hasFocus)
            {
                isTapped = true;
                InputManager.Instance.OverrideFocusedObject = gameObject;
            }
        }

        public void OnInputUp(InputEventData eventData)
        {
            isTapped = false;
            InputManager.Instance.OverrideFocusedObject = null;
        }

        public void OnFocusEnter()
        {
            hasFocus = true;
        }

        public void OnFocusExit()
        {
            hasFocus = false;
        }
        
    }
}