using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Coin : NetworkBehaviour
{
    public static Coin coin;
    private Rigidbody rb;
    [SerializeField] private float Force;

    public List<PutOn> players;
    
    private Vector3 coinUp;
    private bool isThrow;
    private bool isStart = false;
    
    private void Start()
    {
        
        if(coin != this&&coin != null)
        {
            Destroy(coin.gameObject);   
        }
        coin = this;
        TryGetComponent(out rb);
        isThrow = false;

        if(players.Count.Equals(2))
        {
            CmdBounceCoin();
        }
    }

    private void Update()
    {
        //Debug.Log(transform.up);
        //rb.AddForce(Vector3.up * Force, ForceMode.Impulse);
        //float rand = Random.Range(0, 1f);
        //rb.angularDrag = rand;
        //rb.angularVelocity = Vector3.left * 50f;

        if (!isServer) return;

        //CmdBounceCoin();

        if (isThrow&&!isStart) return;

        if (coinUp != Vector3.zero)
        {
            float dotProduct = Vector3.Dot(transform.up, Vector3.up);

            if (dotProduct > 0)
            {

                //Debug.Log("화이트");
                //Invoke("SetPlayerType(White)", 3f);
                SetPlayerType("White");

            }
            else if (dotProduct < 0)
            {

                //Debug.Log("블랙");
                //Invoke("SetPlayerType(Black)", 3f);
                SetPlayerType("Black");
            }
            else
            {
                Debug.Log("오브젝트가 수평입니다.");
            }
        }


    }
    //[Command]  // 클라이언트에서 서버로 명령을 보내는 메서드
    public void CmdBounceCoin()
    {
        if (!isServer) return;
        Debug.Log("나불림?");
        float rand = Random.Range(0, 1f);
        rb.angularDrag = rand;
        rb.AddForce(Vector3.up * Force, ForceMode.Impulse);

        rb.angularVelocity = Vector3.left * 50f;
        
        Debug.Log("나불림?");
    }


    private void OnCollisionExit(Collision collision)
    {
        coinUp = Vector3.zero;
        isThrow = true;
        isStart = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (coinUp == Vector3.zero && isThrow )
        {
            // 바닥에 접촉한 모든 지점들에 대해 체크
            foreach (ContactPoint contact in collision.contacts)
            {
                // 접촉한 지점의 법선 벡터를 가져옴
                Vector3 contactNormal = contact.normal;

                // 법선 벡터와 월드의 위쪽 벡터(Vector3.up) 사이의 각도 비교
                float angle = Vector3.Angle(contactNormal, Vector3.up);

                // 법선이 위쪽(90도)에 가까운지 체크 (0에 가까우면 바닥에 수직으로 착지한 것)
                if (angle < 10.0f)
                {
                    // 동전이 바닥에 완전히 붙었다고 판단
                    Debug.Log("동전이 바닥에 완전히 착지했습니다.");
                    // 위쪽을 반환하는 로직을 여기에 추가
                    coinUp = transform.up;
                    isThrow = false;
                }

            }
        }
    }

    private void SetPlayerType(string type)
    {
        
        if (type == "Black")
        {
            
            players[0].isMyTurn = true;
            players[1].isMyTurn = false;
            Invoke("Coin_False", 2f);


        }
        else if (type == "White")
        {
            players[0].isMyTurn = false;
            players[1].isMyTurn = true;
            Invoke("Coin_False", 2f);
        }
    }

    private void Coin_False()
    {
        GameManager.instance.isSetStart = true;
        DestroyCoin();
        Destroy(gameObject);
    }
    [ClientRpc]
    private void DestroyCoin()
    {
        Destroy(gameObject);
    }

}
