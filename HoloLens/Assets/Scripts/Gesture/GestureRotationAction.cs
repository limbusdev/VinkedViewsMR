using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace Gestures
{
    public class GestureRotationAction : AConstrainedGesture, INavigationHandler, IFocusable, IInputHandler
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


        void Awake()
        {
            if(hostTransform == null)
                hostTransform = transform;
        }

        void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
        {
            if(isTapped)
            {
                InputManager.Instance.PushModalInputHandler(gameObject);
                eventData.Use();
            }
        }

        void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
        {
            if(isTapped)
            {
                Vector3 rotationVector = new Vector3();
                float rotationFactor = 1f;
                switch(rotationConstraint)
                {
                    case AxisAndPlaneConstraint.X_AXIS_ONLY:
                        rotationFactor = eventData.NormalizedOffset.y * RotationSensitivity;
                        rotationVector.x = -1 * rotationFactor;
                        break;
                    case AxisAndPlaneConstraint.Z_AXIS_ONLY:
                        rotationFactor = eventData.NormalizedOffset.y * RotationSensitivity;
                        rotationVector.z = -1 * rotationFactor;
                        break;
                    default: // case: Y_AXIS_ONLY:
                        rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;
                        rotationVector.y = -1 * rotationFactor;
                        break;
                }


                hostTransform.Rotate(rotationVector);
            }
        }

        void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
        {
                InputManager.Instance.PopModalInputHandler();
                eventData.Use();
        }

        void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
            eventData.Use();
        }

        public void OnInputDown(InputEventData eventData)
        {
            if(hasFocus)
                isTapped = true;
        }

        public void OnInputUp(InputEventData eventData)
        {
            isTapped = false;
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