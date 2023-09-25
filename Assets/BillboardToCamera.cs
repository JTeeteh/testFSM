using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    public Camera camTransform;

    Quaternion originalRotation;

    void Start()
    {
        camTransform = GetComponent<Canvas>().worldCamera;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = camTransform.transform.rotation * originalRotation;
    }
}
