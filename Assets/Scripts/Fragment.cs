using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    [SerializeField]
    float explosionMulti = 4;
    GameManager gameManager;
    public bool broken = false;
    public Rigidbody rb;
    public MeshCollider mcol;
    bool brokeOff = false;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (broken)
        {
            transform.position += Vector3.forward * gameManager.playerSpeed.z * Time.deltaTime;
        }
    }

    public void Break()
    {
        if (!mcol)
        {
            mcol = gameObject.AddComponent<MeshCollider>();
            mcol.convex = true;
            rb = gameObject.AddComponent<Rigidbody>();
            rb.drag = 1;
            rb.angularDrag = 1;
            brokeOff = false;
        }
    }

    private void FixedUpdate()
    {
        if (broken)
        {
            if (!brokeOff)
            {
                rb.AddForce((transform.position - playerController.transform.position).normalized * explosionMulti, ForceMode.Impulse);
                rb.AddForce(-Vector3.forward * gameManager.playerSpeed.z * explosionMulti * 0.7f, ForceMode.Impulse);
                rb.AddForce(Vector3.up * gameManager.playerSpeed.z * 0.3f * explosionMulti, ForceMode.Impulse);
                brokeOff = true;
            }
        }
    }
}
