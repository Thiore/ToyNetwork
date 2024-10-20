using UnityEngine;

public class Kick_Chip : MonoBehaviour
{
    private Rigidbody rb;
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
    private PutOn putOn_Player;

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
    }
 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            //Debug.Log("눌림");
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            Debug.Log(cam.transform.position);
            Debug.Log(cam.transform.rotation);

            Ray ray =cam.ScreenPointToRay(Input.mousePosition);
            //GameObject bb = Instantiate(lb);
            Physics.Raycast(ray, out RaycastHit hitt);
            //bb.transform.position = hitt.point;
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                // 마우스를 클릭했을 때
                //수정
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

                DrawCircle(currentRadius, direction);
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
            rb.AddForce(-forceDirection.normalized * forceMultiplier * currentRadius, ForceMode.Impulse);
        }
    }
    public void SetPutOn(PutOn put)
    {
        putOn_Player = put;
        cam = putOn_Player.GetComponent<Camera>();
    }
    // 원 그리기
    void DrawCircle(float radius, Vector3 dir)
    {
        //수정
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

            lineRenderer.SetPosition(i, new Vector3(x, 0.2f, z)); // XZ 평면에서 원을 그림
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
    }
    private void InvokeDie()
    {

        putOn_Player.Die(gameObject);
    }
}
