using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour {

    [SerializeField]
    private PingPongReferee pingPongReferee;

    private enum TargetScore { p1Points, p2Points, p1Games, p2Games };

    [SerializeField]
    private TargetScore targetScore;


    void OnEnable()
    {
        PingPongReferee.ScoreChanged += UpdateScore;
    }


    void OnDisable()
    {
        PingPongReferee.ScoreChanged -= UpdateScore;
    }


	// Update is called once per frame
	void Update () {
		
	}

    void UpdateScore()
    {
        switch (targetScore)
        {
            case TargetScore.p1Points:
                this.GetComponent<TextMesh>().text = pingPongReferee.Player1Points.ToString();
                break;
            case TargetScore.p2Points:
                this.GetComponent<TextMesh>().text = pingPongReferee.Player2Points.ToString();
                break;
            case TargetScore.p1Games:
                this.GetComponent<TextMesh>().text = pingPongReferee.Player1Games.ToString();
                break;
            case TargetScore.p2Games:
                this.GetComponent<TextMesh>().text = pingPongReferee.Player2Games.ToString();
                break;
        }
    }
}
