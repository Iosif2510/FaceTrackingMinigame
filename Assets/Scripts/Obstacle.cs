using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // void Start()
    // {
    //     
    // }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * GameManager.Instance.Speed;
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        rb.WakeUp();
        Debug.Log($"{position}, {transform.position}");
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        
    }
}
