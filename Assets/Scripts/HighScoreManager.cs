using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField]
    string privateKey = "D92vssr2FEepHxJG_rq8bw5avZ7Y14-k-1FRzg-NiOMg";
    [SerializeField]
    string publicKey = "61bc80c38f40bb3d78d79906";
    string url = "http://dreamlo.com/lb/";
    bool submittingScore = false;
    bool downloadingScores = false;

    public TextMeshProUGUI text;

    TextMeshProUGUI newHighScoreTexts;

    public Transform holder;
    public Highscore[] highscoresList;

    // Start is called before the first frame update
    void Start()
    {
        DownloadHighscores();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubmitRandomScore()
    {
        StartCoroutine(SubmittingScore("user" + Random.Range(0, 10000), Random.Range(1, 100)));
    }

    public void SubmitScore(string username, int score)
    {
        StartCoroutine(SubmittingScore(username, score));
    }

    IEnumerator SubmittingScore(string username, int score)
    {
        if (!submittingScore)
        {
            submittingScore = true;
            //print("username = " + username);
            WWW www = new WWW(url + privateKey + "/add/" + WWW.EscapeURL(username) + "/" + score);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                //score uploaded successfully
                //print("added score " + username + " : " + score);
            }
            else
            {
                //there was an error uploading the score;
                Debug.LogError("upload score error");
            }
            submittingScore = false;
        }
        yield return null;
        DownloadHighscores();
    }

    public void DownloadHighscores()
    {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        if (!downloadingScores)
        {
            downloadingScores = true;
            WWW www = new WWW(url + publicKey + "/pipe/0/10");
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                //remove old high score UIs
                if (holder.childCount > 0)
                {
                    for (int i = 0; i < holder.childCount; i++)
                    {
                        Destroy(holder.GetChild(i).gameObject);
                    }
                }
                FormatHighscores(www.text);
            }
            else
            {
                print("Error Downloading: " + www.error);
            }
            downloadingScores = false;
        }
        yield return null;
    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);
            newHighScoreTexts = (Instantiate(text, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), holder));



            newHighScoreTexts.name = ("Score " + i.ToString());

            //print(highscoresList[i].username + ": " + highscoresList[i].score);



            //this line will change the ui text 
            newHighScoreTexts.text = ((i + 1).ToString() + " : " + highscoresList[i].username + ": " + highscoresList[i].score);
        }
        //print("got highscores");
    }
    public struct Highscore
    {
        public string username;
        public int score;

        public Highscore(string _username, int _score)
        {
            username = _username;
            score = _score;
        }
    }


    public void ClearHighScores()
    {
        StartCoroutine(ClearingHighScores());
    }

    IEnumerator ClearingHighScores()
    {
        WWW www = new WWW(url + privateKey + "/clear");
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            //cleared scores
        }
        else
        {
            //error while trying to clear scores
        }
        DownloadHighscores();
    }

}
