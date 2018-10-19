using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;
using UnityEngine;

namespace Gestures
{
    public class GestureTranslationAction : AConstrainedGesture, INavigationHandler, IFocusable, IInputHandler
    {
        [SerializeField]
        [Tooltip("Transform that will be dragged. Default is the components GameObject.")]
        private Transform hostTransform = null;

        [SerializeField]
        [Tooltip("Constrain translation along an axis.")]
        private AxisAndPlaneConstraint translationConstraint = AxisAndPlaneConstraint.NONE;

        private Vector3 manipulationOriginalPosition = Vector3.zero;

        public bool hasFocus = false;
        public bool isTapped = false;



        void Awake()
        {
            if(hostTransform == null) hostTransform = transform;
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
                Vector3 positionUpdate = (new Vector3(eventData.NormalizedOffset.x, eventData.NormalizedOffset.y, eventData.NormalizedOffset.x))*.01f;

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

                hostTransform.position += positionUpdate;

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