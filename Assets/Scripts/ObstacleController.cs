using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    Transform obstacleStartT, obstacleEndT;
    [SerializeField]
    float nearDist = 0.3f;
    [SerializeField]
    Transform activeobstaclesParent, storedobstaclesParent;
    [SerializeField]
    LayerMask groundLayers;

    TileController tileController;
    [SerializeField]
    Transform activeTilesT;

    GameManager gameManager;
    float newObstacleDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (storedobstaclesParent.childCount > 0)
        {
            for (int i = 0; i < storedobstaclesParent.childCount; i++)
            {
                storedobstaclesParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        tileController = FindObjectOfType<TileController>();
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
                newObstacleDelay = gameManager.obstacleDelay * 0.99f;
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
            if (Vector3.Distance(obstacleT.position, obstacleStartT.position) < nearDist)
            {
                RemoveObstacle(obstacleT);
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

    void RemoveObstacle(Transform t)
    {
        if (t.TryGetComponent<FragmentsController>(out FragmentsController fragmentsController))
        {
            fragmentsController.ResetFragments();
        }
        else
        {
            if (t.TryGetComponent<NPC>(out NPC npc))
            {
                npc.enabled = true;
                npc.ResetNPC();
            }
            t.SetParent(storedobstaclesParent);
            t.gameObject.SetActive(false);
        }
    }

    bool AddNewObstacle()
    {
        if (storedobstaclesParent.childCount == 0)
        {
            return false;
        }
        Transform obstacleT = storedobstaclesParent.GetChild(Random.Range(0, storedobstaclesParent.childCount));
        bool isValid = false;
        Transform lastTile = activeTilesT.GetChild(activeTilesT.childCount - 1);
        Ray ray = new Ray(lastTile.position + Vector3.up * 100, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 200, groundLayers))
        {
            if (hit.point.y > -0.4f && hit.point.y < 0.4f)
            {
                isValid = true;
            }
        }
        if (!isValid)
        {
            return false;
        }
        Vector3 pos = lastTile.position + new Vector3(0, 0, 0);
        //Debug.Log("last child", activeobstaclesParent.GetChild(activeobstaclesParent.childCount - 1).gameObject);
        obstacleT.SetParent(activeobstaclesParent);
        obstacleT.position = pos;
        obstacleT.gameObject.SetActive(true);
        return true;
    }
}
