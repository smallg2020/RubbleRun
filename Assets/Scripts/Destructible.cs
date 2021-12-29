using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    GameObject[] disableOnHit, enableOnHit;

    GameManager gameManager;
    PlayerController playerController;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(0).otherCollider.attachedRigidbody.CompareTag("Player"))
        {
            HitPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            HitPlayer();
        }
    }

    private void HitPlayer()
    {
        foreach (var g in enableOnHit)
        {
            g.SetActive(true);
        }
        foreach (var g in disableOnHit)
        {
            g.SetActive(false);
        }
        playerController.AddAttachment(transform.tag);
        Destroy(this);
    }
}
