using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    Transform obstacleStartT, obstacleEndT;
    [SerializeField]
    Transform activeobstaclesParent;
    [SerializeField]
    LayerMask groundLayers;
    [SerializeField]
    GameObject[] obstaclePrefabs;

    GameManager gameManager;
    float newObstacleDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckObstaclePositions();
        MoveObstacles();
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        newObstacleDelay += gameManager.playerSpeed.z * Time.deltaTime;
        if (newObstacleDelay > gameManager.obstacleDelay)
        {
            if (AddNewObstacle())
            {
                newObstacleDelay = 0;
            }
            else
            {
                newObstacleDelay *= 0.95f;
            }
        }
    }

    void CheckObstaclePositions()
    {
        if (activeobstaclesParent.childCount == 0)
        {
            return;
        }
        for (int i = 0; i < activeobstaclesParent.childCount; i++)
        {
            Transform obstacleT = activeobstaclesParent.GetChild(i);
            if (obstacleT.position.z >= obstacleEndT.position.z)
            {
                Destroy(obstacleT.gameObject);
            }
        }
    }

    void MoveObstacles()
    {
        if (activeobstaclesParent.childCount == 0)
        {
            return;
        }
        for (int i = 0; i < activeobstaclesParent.childCount; i++)
        {
            Transform obstacleT = activeobstaclesParent.GetChild(i);
            obstacleT.position += obstacleT.forward * gameManager.playerSpeed.z * Time.deltaTime;
        }
    }

    bool AddNewObstacle()
    {
        bool isValid = false;
        Vector3 pos = obstacleStartT.position;
        Ray ray = new Ray(pos + Vector3.up * 100, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 200, groundLayers))
        {
            if (hit.point.y > -0.4f && hit.point.y < 0.4f)
            {
                pos = hit.point;
                isValid = true;
            }
        }
        if (!isValid)
        {
            return false;
        }
        int id = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstacle = Instantiate(obstaclePrefabs[id], activeobstaclesParent);
        obstacle.transform.position = pos;
        return true;
    }
}
