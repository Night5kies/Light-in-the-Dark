using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform cam;
    public Vector3 properOrientation;
    void LateUpdate()
    {
        transform.LookAt(cam.position);
        transform.Rotate(properOrientation);
    }
}
