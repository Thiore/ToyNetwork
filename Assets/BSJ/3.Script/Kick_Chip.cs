using UnityEngine;
using Mirror;

public class Kick_Chip : NetworkBehaviour
{
    private Rigidbody rb;
    
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
    
    private PutOn player = null;
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

        player = NetworkClient.localPlayer.GetComponent<PutOn>();
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
        if (!isClient) return;
        
        if (player.setTime > player.startTime) return;

        if (!(player.isMyTurn)) return;
        //GameObject ismine_ob;
        if (Input.GetMouseButtonDown(0))
        {
            
            //Debug.Log("����");
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;

            Ray ray =cam.ScreenPointToRay(Input.mousePosition);
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

                DrawCircle(currentRadius, direction);
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
            //rb.AddForce(-forceDirection.normalized * forceMultiplier * currentRadius, ForceMode.Impulse);
            Vector3 force = -forceDirection.normalized * forceMultiplier * currentRadius;
            //CmdKickEgg(force);
            rb.AddForce(force, ForceMode.Impulse);
            // ��ġ�� ȸ���� ������ ����ȭ ��û
            //if (isLocalPlayer)
            //if (isLocalPlayer)//
            //{
            //int num;
            //for(int i = 0; i < putOn_Player.Chip_Array.Length;i++)
            //{
            //    if (putOn_Player.Chip_Array[i] != gameObject)
            //        continue;
            //    else
            //    {
            //        num = i;
            //        //putOn_Player.CmdKickEgg(num, force);
            //        //putOn_Player.Send_RpcKickEgg(num, force, transform.position, transform.rotation);
            //        break;
            //    }
            //}
            //if (isLocalPlayer)


            //}
            //else
            //{
            //    Debug.Log("CmdKickEgg�� ���� ����");
            //    //CmdKickEgg();
            //}
        }
    }

    [ClientRpc]
    public void RpcChipType(PlayerType type, uint playerid)
    {
        //player = NetworkClient.spawned[playerid].gameObject.GetComponent<PutOn>();
        //cam = player.GetComponent<Camera>();
        //TryGetComponent(out MeshRenderer chipRen);
        //if (type.Equals(PlayerType.Black))
        //    chipRen.material.color = Color.black;
        //else
        //    chipRen.material.color = Color.white;
        
    }

    public void Setplayer(PutOn put)
    {
        player = put;
    }

    [Command]
    public void CmdKickEgg(Vector3 force)
    {
        
        //rb.AddForce(force, ForceMode.Impulse);
        RpcApplyForce(force);
    }

    [ClientRpc]
    void RpcApplyForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    // �� �׸���
    void DrawCircle(float radius, Vector3 dir)
    {
        //����
        float angle;
        if (dir.z > 0)
            angle = -Vector3.Angle(-transform.right, dir);
        else
            angle = Vector3.Angle(-transform.right, dir);
        //Debug.Log(angle);
        float angleStep = 360f / circleSegments;

        for (int i = 0; i < circleSegments + 1; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, 0.2f, z)); // XZ ��鿡�� ���� �׸�
            angle += angleStep;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") && !isDie)
        {
            isDie = true;
            Invoke("InvokeDie", 1f);
        }

        if (!isServer) return;

        if (!collision.gameObject.CompareTag("Chip")) return;


        Vector3 collisionForce = collision.relativeVelocity * rb.mass; // �浹�� ���� �� ���

        PhysicMaterial material = collision.collider.sharedMaterial;
        if (material != null)
        {
            float friction = Mathf.Min(material.dynamicFriction, material.staticFriction);
            float bounciness = material.bounciness;

            // ��: �����¿� ���� �� ����, �ݹ߷¿� ���� �߰� �� ����
            collisionForce *= (1 - friction);
            collisionForce += collision.relativeVelocity * bounciness * rb.mass;
        }

        RpcApplyCollisionForce(collisionForce);
    }

    [Command]
    private void CmdApplyCollisionForce(Vector3 collisionForce)
    {
        //RpcApplyCollisionForce(collisionForce);
        if (isOwned)
            rb.AddForce(-collisionForce, ForceMode.Impulse);
    }

    [ClientRpc]
    private void RpcApplyCollisionForce(Vector3 collisionForce)
    {
        if(isOwned)
            rb.AddForce(collisionForce, ForceMode.Impulse);
    }

    private void InvokeDie()
    {

        player.Die(gameObject);
    }

   

}
