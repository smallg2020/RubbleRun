using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    Vector3 maxShake;
    [SerializeField]
    float maxDuration;

    public void ShakeCamera(float intensity = 1)
    {
        StartCoroutine(ShakingCamera(intensity));
    }

    IEnumerator ShakingCamera(float intensity = 1)
    {
        Vector3 startPos = transform.position;
        float t = maxDuration * intensity;
        Vector3 targetPos = startPos;
        while (t > 0)
        {
            float ft = Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, ft * 10);
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                targetPos = startPos + (maxShake * Random.Range(0f, 1.0f));
            }
            t -= ft;
            yield return null;
        }
        while (Vector3.Distance(transform.position, startPos) > 0.1f)
        {
            float ft = Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, startPos, ft);
            yield return null;
        }
        transform.position = startPos;
    }

}
