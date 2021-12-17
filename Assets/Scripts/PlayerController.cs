using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 moveVector;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveVector);
    }
}
