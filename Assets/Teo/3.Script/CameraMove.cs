using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float camSpeed;
    [SerializeField] private Transform Center;
    private float scrollDis;

    private void Awake()
    {
        TryGetComponent(out cam);
        if(camSpeed.Equals(0))
        {
            camSpeed = 10f;
        }
        scrollDis = 0;
    }

    private void Update()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (!scroll.Equals(0))
        {
            float Dis = Vector3.Distance(transform.position, Center.position);
            if (Dis >= 10f && Dis <= 30f)
            {
                if (scroll > 0f)
                {
                    transform.localPosition += transform.forward * Time.deltaTime * camSpeed;
                }
                else if (scroll < 0f)
                {
                    transform.localPosition -= transform.forward * Time.deltaTime * camSpeed;
                }

            }
        }
        
        if (horizontal > 0)
        {
            transform.RotateAround(Center.position, Vector3.up, -Time.deltaTime * camSpeed);
        }
        else if (horizontal < 0)
        {
            transform.RotateAround(Center.position, Vector3.up, Time.deltaTime * camSpeed);
        }


        if (vertical < 0)
        {
            if (transform.eulerAngles.x > 31f)
            {
                Debug.Log(vertical);
                transform.RotateAround(Center.position, -transform.right, Time.deltaTime * camSpeed);
            }

        }

        if (vertical > 0)
        {
            if (transform.eulerAngles.x < 89f)
            {
                Debug.Log(vertical);
                transform.RotateAround(Center.position, transform.right, Time.deltaTime * camSpeed);
            }

        }
    }
    private void LateUpdate()
    {
        Vector3 Rot = transform.eulerAngles;
        float x = Rot.x;
        x = Mathf.Clamp(x, 30f, 90f);
        Rot.x = x;
        transform.eulerAngles = Rot;
        float Dis = Vector3.Distance(transform.position, Center.position);
        if (Dis < 10f)
        {
            transform.localPosition -= transform.forward * Time.deltaTime * camSpeed;
        }
        if (Dis > 30f)
        {
            transform.localPosition += transform.forward * Time.deltaTime * camSpeed;
        }
    }

}
