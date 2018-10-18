using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace Gestures
{
    public class GestureRotationAction : AConstrainedGesture, INavigationHandler, IHoldHandler
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

        public bool hold = false;
        


        void Awake()
        {
            if (hostTransform == null) hostTransform = transform;
        }

        void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
        }

        void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
        {
            if(true /*hold*/)
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
        }

        void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        // HoldHandler
        public void OnHoldStarted(HoldEventData eventData)
        {
            eventData.Use();
            hold = true;
        }

        public void OnHoldCompleted(HoldEventData eventData)
        {
            eventData.Use();
            hold = false;
        }

        public void OnHoldCanceled(HoldEventData eventData)
        {
            eventData.Use();
            //hold = false;
        }
    }
}