using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelController : MonoBehaviour
{
    [SerializeField]
    Transform startT, endT, activeT;
    [SerializeField]
    GameObject[] tunnelPrefabs;
    [SerializeField]
    LayerMask groundLayers, blockedLayer;

    GameManager gameManager;
    float newTunnelDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTunnelPositions();
        MoveTunnels();
        SpawnTunnels();
    }

    void CheckTunnelPositions()
    {
        if (activeT.childCount > 0)
        {
            for (int i = 0; i < activeT.childCount; i++)
            {
                Transform ct = activeT.GetChild(i);
                if (ct.position.z >= endT.position.z)
                {
                    Destroy(ct.gameObject);
                }
            }
        }
    }

    void MoveTunnels()
    {
        if (activeT.childCount > 0)
        {
            for (int i = 0; i < activeT.childCount; i++)
            {
                Transform ct = activeT.GetChild(i);
                ct.position += ct.forward * gameManager.playerSpeed.z * Time.deltaTime;
            }
        }
    }

    void SpawnTunnels()
    {
        newTunnelDelay += gameManager.playerSpeed.z * Time.deltaTime;
        if (newTunnelDelay > gameManager.tunnelDelay)
        {
            if (AddNewTunnel())
            {
                //print("added a Tunnel");
                newTunnelDelay = 0;
            }
            else
            {
                newTunnelDelay *= 0.95f;
            }
        }
    }

    bool AddNewTunnel()
    {
        bool isValid = false;
        int id = Random.Range(0, tunnelPrefabs.Length);
        Vector3 pos = startT.position;
        Ray ray = new Ray(pos + Vector3.up * 100, Vector3.down);
        if (Physics.SphereCast(ray, 3, 200, blockedLayer, QueryTriggerInteraction.Collide))
        {
            isValid = false;
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 200, groundLayers))
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
        GameObject c = Instantiate(tunnelPrefabs[id], activeT);
        c.transform.position = pos;
        return true;
    }

    public void RemoveTunnels()
    {
        if (activeT.childCount > 0)
        {
            for (int i = 0; i < activeT.childCount; i++)
            {
                Destroy(activeT.GetChild(i).gameObject);
            }
        }
    }

}
