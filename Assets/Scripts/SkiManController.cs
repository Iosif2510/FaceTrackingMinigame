using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiManController : MonoBehaviour
{
    // public FlameController headController;

    [SerializeField] private float xThreshold;

    private Rigidbody rb;
    private Vector3 currentPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPosition = rb.position;
    }

    public void MovePosition(float roll, float rollThreshold)
    {
        if (!GameManager.IsPlaying) return;
        float xPos = -Mathf.Clamp(roll * xThreshold / rollThreshold, -xThreshold, xThreshold);
        rb.MovePosition(new Vector3(xPos, currentPosition.y, currentPosition.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
