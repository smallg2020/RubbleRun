using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmazingAssets.CurvedWorld;

public class BendController : MonoBehaviour
{
    [SerializeField]
    float bendSpeedX = 1, bendSpeedY = 1;
    public float bendTargetX = 0, bendTargetY = 0;
    [SerializeField]
    float minBendDurationX, maxBendDurationX, minBendDurationY, maxBendDurationY;
    [SerializeField]
    float minBendX, maxBendX, minBendY, maxBendY;
    [SerializeField]
    float minCurve, maxCurve, curveSpeed;

    float bendDurationX, bendDurationY;
    float bendX = 0, bendY = 0;
    float newBendDurationX, newBendDurationY;
    float currentCurve = 0, curveTarget = 0;
    CurvedWorldController curvedWorld;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        curvedWorld = FindObjectOfType<CurvedWorldController>();
        gameManager = FindObjectOfType<GameManager>();
        bendDurationX = maxBendDurationX;
        bendDurationY = maxBendDurationY;
    }

    // Update is called once per frame
    void Update()
    {
        float ft = Time.deltaTime;
        newBendDurationX += ft * gameManager.playerSpeed.z;
        newBendDurationY += ft * gameManager.playerSpeed.z;
        if (newBendDurationX > bendDurationX)
        {
            newBendDurationX = 0;
            bendDurationX = Random.Range(minBendDurationX, maxBendDurationX);
            bendTargetX = Random.Range(minBendX, maxBendX);
            if (Random.Range(0, 1) == 0)
            {
                float perc = Mathf.Abs(bendTargetX) / maxBendX;
                curveTarget = maxCurve * perc;
                if (bendTargetX < 0)
                {
                    curveTarget *= -1;
                }
            }
            else
            {
                curveTarget = 0;
            }
        }
        if (newBendDurationY > bendDurationY)
        {
            newBendDurationY = 0;
            bendDurationY = Random.Range(minBendDurationY, maxBendDurationY);
            bendTargetY = Random.Range(minBendX, maxBendY);
        }
        bendX = Mathf.Lerp(bendX, bendTargetX, bendSpeedX * ft);
        bendY = Mathf.Lerp(bendY, bendTargetY, bendSpeedY * ft);
        currentCurve = Mathf.Lerp(currentCurve, curveTarget, curveSpeed * ft);
        curvedWorld.bendHorizontalSize = bendX;
        curvedWorld.bendVerticalSize = bendY;
        curvedWorld.bendCurvatureSize = currentCurve;
    }
}
