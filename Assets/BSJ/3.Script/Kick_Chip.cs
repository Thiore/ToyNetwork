using UnityEngine;
using Mirror;

public class Kick_Chip : NetworkBehaviour
{
    public Rigidbody rb;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private LineRenderer lineRenderer;
    private bool isDragging = false;
    private bool isDie;
    public float forceMultiplier = 10f; // 임펄스의 강도를 조정
    public int circleSegments = 200; // 원을 구성하는 세그먼트 수
    public float maxRadius = 50f; // 원의 최대 반지름
    private float currentRadius = 0f; // 원의 현재 반지름
    private Camera cam;
    //[SerializeField] private GameObject lb;

    public PutOn player = null;
    public PlayerType type;

    private RPC_GO_BSJ rpc_go_bsj;
    //public bool ismine = false;

    private void OnEnable()
    {
        isDie = false;
    }

    void Start()
    {

        rb = GetComponent<Rigidbody>();

        // LineRenderer 설정
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        //수정
        lineRenderer.positionCount = circleSegments + 2; // 원을 그리기 위해 필요한 점의 수
        lineRenderer.useWorldSpace = false; // 로컬 좌표를 사용
        lineRenderer.enabled = false; // 드래그할 때만 보이게 설정

        if (!isClient) return;


        cam = player.GetComponent<Camera>();
        //TryGetComponent(out MeshRenderer chipRen);
        //if (type.Equals(PlayerType.Black))
        //    chipRen.material.color = Color.black;
        //else
        //    chipRen.material.color = Color.white;

        //rpc_go_bsj = GetComponent<RPC_GO_BSJ>();

        //gameObject.layer = LayerMask.NameToLayer((player.playerType).ToString());
        // 클라이언트 권한 부여를 서버에서 처리할 수 있도록 함
        //if (!isServer)
        //{
        //    // 권한을 부여할 필요가 있을 때 서버에서 클라이언트 권한을 부여합니다.
        //    // 이 부분은 실제 게임 로직에 맞게 조정 필요
        //    foreach (var conn in NetworkServer.connections.Values)
        //    {
        //        // 특정 오브젝트에 권한을 부여할 클라이언트를 찾는 로직
        //        // 예를 들어, 플레이어가 특정 오브젝트와 상호작용할 때
        //        var identity = GetComponent<NetworkIdentity>();
        //        identity.AssignClientAuthority(conn);
        //    }
        //}
    }

    void Update()
    {
        if (player == null) return;
        if (!(player.isLocalPlayer)) return;
        if (!GameManager.instance.isGameStart) return;
        if (!(player.isMyTurn).Equals(GameManager.instance.isTurn)) return;

        //GameObject ismine_ob;
        if (Input.GetMouseButtonDown(0))
        {

            //Debug.Log("눌림");
            rb.angularVelocity = Vector3.zero;
            
            transform.eulerAngles = Vector3.zero;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //GameObject bb = Instantiate(lb);
            Physics.Raycast(ray, out RaycastHit hitt);
            // bb.transform.position = hitt.point;
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                Debug.Log("이거 들어오나");
                // 마우스를 클릭했을 때
                //수정
                //ismine_ob = hit.collider.gameObject;
                startPoint = new Vector3(hit.point.x, 0.2f, hit.point.z);
                //Debug.Log(startPoint);
                isDragging = true;
                lineRenderer.enabled = true; // 라인렌더러 보이게 설정
            }
        }

        if (isDragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 마우스를 드래그할 때
                //수정
                endPoint = new Vector3(hit.point.x, 0.2f, hit.point.z);

                // 드래그에 따라 원의 반지름을 계산 (최대 반지름은 1.0f)
                currentRadius = Mathf.Clamp(Vector3.Distance(startPoint, endPoint), 0f, maxRadius);

                // 원을 업데이트

                //수정
                // 라인렌더러가 그려지는 방향을 업데이트 (마우스를 당기는 방향)
                Vector3 direction = endPoint - startPoint;
                //lineRenderer.SetPosition(0, new Vector3(transform.position.x, 0.2f, transform.position.z) + direction.normalized * currentRadius);

                DrawCircle(currentRadius, direction.normalized);
                //lineRenderer.SetPosition(circleSegments+1, new Vector3(transform.position.x, 0.2f, transform.position.z) - direction.normalized * currentRadius);
                //Debug.Log("Line시작 : " + transform.position);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Debug.Log("놨다");
            // 마우스를 놓았을 때
            isDragging = false;
            lineRenderer.enabled = false; // 라인렌더러 숨기기

            // 마우스를 당긴 반대 방향으로 힘 적용
            Vector3 forceDirection = endPoint - startPoint;
            
            Vector3 force = -forceDirection.normalized * forceMultiplier * currentRadius;
    
            player.CmdKickEgg(force, netId);
            
        }
    }

    // 원 그리기
    void DrawCircle(float radius, Vector3 dir)
    {
        //수정
        float angle;
        Vector3 CompareVec3 = new Vector3(1f, 0.2f, 0f);
        if (dir.z > 0)
            angle = Vector3.Angle(CompareVec3, dir);
        else
            angle = -Vector3.Angle(CompareVec3, dir);
        //Debug.Log(angle);
        float angleStep = 360f / circleSegments;

        for (int i = 0; i < circleSegments + 1; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(-x, 0.2f, -z)); // XZ 평면에서 원을 그림
            angle += angleStep;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (isClient)
            {
                player.Die(gameObject);
            }
               
            Destroy(gameObject, 1f);
        }


    }

}

   


