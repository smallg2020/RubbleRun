using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    GameObject splashGO;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            gameManager.Die();
            GameObject splash = Instantiate(splashGO, null);
            splash.transform.position = other.transform.position;
        }
    }
}
