using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Obstacle>(out var obstacle))
        {
            obstacle.Despawn();
            GameManager.Instance.obstaclePool.Enqueue(obstacle);
        }
    }
}
