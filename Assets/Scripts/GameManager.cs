using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    [SerializeField]
    TextMeshProUGUI scoreTxt;
    [SerializeField]
    Vector3 playerStartingSpeed;
    public Vector3 playerSpeed;
    public int lives = 0;
    public int maxHealth = 10;
    public int health = 10;
    public float obstacleDelay = 3;
    public float collectibleDelay = 8;
    public bool isDead = false;
    public float gameSpeed = 1;
    public float gameSpeedIncreaseOverDistance = 0.01f;
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
        // playerSpeed = playerStartingSpeed * gameSpeed;
    }

    private void FixedUpdate()
    {
        gameSpeed += gameSpeedIncreaseOverDistance;
        Time.timeScale = gameSpeed;
    }

    public void UpdateScore(int v)
    {
        score += v;
        UpdateScoreText(score);
    }

    void UpdateScoreText(int v)
    {
        scoreTxt.text = v.ToString();
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
        gameSpeed = 1;
        isDead = false;
    }
}
