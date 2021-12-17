using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Vector3 playerMaxSpeed;
    public Vector3 playerSpeed;
    public int lives = 0;
    public int maxHealth = 10;
    public int health = 10;
    public float obstacleDelay = 3;
    public bool isDead = false;
    public float sizeIncreaseOverDistance = 0.01f;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        isDead = true;
        float t = 1;
        while (t > 0)
        {
            float ft = Time.deltaTime;
            if (playerSpeed.z > 0)
            {
                playerSpeed.z -= ft * 2;
            }

            t -= ft;
            yield return null;
        }
        playerController.Restore();
        isDead = false;
    }
}
