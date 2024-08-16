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
            Debug.LogError("Center GameObject not found. Please ensure 'go_game_board_0' exists in the scene.");
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
    //        // 로컬 플레이어가 아닌 경우, 메인 카메라 비활성화
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

        // 카메라 위치와 회전을 동기화
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
}
