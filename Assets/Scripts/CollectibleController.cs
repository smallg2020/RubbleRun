using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField]
    Transform startT, endT, activeT;
    [SerializeField]
    GameObject[] collectiblePrefabs;
    [SerializeField]
    LayerMask groundLayers;

    Collectible[] collectibleScripts;
    GameManager gameManager;
    float newCollectibleDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        collectibleScripts = new Collectible[collectiblePrefabs.Length];
        for (int i = 0; i < collectiblePrefabs.Length; i++)
        {
            collectibleScripts[i] = collectiblePrefabs[i].GetComponent<Collectible>();
        }
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollectiblePositions();
        MoveCollectibles();
        SpawnCollectibles();
    }

    private void CheckCollectiblePositions()
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

    private void MoveCollectibles()
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

    private void SpawnCollectibles()
    {
        newCollectibleDelay += gameManager.playerSpeed.z * Time.deltaTime;
        if (newCollectibleDelay > gameManager.collectibleDelay)
        {
            if (AddNewCollectible())
            {
                //print("added a collectible");
                newCollectibleDelay = 0;
            }
            else
            {
                newCollectibleDelay *= 0.95f;
            }
        }
    }

    bool AddNewCollectible()
    {
        bool isValid = false;
        int id = Random.Range(0, collectiblePrefabs.Length);
        Collectible cScript = collectibleScripts[id];
        float xpos = Random.Range(cScript.minPosX, cScript.maxPosX);
        Vector3 pos = startT.position + new Vector3(xpos, 0, 0);
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
        GameObject c = Instantiate(collectiblePrefabs[id], activeT);
        pos.y += Random.Range(cScript.minPosY, cScript.maxPosY);
        c.transform.position = pos;
        return true;
    }

    public void RemoveCollectibles()
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
