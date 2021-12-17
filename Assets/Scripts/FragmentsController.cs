using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentsController : MonoBehaviour
{
    List<Vector3> startPositions = new List<Vector3>();
    List<Quaternion> startRotations = new List<Quaternion>();
    Transform startParent;

    public void SetUp()
    {
        if (!startParent)
        {
            startParent = transform.parent;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform t = transform.GetChild(i);
                startPositions.Add(t.localPosition);
                startRotations.Add(t.localRotation);
            }
        }
    }

    public void ResetFragments()
    {
        transform.SetParent(startParent);
        transform.localPosition = Vector3.zero;
        //Debug.Log("set " + transform.name + " parent to " + startParent.name, startParent.gameObject);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.localPosition = startPositions[i];
            t.localRotation = startRotations[i];
        }
        transform.gameObject.SetActive(false);
    }
}
