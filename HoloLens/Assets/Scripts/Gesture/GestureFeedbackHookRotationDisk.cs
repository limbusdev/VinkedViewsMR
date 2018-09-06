using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestures
{
    public class GestureFeedbackHookRotationDisk : AGestureFeedbackHook, INavigationHandler
    {
        public GameObject Disk;

        
        public void OnNavigationCanceled(NavigationEventData eventData)
        {
            FeedbackEnd();
        }

        public void OnNavigationCompleted(NavigationEventData eventData)
        {
            FeedbackEnd();
        }

        public void OnNavigationStarted(NavigationEventData eventData)
        {
            FeedbackStart();
        }

        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            FeedbackUpdate();
        }

        public override void FeedbackStart()
        {
            Disk.SetActive(true);
        }

        public override void FeedbackUpdate()
        {
            // Nothing
        }

        public override void FeedbackEnd()
        {
            Disk.SetActive(false);
        }
    }
}