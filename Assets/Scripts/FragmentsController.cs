using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentsController : MonoBehaviour
{
    private void OnEnable()
    {
        SetUp();
    }

    public void SetUp()
    {
        foreach (var mcol in GetComponentsInChildren<MeshCollider>())
        {
            if (!mcol.sharedMesh)
            {
                MeshFilter meshFilter = mcol.transform.GetComponent<MeshFilter>();
                if (meshFilter)
                {
                    mcol.sharedMesh = meshFilter.sharedMesh;
                }
            }
        }
    }
}
