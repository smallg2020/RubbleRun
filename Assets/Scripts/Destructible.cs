using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    int damage = 1;

    Transform oldParent;
    GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        oldParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            gameManager.health -= damage;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform t = transform.GetChild(i);
                t.gameObject.SetActive(true);
                if (t.TryGetComponent<FragmentsController>(out FragmentsController fragmentsController))
                {
                    fragmentsController.SetUp();
                }
                transform.GetChild(i).SetParent(transform.parent);
            }
            transform.SetParent(oldParent);
            transform.gameObject.SetActive(false);
        }
    }
}
