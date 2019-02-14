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