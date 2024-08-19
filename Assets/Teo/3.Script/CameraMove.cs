using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraMove : NetworkBehaviour
{
    [SerializeField] private float camSpeed;
    private Transform Center;
    private Camera mainCamera;

    private void Awake()
    {
        Center = GameObject.Find("go_game_board_0")?.transform;
        if (Center == null)
        {
            Debug.LogError("서버에서 바둑판 못찾음");
        }

        if (!isLocalPlayer)
        {
            mainCamera = GameObject.Find("Camera").GetComponent<Camera>();

            //if (mainCamera != null)
            //{
            //    mainCamera.gameObject.SetActive(true);
            //}
        }
    }

    //private void Start()
    //{
    //    if (isLocalPlayer)
    //    {
    //        // ���� �÷��̾ �ƴ� ���, ���� ī�޶� ��Ȱ��ȭ
    //        if (mainCamera != null)
    //        {
    //            mainCamera.gameObject.SetActive(false);
    //        }
    //    }
    //}

    private void Update()
    {
        if (!isLocalPlayer) return;

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        HandleCameraMovement(scroll);
        HandleCameraRotation(horizontal, vertical);
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer) return;

        // ī�޶� ��ġ�� ȸ���� ����ȭ
        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position;
            mainCamera.transform.rotation = transform.rotation;
        }
    }

    private void HandleCameraMovement(float scroll)
    {
        if (scroll != 0)
        {
            float distance = Vector3.Distance(transform.position, Center.position);
            if (distance >= 10f && distance <= 30f)
            {
                if (scroll > 0f)
                {
                    transform.position += transform.forward * Time.deltaTime * camSpeed;
                }
                else if (scroll < 0f)
                {
                    transform.position -= transform.forward * Time.deltaTime * camSpeed;
                }
            }
        }
    }

    private void HandleCameraRotation(float horizontal, float vertical)
    {
        if (horizontal != 0)
        {
            transform.RotateAround(Center.position, Vector3.up, horizontal * Time.deltaTime * camSpeed);
        }

        if (vertical != 0)
        {
            if (vertical < 0 && transform.eulerAngles.x > 31f)
            {
                transform.RotateAround(Center.position, -transform.right, Time.deltaTime * camSpeed);
            }
            else if (vertical > 0 && transform.eulerAngles.x < 89f)
            {
                transform.RotateAround(Center.position, transform.right, Time.deltaTime * camSpeed);
            }
        }
    }
//<<<<<<< HEAD
//    private void LateUpdate()
//    {
//        Vector3 Rot = transform.eulerAngles;
//        float x = Rot.x;
//        x = Mathf.Clamp(x, 30f, 90f);
//        Rot.x = x;
//        transform.eulerAngles = Rot;
//        float Dis = Vector3.Distance(transform.position, Center.position);
//        if (Dis < 10f)
//        {
//            transform.position -= transform.forward * Time.deltaTime * camSpeed;
//        }
//        if (Dis > 30f)
//        {
//            transform.position += transform.forward * Time.deltaTime * camSpeed;
//        }
//    }

//=======
//>>>>>>> origin/BSJ/Basic
}
