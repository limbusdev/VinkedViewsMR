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