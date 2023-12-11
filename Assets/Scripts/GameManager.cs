using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = GameObject.Find("Manager").GetComponent<GameManager>();
            return _instance;
        }
    }

    private static bool _isPlaying = false;
    public static bool IsPlaying => _isPlaying;

    private float standardSpeed = 50f;
    [SerializeField] private float speed = 50f;
    private float speedThreshold = 20f;
    public float Speed => _isPlaying ? speed : 0;

    [SerializeField] private Obstacle obstaclePrefab;
    public Queue<Obstacle> obstaclePool;
    public Transform creater, destroyer;
    private float obstaclePositionThreshold = 4.2f;

    private float currentTime = 0;
    [SerializeField] private float spawnTime;

    [SerializeField] private TcpConnector tcpConnector;
    
    [SerializeField] private GameObject waitingUI;
    [SerializeField] private GameObject gameOverUI;

    private bool deactivatedWatingUI = false;
    // private UnityEvent startGameEvent;

    private void Awake()
    {
        _instance = this;
        // startGameEvent = new UnityEvent();
        // startGameEvent.AddListener(() => waitingUI.SetActive(false));
    }

    private void Start()
    {
        obstaclePool = new Queue<Obstacle>(10);
        foreach (Transform obstacleTransform in creater)
        {
            var obstacle = obstacleTransform.GetComponent<Obstacle>();
            obstaclePool.Enqueue(obstacle);
            obstacle.Despawn();
        }
    }

    private void Update()
    {
        if (!IsPlaying) return;

        if (!deactivatedWatingUI)
        {
            waitingUI.SetActive(false);
            deactivatedWatingUI = true;
        }
        
        currentTime += Time.deltaTime;
        if (currentTime >= spawnTime)
        {
            currentTime -= spawnTime;
            if (obstaclePool.Count == 0)
            {
                var obstacle = Instantiate(obstaclePrefab);
                obstaclePool.Enqueue(obstacle);
                obstacle.Despawn();
            }
            var newObstacle = obstaclePool.Dequeue();
            var spawnXPosition = Random.Range(-obstaclePositionThreshold, obstaclePositionThreshold);
            var position = creater.position;
            newObstacle.Spawn(new Vector3(spawnXPosition + position.x, position.y, position.z));
        }
        
    }

    public void ControlSpeed(float pitch, float pitchThreshold)
    {
        if (!GameManager.IsPlaying) return;
        speed = standardSpeed + Mathf.Clamp(pitch * speedThreshold / pitchThreshold, -speedThreshold, speedThreshold);
    }

    public void StartGame()
    {
        Debug.Log("Starting Game!");
        _isPlaying = true;
        // startGameEvent.Invoke();
    }

    public void GameOver()
    {
        _isPlaying = false;
        gameOverUI.SetActive(true);
        tcpConnector.StopTCP();
    }

}
