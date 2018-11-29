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
        [Tooltip("Constrain translation along an axis.")]
        private AxisAndPlaneConstraint translationConstraint = AxisAndPlaneConstraint.NONE;

        private Vector3 manipulationOriginalPosition = Vector3.zero;
        private Vector3 originCameraPos = Vector3.zero;

        public bool hasFocus = false;
        public bool isTapped = false;

        public GameObject Handle;

        public Material notHoldMat, HoldMat;
        
        void Awake()
        {
            if(hostTransform == null)
            {
                hostTransform = transform;
            }
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {
            if(isTapped)
            {
                InputManager.Instance.PushModalInputHandler(gameObject);

                manipulationOriginalPosition = transform.position;
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
                        cumulativeUpdate.y = 0;
                        break;
                    case AxisAndPlaneConstraint.YZ_PLANE_ONLY:
                        cumulativeUpdate.x = 0;
                        break;
                    default: // case NONE:
                        break;
                }

                hostTransform.position = manipulationOriginalPosition + cumulativeUpdate;
                eventData.Use();
            }

            if(isTapped)
            {
                //Vector3 positionUpdate = (new Vector3(eventData.NormalizedOffset.x, eventData.NormalizedOffset.y, eventData.NormalizedOffset.x)) * .01f;
                //var glOff = Camera.current.transform.TransformVector(eventData.NormalizedOffset);
                //glOff = hostTransform.InverseTransformVector(glOff) * .5f;
                //positionUpdate = glOff;

                //Debug.Log(glOff);

                //switch(translationConstraint)
                //{
                //    case AxisAndPlaneConstraint.X_AXIS_ONLY:
                //        positionUpdate.y = 0;
                //        positionUpdate.z = 0;
                //        positionUpdate.x = glOff.x;
                //        //var diff = Camera.current.transform.position - originCameraPos;
                //        //hostTransform.position = new Vector3(
                //        //    manipulationOriginalPosition.x + glOff.x, 
                //        //    hostTransform.position.y, 
                //        //    hostTransform.position.z
                //        //    );
                //        break;
                //    case AxisAndPlaneConstraint.Y_AXIS_ONLY:
                //        positionUpdate.x = 0;
                //        positionUpdate.z = 0;
                //        break;
                //    case AxisAndPlaneConstraint.Z_AXIS_ONLY:
                //        positionUpdate.z = positionUpdate.x; // Map X-Component of Gesture to z-Position
                //        positionUpdate.x = 0;
                //        positionUpdate.y = 0;
                //        break;
                //    case AxisAndPlaneConstraint.XY_PLANE_ONLY:
                //        positionUpdate.z = 0;
                //        break;
                //    case AxisAndPlaneConstraint.XZ_PLANE_ONLY:
                //        positionUpdate.y = 0;
                //        break;
                //    case AxisAndPlaneConstraint.YZ_PLANE_ONLY:
                //        positionUpdate.z = positionUpdate.x;
                //        positionUpdate.x = 0;
                //        break;
                //    default: // case NONE:
                //        break;
                //}

                //hostTransform.position += positionUpdate;

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