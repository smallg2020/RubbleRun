using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float speed = 2;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float ft = Time.deltaTime;
        Vector3 npos = transform.position;
        npos.x = playerController.transform.position.x;
        transform.position = Vector3.Lerp(transform.position, npos, ft * speed);
    }
}
