using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    int damage = 1;

    Animator anim;
    CapsuleCollider col;
    GameManager gameManager;
    [SerializeField]
    Rigidbody[] ragdollRBs;
    bool hitRecently = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetNPC()
    {
        anim.enabled = true;
        col.isTrigger = false;
        hitRecently = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hit(collision.GetContact(0).point);
    }

    public void Hit(Vector3 pos)
    {
        StartCoroutine(HitByPlayer(pos));
    }

    IEnumerator HitByPlayer(Vector3 pos)
    {
        if (hitRecently)
        {

        }
        else
        {
            hitRecently = true;
            anim.enabled = false;
            col.isTrigger = true;
            gameManager.health -= damage;
            yield return null;
            for (int i = 0; i < ragdollRBs.Length; i++)
            {
                ragdollRBs[i].AddForce(Vector3.forward * -gameManager.playerSpeed.z * 1, ForceMode.Impulse);
                ragdollRBs[i].AddForce(Vector3.up * gameManager.playerSpeed.z * 2, ForceMode.Impulse);
            }
            this.enabled = false;
            hitRecently = false;
        }
        yield return null;
    }
}
