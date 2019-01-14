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
        private Vector3 originCameraPos = Vector3.zero;

        public bool hasFocus = false;
        public bool isTapped = false;
        
        
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

                hostTransform.localPosition = manipulationOriginalPosition + cumulativeUpdate;
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