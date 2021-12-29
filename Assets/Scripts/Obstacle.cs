using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    int damage = 1;
    public float minPosX = 0, maxPosX = 0, minPosY = 0, maxPosY = 0;
    [SerializeField]
    Vector3 minScale = Vector3.one * 0.9f;
    [SerializeField]
    Vector3 maxScale = Vector3.one * 1.1f;
    [SerializeField]
    GameObject[] showWhenHit;
    [SerializeField]
    bool[] moveToPlayerPosition;

    CameraShake cameraShake;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.Lerp(minScale, maxScale, Random.Range(0.0f, 1.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            HitPlayer(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.attachedRigidbody.CompareTag("Player"))
        {
            HitPlayer(collision.collider.gameObject);
        }
    }

    void HitPlayer(GameObject g)
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!gameManager.invincible)
        {
            cameraShake = FindObjectOfType<CameraShake>();
            cameraShake.ShakeCamera();
            gameManager.HurtPlayer(damage);
        }
        if (showWhenHit.Length > 0)
        {
            for (int i = 0; i < showWhenHit.Length; i++)
            {
                showWhenHit[i].SetActive(true);
                if (moveToPlayerPosition[i])
                {
                    showWhenHit[i].transform.position = g.transform.position;
                }
            }
        }
        Destroy(this);
    }
}
