using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class AdManager : MonoBehaviour, IUnityAdsShowListener
{
    string gameID = "4517641";
    string extraLifeAdID = "ExtraLifeRV";
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameID);
        gameManager = FindObjectOfType<GameManager>();
    }

    public void LoadExtraLifeAd()
    {
        Advertisement.Load(extraLifeAdID);
    }

    public void PlayExtraLifeAd()
    {
        Advertisement.Show(extraLifeAdID, this);
        //print("showing ad");
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //print("add completed");
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            gameManager.RestorePlayer();
            //print("restored player");
        }
        else
        {
            gameManager.GameOver();
            //print("game over");
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //print("ad failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //print("starting ad");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        //print("add clicked");
    }

}
