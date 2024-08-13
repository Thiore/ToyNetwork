using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private LineRenderer lineRenderer;
    private bool isDragging = false;
    private bool isDie;
    public float forceMultiplier = 10f; // 임펄스의 강도를 조정
    public int circleSegments = 100; // 원을 구성하는 세그먼트 수
    public float maxRadius = 3.0f; // 원의 최대 반지름
    private float currentRadius = 0f; // 원의 현재 반지름

    private PutOn player;

    private void OnEnable()
    {
        isDie = false;
        transform.parent.TryGetComponent(out player);
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
        lineRenderer.positionCount = circleSegments+2; // 원을 그리기 위해 필요한 점의 수
        lineRenderer.useWorldSpace = false; // 로컬 좌표를 사용
        lineRenderer.enabled = false; // 드래그할 때만 보이게 설정
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                // 마우스를 클릭했을 때
                //수정
                startPoint = new Vector3 (hit.point.x,0.2f, hit.point.z) ;
                //Debug.Log(startPoint);
                isDragging = true;
                lineRenderer.enabled = true; // 라인렌더러 보이게 설정
            }
        }

        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            // 마우스를 놓았을 때
            isDragging = false;
            lineRenderer.enabled = false; // 라인렌더러 숨기기

            // 마우스를 당긴 반대 방향으로 힘 적용
            Vector3 forceDirection = endPoint - startPoint;
            rb.AddForce(-forceDirection.normalized * forceMultiplier * currentRadius, ForceMode.Impulse);
        }
    }

    

    // 원 그리기
    void DrawCircle(float radius, Vector3 dir)
    {
        //수정
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

            lineRenderer.SetPosition(i, new Vector3(x, 0.2f, z)); // XZ 평면에서 원을 그림
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
