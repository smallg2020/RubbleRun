using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public Vector3 tileSize;
    [SerializeField]
    Transform tileStartT, tileEndT;
    [SerializeField]
    Transform activeTilesParent, storedTilesParent;

    [SerializeField]
    GameObject[] firstTiles;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (storedTilesParent.childCount > 0)
        {
            for (int i = 0; i < storedTilesParent.childCount; i++)
            {
                storedTilesParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        CreateFirstMap();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTilePositions();
        MoveTiles();
    }

    void CheckTilePositions()
    {
        for (int i = 0; i < activeTilesParent.childCount; i++)
        {
            Transform tileT = activeTilesParent.GetChild(i);
            if (tileT.position.z >= tileEndT.position.z)
            {
                RemoveTile(tileT);
                AddNewTile();
            }
        }
    }

    void MoveTiles()
    {
        for (int i = 0; i < activeTilesParent.childCount; i++)
        {
            Transform tileT = activeTilesParent.GetChild(i);
            tileT.position += tileT.forward * gameManager.playerSpeed.z * Time.deltaTime;
        }
    }

    void RemoveTile(Transform t)
    {
        t.SetParent(storedTilesParent);
        t.gameObject.SetActive(false);
    }

    void AddNewTile()
    {
        Transform tileT = storedTilesParent.GetChild(Random.Range(0, storedTilesParent.childCount));
        Vector3 pos = activeTilesParent.GetChild(activeTilesParent.childCount - 1).position - new Vector3(0, 0, tileSize.z);
        //Debug.Log("last child", activeTilesParent.GetChild(activeTilesParent.childCount - 1).gameObject);
        tileT.SetParent(activeTilesParent);
        //print(pos);
        tileT.position = pos;
        tileT.gameObject.SetActive(true);
    }

    void CreateFirstMap()
    {
        int tilesToPlace = Mathf.RoundToInt((Mathf.Abs(tileEndT.position.z) + Mathf.Abs(tileStartT.position.z)) / tileSize.z) + 1;
        for (int i = 0; i < tilesToPlace; i++)
        {
            int id;
            Transform tileT = transform;
            if (i < 3)
            {
                tileT = firstTiles[i].transform;
            }
            else
            {
                id = Random.Range(0, storedTilesParent.childCount);
                tileT = storedTilesParent.GetChild(id);
            }
            tileT.SetParent(activeTilesParent);
            tileT.position = tileEndT.position - new Vector3(0, 0, i * tileSize.z);
            tileT.gameObject.SetActive(true);
        }
    }
}
