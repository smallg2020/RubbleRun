using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    int scoreReward = 1;
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
        gameManager.UpdateScore(scoreReward);
        Destroy(gameObject);
    }
}
