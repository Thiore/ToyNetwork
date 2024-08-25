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
    public float forceMultiplier = 10f; // ���޽��� ������ ����
    public int circleSegments = 200; // ���� �����ϴ� ���׸�Ʈ ��
    public float maxRadius = 50f; // ���� �ִ� ������
    private float currentRadius = 0f; // ���� ���� ������
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

        // LineRenderer ����
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        //����
        lineRenderer.positionCount = circleSegments + 2; // ���� �׸��� ���� �ʿ��� ���� ��
        lineRenderer.useWorldSpace = false; // ���� ��ǥ�� ���
        lineRenderer.enabled = false; // �巡���� ���� ���̰� ����

        if (!isClient) return;


        cam = player.GetComponent<Camera>();
        //TryGetComponent(out MeshRenderer chipRen);
        //if (type.Equals(PlayerType.Black))
        //    chipRen.material.color = Color.black;
        //else
        //    chipRen.material.color = Color.white;

        //rpc_go_bsj = GetComponent<RPC_GO_BSJ>();

        //gameObject.layer = LayerMask.NameToLayer((player.playerType).ToString());
        // Ŭ���̾�Ʈ ���� �ο��� �������� ó���� �� �ֵ��� ��
        //if (!isServer)
        //{
        //    // ������ �ο��� �ʿ䰡 ���� �� �������� Ŭ���̾�Ʈ ������ �ο��մϴ�.
        //    // �� �κ��� ���� ���� ������ �°� ���� �ʿ�
        //    foreach (var conn in NetworkServer.connections.Values)
        //    {
        //        // Ư�� ������Ʈ�� ������ �ο��� Ŭ���̾�Ʈ�� ã�� ����
        //        // ���� ���, �÷��̾ Ư�� ������Ʈ�� ��ȣ�ۿ��� ��
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

            //Debug.Log("����");
            rb.angularVelocity = Vector3.zero;
            
            transform.eulerAngles = Vector3.zero;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //GameObject bb = Instantiate(lb);
            Physics.Raycast(ray, out RaycastHit hitt);
            // bb.transform.position = hitt.point;
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                Debug.Log("�̰� ������");
                // ���콺�� Ŭ������ ��
                //����
                //ismine_ob = hit.collider.gameObject;
                startPoint = new Vector3(hit.point.x, 0.2f, hit.point.z);
                //Debug.Log(startPoint);
                isDragging = true;
                lineRenderer.enabled = true; // ���η����� ���̰� ����
            }
        }

        if (isDragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // ���콺�� �巡���� ��
                //����
                endPoint = new Vector3(hit.point.x, 0.2f, hit.point.z);

                // �巡�׿� ���� ���� �������� ��� (�ִ� �������� 1.0f)
                currentRadius = Mathf.Clamp(Vector3.Distance(startPoint, endPoint), 0f, maxRadius);

                // ���� ������Ʈ

                //����
                // ���η������� �׷����� ������ ������Ʈ (���콺�� ���� ����)
                Vector3 direction = endPoint - startPoint;
                //lineRenderer.SetPosition(0, new Vector3(transform.position.x, 0.2f, transform.position.z) + direction.normalized * currentRadius);

                DrawCircle(currentRadius, direction.normalized);
                //lineRenderer.SetPosition(circleSegments+1, new Vector3(transform.position.x, 0.2f, transform.position.z) - direction.normalized * currentRadius);
                //Debug.Log("Line���� : " + transform.position);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Debug.Log("����");
            // ���콺�� ������ ��
            isDragging = false;
            lineRenderer.enabled = false; // ���η����� �����

            // ���콺�� ��� �ݴ� �������� �� ����
            Vector3 forceDirection = endPoint - startPoint;
            
            Vector3 force = -forceDirection.normalized * forceMultiplier * currentRadius;
    
            player.CmdKickEgg(force, netId);
            
        }
    }

    // �� �׸���
    void DrawCircle(float radius, Vector3 dir)
    {
        //����
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

            lineRenderer.SetPosition(i, new Vector3(-x, 0.2f, -z)); // XZ ��鿡�� ���� �׸�
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

   


