using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoard : MonoBehaviour
{
    private Rigidbody rigid;
    private void Awake()
    {
        TryGetComponent(out rigid);
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            
        //    rigid.isKinematic = false;

        //    rigid.AddForce(Vector3.up * 100f + Vector3.forward * 30f, ForceMode.Impulse);
        //    rigid.angularVelocity = Vector3.right * 100f;
        //}

    }
}
