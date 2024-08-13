using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera cam;
    private float camSpeed;

    private void Awake()
    {
        TryGetComponent(out cam);
    }

    private void Update()
    {
        //float x = Input.GetAxisRaw
    }
}
