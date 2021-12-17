using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpAccel = 0.1f;
    public float groundDist = 0.1f;
    public LayerMask groundLayers;
    [SerializeField]
    Rigidbody rb;
    Vector3 moveVector;
    GameManager gameManager;
    bool jumped = false;
    Vector3 startPosition;
    Quaternion startRotation;
    [SerializeField]
    Transform fragmentsParent;
    [SerializeField]
    Transform[] fragmentTs;
    Fragment[] fragments;
    Vector3[] startPositions;
    Quaternion[] startRotations;
    Vector3[] startScales;
    [SerializeField]
    int health;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        gameManager = FindObjectOfType<GameManager>();
        fragments = new Fragment[fragmentTs.Length];
        startPositions = new Vector3[fragmentTs.Length];
        startRotations = new Quaternion[fragmentTs.Length];
        startScales = new Vector3[fragmentTs.Length];
        for (int i = 0; i < fragmentTs.Length; i++)
        {
            startPositions[i] = fragmentTs[i].localPosition;
            startRotations[i] = fragmentTs[i].localRotation;
            startScales[i] = fragmentTs[i].localScale;
            fragments[i] = fragmentTs[i].GetComponent<Fragment>();
        }
        health = gameManager.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isDead)
        {
            return;
        }
        // if we lost some health we should show some cracks
        if (gameManager.health < health)
        {
            float percLost = health - gameManager.health;
            percLost = percLost / gameManager.maxHealth;
            //print("perc lost = " + percLost);
            int piecesToBreak = Mathf.CeilToInt(fragmentTs.Length * percLost) - 1;
            //print("broke off " + piecesToBreak + " pieces");
            while (piecesToBreak > 0)
            {
                int id = 0;
                bool valid = false;
                int tries = 0;
                while (!valid && tries < 99)
                {
                    id = Random.Range(0, fragmentTs.Length);
                    if (!fragments[id].broken)
                    {
                        valid = true;
                        fragments[id].broken = true;
                    }
                    else
                    {
                        tries++;
                    }
                }
                if (tries < 99)
                {
                    fragments[id].Break();
                    fragmentTs[id].SetParent(null);
                    //print("broke off piece " + id);
                }
                piecesToBreak--;
            }
            health = gameManager.health;
            if (health < 1)
            {
                gameManager.Die();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x = gameManager.playerSpeed.x * 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveVector.x = gameManager.playerSpeed.x * -1;
        }
        else
        {
            moveVector.x *= 0.8f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }

    public void Jump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, groundDist * transform.localScale.y, groundLayers))
        {
            moveVector.y = jumpAccel;
            jumped = true;
        }
    }

    private void FixedUpdate()
    {
        //print("moveVector = " + moveVector);
        if (jumped)
        {
            rb.AddForce(Vector3.up * moveVector.y, ForceMode.Impulse);
            moveVector.y = 0;
            jumped = false;
        }
        rb.AddForce(moveVector, ForceMode.Force);
        Vector3 angVelo = rb.angularVelocity;
        angVelo = new Vector3(-gameManager.playerSpeed.z, moveVector.x, moveVector.z);
        rb.angularVelocity = angVelo;
        if (transform.localScale.sqrMagnitude < 30)
        {
            transform.localScale *= 1 + gameManager.sizeIncreaseOverDistance;
        }
    }

    public void Restore()
    {
        StartCoroutine(RestoringPlayer());
    }

    IEnumerator RestoringPlayer()
    {
        transform.localScale = Vector3.one;
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        for (int i = 0; i < fragmentTs.Length; i++)
        {
            fragmentTs[i].SetParent(fragmentsParent);
            if (fragmentTs[i].TryGetComponent<Rigidbody>(out Rigidbody frb))
            {
                Destroy(frb);
                fragments[i].rb = null;
            }
            if (fragmentTs[i].TryGetComponent<MeshCollider>(out MeshCollider mcol))
            {
                Destroy(mcol);
                fragments[i].mcol = null;
            }
        }
        yield return null;
        for (int i = 0; i < fragmentTs.Length; i++)
        {
            fragmentTs[i].transform.localPosition = startPositions[i];
            fragmentTs[i].transform.localRotation = startRotations[i];
            fragmentTs[i].transform.localScale = startScales[i];
            fragments[i].broken = false;
        }
        gameManager.health = gameManager.maxHealth;
        gameManager.lives--;
        health = gameManager.health;
    }

}
