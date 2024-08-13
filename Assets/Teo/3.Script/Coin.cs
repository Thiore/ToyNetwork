using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float Force;

    private void Awake()
    {
        TryGetComponent(out rb);
    }
   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            rb.AddForce(Vector3.up * Force, ForceMode.Impulse);
            float rand = Random.Range(0, 1f);
            rb.angularDrag = rand;
            rb.angularVelocity = Vector3.left * 50f;
        }
    }
}
