using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using UnityEngine;

namespace Gestures
{
    public enum AxisAndPlaneConstraint
    {
        NONE, X_AXIS_ONLY, Y_AXIS_ONLY, Z_AXIS_ONLY, XY_PLANE_ONLY, XZ_PLANE_ONLY, YZ_PLANE_ONLY
    }

    public class GestureTranslationAction : MonoBehaviour, IManipulationHandler
    {
        [SerializeField]
        [Tooltip("Transform that will be dragged. Default is the components GameObject.")]
        private Transform hostTransform = null;

        [SerializeField]
        [Tooltip("Constrain translation along an axis.")]
        private AxisAndPlaneConstraint translationConstraint = AxisAndPlaneConstraint.NONE;

        private Vector3 manipulationOriginalPosition = Vector3.zero;



        void Awake()
        {
            if(hostTransform == null) hostTransform = transform;
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {

                InputManager.Instance.PushModalInputHandler(gameObject);

                manipulationOriginalPosition = hostTransform.position;

        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            Vector3 positionUpdate = eventData.CumulativeDelta;

            switch(translationConstraint)
            {
                case AxisAndPlaneConstraint.X_AXIS_ONLY:
                    positionUpdate.y = 0;
                    positionUpdate.z = 0;
                    break;
                case AxisAndPlaneConstraint.Y_AXIS_ONLY:
                    positionUpdate.x = 0;
                    positionUpdate.z = 0;
                    break;
                case AxisAndPlaneConstraint.Z_AXIS_ONLY:
                    positionUpdate.z = positionUpdate.x; // Map X-Component of Gesture to z-Position
                    positionUpdate.x = 0;
                    positionUpdate.y = 0;
                    break;
                case AxisAndPlaneConstraint.XY_PLANE_ONLY:
                    positionUpdate.z = 0;
                    break;
                case AxisAndPlaneConstraint.XZ_PLANE_ONLY:
                    positionUpdate.y = 0;
                    break;
                case AxisAndPlaneConstraint.YZ_PLANE_ONLY:
                    positionUpdate.z = positionUpdate.x;
                    positionUpdate.x = 0;
                    break;
                default: // case NONE:
                    break;
            }

            hostTransform.position = manipulationOriginalPosition + positionUpdate;
        }

        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }
    }
}