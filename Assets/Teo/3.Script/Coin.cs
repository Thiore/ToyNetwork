using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Coin : NetworkBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float Force;
    [SerializeField] private GameObject Player_1P;
    
    private Vector3 coinUp;
    private bool isThrow;
    private PutOn puton;
    
    private void Awake()
    {
        TryGetComponent(out rb);
        isThrow = false;
        puton = Player_1P.GetComponent<PutOn>();
        

    }
   
    private void Update()
    {
        if (hasAuthority) return; //�����÷��̾

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(transform.up);
            //rb.AddForce(Vector3.up * Force, ForceMode.Impulse);
            //float rand = Random.Range(0, 1f);
            //rb.angularDrag = rand;
            //rb.angularVelocity = Vector3.left * 50f;

            Debug.Log(transform.up);
            CmdBounceCoin();


        }
        
        if (isThrow)
            return;
        if (coinUp != Vector3.zero && !isThrow)
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
    [Command]  // Ŭ���̾�Ʈ���� ������ ����� ������ �޼���
    private void CmdBounceCoin()
    {
        Debug.Log("CmdBounceCoin() ȣ���");
        RpcCoin();  // ��� Ŭ���̾�Ʈ���� ���ο� ���� ���ϴ� RPC ȣ��
    }

    [ClientRpc]
    private void RpcCoin()
    {
        Debug.Log("RpcCoin() ȣ���");
        rb.AddForce(Vector3.up * Force, ForceMode.Impulse);
        float rand = Random.Range(0, 1f);
        rb.angularDrag = rand;
        rb.angularVelocity = Vector3.left * 50f;
    }

    private void OnCollisionExit(Collision collision)
    {
        coinUp = Vector3.zero;
        isThrow = true;
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
            puton.isGameStart = true;
            Invoke("Coin_False", 3f);


        }
        else if (type == "White")
        {
            puton.isGameStart = true;
            Invoke("Coin_False", 3f);
        }
    }

    private void Coin_False()
    {
        gameObject.SetActive(false);
    }

}
