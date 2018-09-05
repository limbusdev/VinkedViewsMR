// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace Academy
{
    /// <summary>
    /// GestureAction performs custom actions based on 
    /// which gesture is being performed.
    /// </summary>
    public class GestureTranslationAction : MonoBehaviour, INavigationHandler, IManipulationHandler
    {
        private Vector3 manipulationOriginalPosition = Vector3.zero;

        void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
        }

        void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
        {

        }

        void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {

                InputManager.Instance.PushModalInputHandler(gameObject);

                manipulationOriginalPosition = transform.position;

        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
                transform.position = manipulationOriginalPosition + eventData.CumulativeDelta;
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