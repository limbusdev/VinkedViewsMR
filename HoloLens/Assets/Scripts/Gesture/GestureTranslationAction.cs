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
using System;
using UnityEngine;

namespace Gestures
{
    /// <summary>
    /// Constrainable Translation Component
    /// With this component, any GameObject, containing a mesh, can be made
    /// touch- and translateable with the given constraints:
    ///     * translate along X only
    ///     * translate along Y only
    ///     * translate along Z only
    ///     * translate in X-Y-layer only
    ///     * translate in X-Z-layer only
    ///     * translate in Y-Z-layer only
    ///     
    /// A mesh, reacting to manipulation gestures and a transform, being
    /// target of the translation, must be provided.
    /// </summary>
    public class GestureTranslationAction : 
        AConstrainedGesture,
        IManipulationHandler,
        IFocusable, 
        IInputHandler
    {
        [SerializeField]
        [Tooltip("Transform that will be dragged. Default is the components GameObject.")]
        private Transform hostTransform = null;

        [SerializeField]
        [Tooltip("Constrain translation to layer or axis.")]
        private AxisAndPlaneConstraint translationConstraint = AxisAndPlaneConstraint.NONE;

        [SerializeField]
        [Tooltip("Mesh that triggers this translation action.")]
        private GameObject Handle;

        [SerializeField]
        [Tooltip("Default material of the trigger mesh.")]
        private Material notHoldMat;

        [SerializeField]
        [Tooltip("Material, when trigger is active and translation in action.")]
        private Material HoldMat;

        private Vector3 manipulationOriginalPosition = Vector3.zero;

        public bool hasFocus = false;
        public bool isTapped = false;
        public float translationFactor = 3f;
        
        
        // .................................................................... Unity Methods

        void Awake()
        {
            if(hostTransform == null)
            {
                hostTransform = transform;
            }
        }

        // .................................................................... INTERFACE IManipulationHandler

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {
            if(isTapped)
            {
                InputManager.Instance.PushModalInputHandler(gameObject);
                manipulationOriginalPosition = hostTransform.localPosition;
                eventData.Use();
            }
        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if(isTapped)
            {
                var cumulativeUpdate = eventData.CumulativeDelta;

                switch(translationConstraint)
                {
                    case AxisAndPlaneConstraint.X_AXIS_ONLY:
                        cumulativeUpdate.y = 0;
                        cumulativeUpdate.z = 0;
                        break;
                    case AxisAndPlaneConstraint.Y_AXIS_ONLY:
                        cumulativeUpdate.x = 0;
                        cumulativeUpdate.z = 0;
                        break;
                    case AxisAndPlaneConstraint.Z_AXIS_ONLY:
                        cumulativeUpdate.x = 0;
                        cumulativeUpdate.y = 0;
                        break;
                    case AxisAndPlaneConstraint.XY_PLANE_ONLY:
                        cumulativeUpdate.z = 0;
                        break;
                    case AxisAndPlaneConstraint.XZ_PLANE_ONLY:
                        cumulativeUpdate.x += cumulativeUpdate.y / 2f;
                        cumulativeUpdate.z += cumulativeUpdate.y / 2f;
                        cumulativeUpdate.y = 0;
                        break;
                    case AxisAndPlaneConstraint.YZ_PLANE_ONLY:
                        cumulativeUpdate.x = 0;
                        break;
                    default: // case NONE:
                        break;
                }

                hostTransform.localPosition = manipulationOriginalPosition + cumulativeUpdate*translationFactor;
                eventData.Use();
            }
        }

        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
            eventData.Use();
        }

        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
            eventData.Use();
        }

        // .................................................................... INTERFACE INavigationHandler

            

        public void OnInputDown(InputEventData eventData)
        {
            if(hasFocus)
            {
                isTapped = true;
                InputManager.Instance.OverrideFocusedObject = gameObject;
                try
                {
                    Handle.GetComponent<MeshRenderer>().material = HoldMat;
                } catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
                
            }
        }

        public void OnInputUp(InputEventData eventData)
        {
            isTapped = false;
            InputManager.Instance.OverrideFocusedObject = null;
            try
            {
                Handle.GetComponent<MeshRenderer>().material = notHoldMat;
            } catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
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