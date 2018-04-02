using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TvFunctions : MonoBehaviour {

    [SerializeField]
    private ScrollRect tvScrollRect;

    private int targetPage;
    [SerializeField]
    private int maxPage;


    private PingPongGame pingPongGame;

	// Use this for initialization
	void Start () {
        targetPage = 0;
        pingPongGame = GameObject.Find("PingPongGame").GetComponent<PingPongGame>();
        
    }
	
	// Update is called once per frame
	void Update () {

        tvScrollRect.normalizedPosition = Vector2.Lerp(tvScrollRect.normalizedPosition, new Vector2(((float)targetPage / (float)maxPage), 0.0f), 1 - Mathf.Exp(-20 * Time.deltaTime));
    }

    public void LoadTestMatch()
    {

        Application.LoadLevel("pingpongscene");
    }

    public void StartMatch(string opponentTag)
    {
        pingPongGame.StartMatch(opponentTag);
    }



    public void ToNextPage()
    {
        targetPage++;
        if (targetPage > maxPage) targetPage = maxPage;
    }

    public void ToPreviousPage()
    {
        targetPage--;
        if (targetPage < 0) targetPage = 0;
    }


}
