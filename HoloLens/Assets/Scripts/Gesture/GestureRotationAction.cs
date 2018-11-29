using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace Gestures
{
    public class GestureRotationAction : 
        AConstrainedGesture,
        IManipulationHandler, 
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
                eventData.Use();
            }
        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if(isTapped)
            {
                Vector3 rotationVector = new Vector3();
                float rotationFactor = .5f;

                var cumulativeUpdate = eventData.CumulativeDelta;

                switch(rotationConstraint)
                {
                    case AxisAndPlaneConstraint.X_AXIS_ONLY:
                        rotationFactor = cumulativeUpdate.x * RotationSensitivity;
                        rotationVector.x = -1 * rotationFactor;
                        break;
                    case AxisAndPlaneConstraint.Z_AXIS_ONLY:
                        rotationFactor = cumulativeUpdate.x * RotationSensitivity;
                        rotationVector.z = -1 * rotationFactor;
                        break;
                    default: // case: Y_AXIS_ONLY:
                        rotationFactor = cumulativeUpdate.x * RotationSensitivity;
                        rotationVector.y = -1 * rotationFactor;
                        break;
                }


                hostTransform.Rotate(rotationVector);
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