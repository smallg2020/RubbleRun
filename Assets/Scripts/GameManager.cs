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
    public float tunnelDelay = 7;
    public bool isDead = false;
    public float gameSpeed = 1;
    public float gameSpeedIncreaseOverDistance = 0.01f;
    public float sizeIncreaseOverDistance = 0.01f;
    public float invincibilityTime = 0.3f;
    PlayerController playerController;
    AdManager adManager;
    [SerializeField]
    GameObject[] hideOnStart;
    [SerializeField]
    TMP_InputField name_input;
    [SerializeField]
    TextMeshProUGUI name_text, username_UI_text;
    [SerializeField]
    GameObject continueScreen, highScoreScreen;

    HighScoreManager highScoreManager;
    ObstacleController obstacleController;
    TunnelController tunnelController;
    CollectibleController collectibleController;

    public string username;
    float newInvincibilityTime = 0;
    public bool invincible = false;

    // Start is called before the first frame update
    void Start()
    {
        continueScreen.SetActive(false);
        for (int i = 0; i < hideOnStart.Length; i++)
        {
            hideOnStart[i].SetActive(true);
        }
        playerController = FindObjectOfType<PlayerController>();
        adManager = FindObjectOfType<AdManager>();
        obstacleController = FindObjectOfType<ObstacleController>();
        tunnelController = FindObjectOfType<TunnelController>();
        collectibleController = FindObjectOfType<CollectibleController>();
        highScoreManager = FindObjectOfType<HighScoreManager>();
        TryGetUsername();
        gameSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // playerSpeed = playerStartingSpeed * gameSpeed;
        if (newInvincibilityTime < invincibilityTime)
        {
            newInvincibilityTime += Time.deltaTime;
            if (!invincible)
            {
                invincible = true;
                playerController.Invincible();
            }
        }
        else
        {
            invincible = false;
        }
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

    public void HurtPlayer(int v = 1)
    {
        if (!invincible)
        {
            health -= v;
            newInvincibilityTime = 0;
        }
    }

    public void Die()
    {
        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        isDead = true;
        while (gameSpeed > 0)
        {
            float ft = Time.deltaTime;
            gameSpeed -= ft * 2;
            gameSpeed = Mathf.Clamp(gameSpeed, 0, 2);
            yield return null;
        }
        continueScreen.SetActive(true);
    }

    public void Revive()
    {
        continueScreen.SetActive(false);
        adManager.LoadExtraLifeAd();
        adManager.PlayExtraLifeAd();
        gameSpeed = 0.01f;
    }

    public void SpawnPlayer()
    {
        health = maxHealth;
        lives--;
        newInvincibilityTime = 0;
        invincible = false;
    }

    public void TryGetUsername()
    {
        string name = PlayerPrefs.GetString("Username", GetRandomID());
        UpdateUsername(name);
    }

    public void UpdateUsername(string newname)
    {
        string name = newname;
        if (name == "")
        {
            name = name_input.text;
        }
        name_text.text = name;
        username_UI_text.text = name;
        PlayerPrefs.SetString("Username", name);
    }

    public void StartGame()
    {
        obstacleController.RemoveObstacles();
        tunnelController.RemoveTunnels();
        collectibleController.RemoveCollectibles();
        username = PlayerPrefs.GetString("Username", GetRandomID());
        if (string.IsNullOrEmpty(username))
        {
            username = GetRandomID();
        }
        username_UI_text.text = username;
        for (int i = 0; i < hideOnStart.Length; i++)
        {
            hideOnStart[i].SetActive(false);
        }
        UpdateScore(-score);
        RestorePlayer();
        gameSpeed = 1;
        Time.timeScale = gameSpeed;
    }

    string GetRandomID()
    {
        string id = "";
        for (int i = 0; i < 6; i++)
        {
            id += Random.Range(0, 9).ToString();
        }
        return id;
    }

    public void RestorePlayer()
    {
        playerController.Restore();
        isDead = false;
        gameSpeed = 1;
        Time.timeScale = gameSpeed;
    }

    public void GameOver()
    {
        continueScreen.SetActive(false);
        highScoreScreen.SetActive(true);
        highScoreManager.SubmitScore(username, score);
        for (int i = 0; i < hideOnStart.Length; i++)
        {
            hideOnStart[i].SetActive(true);
        }
    }
}
