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
    LayerMask groundLayers, blockedLayer;
    [SerializeField]
    GameObject[] obstaclePrefabs;
    Obstacle[] obstacleScripts;

    GameManager gameManager;
    PlayerController playerController;
    float newObstacleDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
        obstacleScripts = new Obstacle[obstaclePrefabs.Length];
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            obstacleScripts[i] = obstaclePrefabs[i].GetComponentInChildren<Obstacle>();
        }
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
        // access the script on the prefab to get settings
        int id = Random.Range(0, obstaclePrefabs.Length);
        Vector3 pos = obstacleStartT.position;
        Obstacle obScript = obstacleScripts[id];
        float xpos = playerController.transform.position.x;
        pos.x = xpos;
        Ray ray = new Ray(pos + Vector3.up * 100, Vector3.down);
        if (Physics.SphereCast(ray, 3, 200, blockedLayer))
        {
            isValid = false;
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 200, groundLayers, QueryTriggerInteraction.Collide))
            {
                if (hit.point.y > -0.4f && hit.point.y < 0.4f)
                {
                    pos = hit.point;
                    isValid = true;
                }
            }
        }
        if (!isValid)
        {
            return false;
        }
        GameObject obstacle = Instantiate(obstaclePrefabs[id], activeobstaclesParent);
        pos.y += Random.Range(obScript.minPosY, obScript.maxPosY);
        obstacle.transform.position = pos;
        return true;
    }

    public void RemoveObstacles()
    {
        if (activeobstaclesParent.childCount > 0)
        {
            for (int i = 0; i < activeobstaclesParent.childCount; i++)
            {
                Destroy(activeobstaclesParent.GetChild(i).gameObject);
            }
        }
    }
}
