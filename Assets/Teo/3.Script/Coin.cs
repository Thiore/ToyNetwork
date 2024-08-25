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

                //Debug.Log("ȭ��Ʈ");
                //Invoke("SetPlayerType(White)", 3f);
                SetPlayerType("White");

            }
            else if (dotProduct < 0)
            {

                //Debug.Log("��");
                //Invoke("SetPlayerType(Black)", 3f);
                SetPlayerType("Black");
            }
            else
            {
                Debug.Log("������Ʈ�� �����Դϴ�.");
            }
        }


    }
    //[Command]  // Ŭ���̾�Ʈ���� ������ ����� ������ �޼���
    public void CmdBounceCoin()
    {
        if (!isServer) return;
        Debug.Log("���Ҹ�?");
        float rand = Random.Range(0, 1f);
        rb.angularDrag = rand;
        rb.AddForce(Vector3.up * Force, ForceMode.Impulse);

        rb.angularVelocity = Vector3.left * 50f;
        
        Debug.Log("���Ҹ�?");
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
            // �ٴڿ� ������ ��� �����鿡 ���� üũ
            foreach (ContactPoint contact in collision.contacts)
            {
                // ������ ������ ���� ���͸� ������
                Vector3 contactNormal = contact.normal;

                // ���� ���Ϳ� ������ ���� ����(Vector3.up) ������ ���� ��
                float angle = Vector3.Angle(contactNormal, Vector3.up);

                // ������ ����(90��)�� ������� üũ (0�� ������ �ٴڿ� �������� ������ ��)
                if (angle < 10.0f)
                {
                    // ������ �ٴڿ� ������ �پ��ٰ� �Ǵ�
                    Debug.Log("������ �ٴڿ� ������ �����߽��ϴ�.");
                    // ������ ��ȯ�ϴ� ������ ���⿡ �߰�
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
