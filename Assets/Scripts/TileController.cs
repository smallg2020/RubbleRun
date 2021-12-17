using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField]
    Transform tileStartT, tileEndT;
    [SerializeField]
    float nearDist = 0.3f;
    [SerializeField]
    Transform activeTilesParent, storedTilesParent;

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
            if (Vector3.Distance(tileT.position, tileStartT.position) < nearDist)
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
        Vector3 pos = activeTilesParent.GetChild(activeTilesParent.childCount - 1).position + new Vector3(0, 0, -2.5f);
        //Debug.Log("last child", activeTilesParent.GetChild(activeTilesParent.childCount - 1).gameObject);
        tileT.SetParent(activeTilesParent);
        //print(pos);
        tileT.position = pos;
        tileT.gameObject.SetActive(true);
    }

    void CreateFirstMap()
    {
        int tilesToPlace = Mathf.RoundToInt((Mathf.Abs(tileEndT.position.z) + Mathf.Abs(tileStartT.position.z)) / 2.5f) + 1;
        for (int i = 0; i < tilesToPlace; i++)
        {
            Transform tileT = storedTilesParent.GetChild(Random.Range(0, storedTilesParent.childCount));
            tileT.SetParent(activeTilesParent);
            tileT.position = tileStartT.position + new Vector3(0, 0, i * -2.5f);
            tileT.gameObject.SetActive(true);
        }
    }
}
