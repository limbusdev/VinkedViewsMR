using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestures
{
    public class GestureFeedbackHookRotationDisk : AGestureFeedbackHook, INavigationHandler
    {
        public GameObject Disk;
        public GameObject Handle;

        public Material notHoldMat, HoldMat;


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