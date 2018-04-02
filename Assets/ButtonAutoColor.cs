using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAutoColor : MonoBehaviour {

    [SerializeField]
    private string opponentTag;
    private PingPongGame pingPongGame;

    private static float brightFactor = 1.3f;
    private static float darkFactor = 0.9f;

    // Use this for initialization
    void Start()
    {

        pingPongGame = GameObject.Find("PingPongGame").GetComponent<PingPongGame>();
        Opponent opp = pingPongGame.opponentContainer.GetOpponent(opponentTag);
        

        ColorBlock cb = this.GetComponent<Button>().colors;
        cb.normalColor = new Color(opp.colorRed / 255f,
            opp.colorGreen / 255f,
            opp.colorBlue / 255f);
        this.GetComponent<Button>().colors = cb;

        cb.highlightedColor = new Color(opp.colorRed / 255f * brightFactor,
            opp.colorGreen / 255f * brightFactor,
            opp.colorBlue / 255f * brightFactor);
        this.GetComponent<Button>().colors = cb;

        cb.pressedColor = new Color(opp.colorRed / 255f * darkFactor,
            opp.colorGreen / 255f * darkFactor,
            opp.colorBlue / 255f * darkFactor);
        this.GetComponent<Button>().colors = cb;


        // change name text to xml defined name
        gameObject.GetComponentInChildren<Text>().text = opp.name;


    }


}
