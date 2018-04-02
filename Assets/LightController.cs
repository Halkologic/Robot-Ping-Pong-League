using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    [SerializeField] private GameObject[] spots;
    [SerializeField] private GameObject[] globalLight;
    [SerializeField] private GameObject[] lightSabres;
    [SerializeField] private GameObject[] strobos;
    [SerializeField]
    private GameObject[] spinningBlues;
    [SerializeField]
    private GameObject[] spinningReds;



    private enum LightMode { normal, strobo, blue, red };
    [SerializeField] private LightMode lightMode = LightMode.normal;


    void OnEnable()
    {
        PingPongReferee.PointToPlayer1Event += PlayerWonPointLights;
        PingPongReferee.PointToPlayer2Event += PlayerLostPointLights;
    }


    void OnDisable()
    {
        PingPongReferee.PointToPlayer1Event -= PlayerWonPointLights;
        PingPongReferee.PointToPlayer2Event -= PlayerLostPointLights;
    }

    private void PlayerWonPointLights()
    {
        StartCoroutine(ModeForXSecs(LightMode.blue, 1.5f));
    }

    private void PlayerLostPointLights()
    {
        StartCoroutine(ModeForXSecs(LightMode.red, 1.5f));
    }

    IEnumerator StroboForXSecs(float lenght)
    {
        lightMode = LightMode.strobo;
        yield return new WaitForSeconds(lenght);
        lightMode = LightMode.normal;
    }

    IEnumerator ModeForXSecs(LightMode LM, float lenght)
    {
        lightMode = LM;
        yield return new WaitForSeconds(lenght);
        lightMode = LightMode.normal;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (lightMode)
        {
            case LightMode.normal:
                LightGroupOn(spots, true);
                LightGroupOn(globalLight, true);
                LightGroupOn(lightSabres, true);
                LightGroupOn(strobos, false);
                LightGroupOn(spinningBlues, false);
                LightGroupOn(spinningReds, false);
                break;
            case LightMode.strobo:
                LightGroupOn(spots, false);
                LightGroupOn(globalLight, false);
                LightGroupOn(lightSabres, false);
                LightGroupOn(strobos, true);
                LightGroupOn(spinningBlues, false);
                LightGroupOn(spinningReds, false);
                break;
            case LightMode.blue:
                LightGroupOn(spots, false);
                LightGroupOn(globalLight, true);
                LightGroupOn(lightSabres, false);
                LightGroupOn(strobos, false);
                LightGroupOn(spinningBlues, true);
                LightGroupOn(spinningReds, false);
                break;
            case LightMode.red:
                LightGroupOn(spots, false);
                LightGroupOn(globalLight, true);
                LightGroupOn(lightSabres, false);
                LightGroupOn(strobos, false);
                LightGroupOn(spinningBlues, false);
                LightGroupOn(spinningReds, true);
                break;
        }

            
    }

    private void LightGroupOn(GameObject[] lightGroup, bool state)
    {
        foreach (GameObject gm in lightGroup)
        {
            gm.SetActive(state);
        }
    }

}
