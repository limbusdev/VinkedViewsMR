using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AGestureFeedbackHook : MonoBehaviour
{
    public abstract void FeedbackStart();
    public abstract void FeedbackUpdate();
    public abstract void FeedbackEnd();
}
