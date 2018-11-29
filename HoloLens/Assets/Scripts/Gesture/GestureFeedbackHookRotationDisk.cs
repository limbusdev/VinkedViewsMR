using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace Gestures
{
    /// <summary>
    /// Constrainable Rotation Component
    /// With this component, any GameObject, containing a mesh, can be made
    /// touch- and rotationable with the given constraints:
    ///     * rotate around X axis
    ///     * rotate around Y axis
    ///     * rotate around Z axis
    /// </summary>
    public class GestureFeedbackHookRotationDisk : 
        AGestureFeedbackHook, 
        IManipulationHandler
    {
        [SerializeField]
        [Tooltip("Mesh to visualize rotation direction.")]
        private GameObject Disk;

        [SerializeField]
        [Tooltip("Mesh that triggers this rotation action.")]
        private GameObject Handle;

        [SerializeField]
        [Tooltip("Default material of the trigger mesh.")]
        private Material notHoldMat;

        [SerializeField]
        [Tooltip("Material, when trigger is active and rotation in action.")]
        private Material HoldMat;


        public void OnManipulationCanceled(ManipulationEventData eventData)
        {
            FeedbackEnd();
        }

        public void OnManipulationCompleted(ManipulationEventData eventData)
        {
            FeedbackEnd();
        }

        public void OnManipulationStarted(ManipulationEventData eventData)
        {
            FeedbackStart();
        }

        public void OnManipulationUpdated(ManipulationEventData eventData)
        {
            FeedbackUpdate();
        }

        public override void FeedbackStart()
        {
            Disk.SetActive(true);
            Handle.GetComponent<MeshRenderer>().material = HoldMat;
        }

        public override void FeedbackUpdate()
        {
            // Nothing
        }

        public override void FeedbackEnd()
        {
            Disk.SetActive(false);
            Handle.GetComponent<MeshRenderer>().material = notHoldMat;
        }

       
    }
}