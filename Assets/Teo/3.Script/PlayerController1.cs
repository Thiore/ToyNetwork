using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private LineRenderer lineRenderer;
    private bool isDragging = false;
    private bool isDie;
    public float forceMultiplier = 10f; // ���޽��� ������ ����
    public int circleSegments = 100; // ���� �����ϴ� ���׸�Ʈ ��
    public float maxRadius = 3.0f; // ���� �ִ� ������
    private float currentRadius = 0f; // ���� ���� ������

    private PutOn player;

    private void OnEnable()
    {
        isDie = false;
        transform.parent.TryGetComponent(out player);
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
        lineRenderer.positionCount = circleSegments+2; // ���� �׸��� ���� �ʿ��� ���� ��
        lineRenderer.useWorldSpace = false; // ���� ��ǥ�� ���
        lineRenderer.enabled = false; // �巡���� ���� ���̰� ����
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                // ���콺�� Ŭ������ ��
                //����
                startPoint = new Vector3 (hit.point.x,0.2f, hit.point.z) ;
                //Debug.Log(startPoint);
                isDragging = true;
                lineRenderer.enabled = true; // ���η����� ���̰� ����
            }
        }

        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            // ���콺�� ������ ��
            isDragging = false;
            lineRenderer.enabled = false; // ���η����� �����

            // ���콺�� ��� �ݴ� �������� �� ����
            Vector3 forceDirection = endPoint - startPoint;
            rb.AddForce(-forceDirection.normalized * forceMultiplier * currentRadius, ForceMode.Impulse);
        }
    }

    

    // �� �׸���
    void DrawCircle(float radius, Vector3 dir)
    {
        //����
        float angle;
        if (dir.z>0)
            angle = -Vector3.Angle(Vector3.left,dir);
        else
            angle = Vector3.Angle(Vector3.left, dir);
        //Debug.Log(angle);
        float angleStep = 360f / circleSegments;

        for (int i = 0; i < circleSegments+1; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, 0.2f, z)); // XZ ��鿡�� ���� �׸�
            angle += angleStep;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor")&&!isDie)
        {
            isDie = true;
            Invoke("InvokeDie", 1f);
        }
    }

    private void InvokeDie()
    {
        
        player.Die(gameObject);
    }
    

}
