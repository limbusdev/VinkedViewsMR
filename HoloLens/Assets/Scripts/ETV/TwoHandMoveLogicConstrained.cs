﻿/*
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
using UnityEngine;

namespace HoloToolkit.Unity.InputModule.Utilities.Interactions
{
    /// <summary>
    /// Implements a movement logic that uses the model of angular rotations along a sphere whose 
    /// radius varies. The angle to move by is computed by looking at how much the hand changes
    /// relative to a pivot point (slightly below and in front of the head).
    /// 
    /// Usage:
    /// When a manipulation starts, call Setup.
    /// Call Update any time to update the move logic and get a new rotation for the object.
    /// </summary>
    public class TwoHandMoveLogicConstrained
    {
        #region private members
        private float handRefDistance;
        private float objRefDistance;
        private const float DistanceScale = 2f;
        #endregion

        /// <summary>
        /// The initial angle between the hand and the object
        /// </summary>
        private Quaternion m_gazeAngularOffset;

        /// <summary>
        /// The initial object position
        /// </summary>
        private Vector3 m_draggingPosition;

        /// <summary>
        /// This enum value stores the initial constraint specified as an argument in the Constructor.
        /// It may be overridden by runtime conditions. The actual constraint is stored in the
        /// variable currentRotationConstraint.
        /// </summary>
        private AxisConstraint translationConstraint;


        /// <summary>
        /// TwoHandMoveLogicConstrained Constructor
        /// </summary>
        /// <param name="constrainRotation">Enum describing to which axis the rotation is constrained</param>
        public TwoHandMoveLogicConstrained(AxisConstraint constrainTranslation)
        {
            translationConstraint = constrainTranslation;
        }

        /// <summary>
        /// Initialize system with controller/hand states- starting position and current Transform.
        /// </summary>
        /// <param name="startHandPositionMeters">starting position of Controllers/Hands which determine orientation</param>
        /// <param name="manipulationRoot">Transform of gameObject being manipulated</param>
        public void Setup(Vector3 startHandPositionMeters, Transform manipulationRoot)
        {
            var newHandPosition = startHandPositionMeters;

            // The pivot is just below and in front of the head.
            var pivotPosition = GetHandPivotPosition();

            handRefDistance = Vector3.Distance(newHandPosition, pivotPosition);
            objRefDistance = Vector3.Distance(manipulationRoot.position, pivotPosition);

            var objDirectoin = Vector3.Normalize(manipulationRoot.position - pivotPosition);
            var handDirection = Vector3.Normalize(newHandPosition - pivotPosition);

            // We transform the forward vector of the object, the direction of the object, and the direction of the hand
            // to camera space so everything is relative to the user's perspective.
            objDirectoin = CameraCache.Main.transform.InverseTransformDirection(objDirectoin);
            handDirection = CameraCache.Main.transform.InverseTransformDirection(handDirection);

            // Store the original rotation between the hand an object
            m_gazeAngularOffset = Quaternion.FromToRotation(handDirection, objDirectoin);
            m_draggingPosition = manipulationRoot.position;
        }

        /// <summary>
        /// Updates gameobject with new position information of controller/hand
        /// </summary>
        /// <param name="centroid">center of translation to be used for Manipulation</param>
        /// <param name="manipulationObjectPosition">position of gameobject to be manipulated</param>
        /// <returns> a Vector3 describing the updated current Position of the gameObject being two-hand manipulated</returns>
        public Vector3 Update(Vector3 centroid, Vector3 manipulationObjectPosition)
        {
            var newHandPosition = centroid;
            var pivotPosition = GetHandPivotPosition();

            // Compute the pivot -> hand vector in camera space
            var newHandDirection = Vector3.Normalize(newHandPosition - pivotPosition);
            newHandDirection = CameraCache.Main.transform.InverseTransformDirection(newHandDirection);

            // The direction the object should face is the current hand direction rotated by the original hand -> object rotation.
            var targetDirection = Vector3.Normalize(m_gazeAngularOffset * newHandDirection);
            targetDirection = CameraCache.Main.transform.TransformDirection(targetDirection);
            
            // Compute how far away the object should be based on the ratio of the current to original hand distance
            var currentHandDistance = Vector3.Magnitude(newHandPosition - pivotPosition);
            var distanceRatio = currentHandDistance / handRefDistance;
            var distanceOffset = distanceRatio > 0 ? (distanceRatio - 1f) * DistanceScale : 0;
            var targetDistance = objRefDistance + distanceOffset;

            var newPosition = pivotPosition + (targetDirection * targetDistance);

            var newDistance = Vector3.Distance(newPosition, pivotPosition);
            if (newDistance > 4f)
            {
                newPosition = pivotPosition + Vector3.Normalize(newPosition - pivotPosition) * 4f;
            }


            switch (translationConstraint)
            {
                case AxisConstraint.XAxisOnly:
                    m_draggingPosition = new Vector3(newPosition.x, m_draggingPosition.y, m_draggingPosition.z);
                    break;
                case AxisConstraint.YAxisOnly:
                    m_draggingPosition = new Vector3(m_draggingPosition.x, newPosition.y, m_draggingPosition.z);
                    break;
                case AxisConstraint.ZAxisOnly:
                    m_draggingPosition = new Vector3( m_draggingPosition.x, m_draggingPosition.y, newPosition.z);
                    break;
                default: /* NONE */
                    m_draggingPosition = newPosition;
                    break;
            }
           

            return m_draggingPosition;
        }

        ///<summary>
        /// gets current controller/hand position
        /// <returns>A point that is below and just in front of the head.</returns>
        ///</summary>
        public static Vector3 GetHandPivotPosition()
        {
            Vector3 pivot = CameraCache.Main.transform.position; // a bit lower and behind
            return pivot;
        }
    }
}
