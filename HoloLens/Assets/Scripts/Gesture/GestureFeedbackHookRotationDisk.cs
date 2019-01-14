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