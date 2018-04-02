using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueButtonForWonTest : MonoBehaviour {

    [SerializeField] private string referenceTag;
    private PingPongGame pingPongGame;

    void Awake()
    {

        GameObject go1 = GameObject.Find("PingPongGame");
        pingPongGame = (PingPongGame)go1.GetComponent(typeof(PingPongGame));

        bool beaten = pingPongGame.opponentContainer.GetOpponent(referenceTag).beaten;
        if (beaten) this.GetComponent<Image>().color = new Color(0.0f, 0.0f, 1.0f);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
