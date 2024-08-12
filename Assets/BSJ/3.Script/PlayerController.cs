using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private LineRenderer lineRenderer;
    private bool isDragging = false;

    public float forceMultiplier = 10f; // ���޽��� ������ ����
    public int circleSegments = 100; // ���� �����ϴ� ���׸�Ʈ ��
    public float maxRadius = 1.0f; // ���� �ִ� ������
    private float currentRadius = 0f; // ���� ���� ������

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // LineRenderer ����
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = circleSegments + 1; // ���� �׸��� ���� �ʿ��� ���� ��
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
                startPoint = new Vector3 (hit.point.x,17.58f, hit.point.z) ;
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
                endPoint = hit.point;

                // �巡�׿� ���� ���� �������� ��� (�ִ� �������� 1.0f)
                currentRadius = Mathf.Clamp(Vector3.Distance(startPoint, endPoint), 0f, maxRadius);

                // ���� ������Ʈ
                DrawCircle(currentRadius);

                // ���η������� �׷����� ������ ������Ʈ (���콺�� ���� ����)
                Vector3 direction = endPoint - startPoint;
                lineRenderer.SetPosition(circleSegments, transform.position);
                lineRenderer.SetPosition(circleSegments + 1, transform.position + direction.normalized * currentRadius);
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
    void DrawCircle(float radius)
    {
        float angle = 0f;
        float angleStep = 360f / circleSegments;

        for (int i = 0; i < circleSegments + 1; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, 0f, z)); // XZ ��鿡�� ���� �׸�
            angle += angleStep;
        }
    }
}
