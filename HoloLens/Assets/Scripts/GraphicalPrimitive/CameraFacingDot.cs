using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingDot : MonoBehaviour
{
    void Update()
    {
            var camera = GameObject.FindGameObjectWithTag("MainCamera");
            gameObject.transform.LookAt(camera.transform);
    }
}
