using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    int scoreReward = 1;
    public float minPosX = 0, maxPosX = 0, minPosY = 0, maxPosY = 0;
    [SerializeField]
    GameObject collectEffect;

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
            HitPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.attachedRigidbody.CompareTag("Player"))
        {
            HitPlayer();
        }
    }

    void HitPlayer()
    {
        GameObject effect = Instantiate(collectEffect, null);
        effect.transform.position = transform.position;
        gameManager.UpdateScore(scoreReward);
        Destroy(gameObject);
    }
}
