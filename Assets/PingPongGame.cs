using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PingPongGame : MonoBehaviour {

    //Master class for whole game state 

    public const string path = "Opponents.xml";
    public TextAsset ta;
    private Opponent currentOpponent;

    public OpponentContainer opponentContainer;

    void OnEnable()
    {
        PingPongReferee.MatchToPlayer1Event += MatchWon;
        PingPongReferee.MatchToPlayer2Event += MatchLost;
    }


    void OnDisable()
    {
        PingPongReferee.MatchToPlayer1Event -= MatchWon;
        PingPongReferee.MatchToPlayer2Event -= MatchLost;
    }




    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        opponentContainer = OpponentContainer.Load(ta);
        LoadPrefabs();
        //Application.LoadLevel("lockerroomscene");
        SceneManager.LoadScene("lockerroomscene");

    }

    private void LoadPrefabs()
    {
        foreach (Opponent opponent in opponentContainer.opponents)
        {
            opponent.SetBeaten(
                PlayerPrefs.GetInt(opponent.tag, 0) == 1
                );
        }
    }

    /// <summary>
    /// Saves game state
    /// - tutorial completed (TODO!)
    /// - beaten opponents
    /// </summary>
    private void SavePrefabs()
    {
        foreach (Opponent opponent in opponentContainer.opponents)
        {
            PlayerPrefs.SetInt(opponent.tag, (opponent.beaten ? 1 : 0));
        }
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartMatch(string opponentTag)
    {
        currentOpponent = opponentContainer.GetOpponent(opponentTag);
        //if (currentOpponent != null) Application.("pingpongscene");
        if (currentOpponent != null) SceneManager.LoadScene("pingpongscene");
        else Debug.Log("Opponent with requested Tag '" + opponentTag + "' not found.");
    }

    public Opponent GetCurrentOpponent()
    {
        return currentOpponent;
    }

    public void MatchWon()
    {
        OpponentBeaten(currentOpponent.tag);
        //Application.LoadLevel("lockerroomscene");
        SceneManager.LoadScene("lockerroomscene");

    }

    public void MatchLost()
    {
        //Application.LoadLevel("lockerroomscene");
        SceneManager.LoadScene("lockerroomscene");
    }

    public void OpponentBeaten(string opponentTag)
    {
        opponentContainer.GetOpponent(opponentTag).SetBeaten(true);
        SavePrefabs();
    }




}
