using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroboLight : MonoBehaviour {

    [SerializeField] private float frequency; //blinks per sec
    private float stillTime;
    bool stroboRunning = false;

	// Use this for initialization
	void Start () {
        //stillTime = 1.0f / frequency * 0.5f;
        //StartCoroutine(StroboChanger(stillTime));
    }

    void OnEnable()
    {
        stillTime = 1.0f / frequency * 0.5f;
        StartCoroutine(StroboChanger(stillTime));
    }
	
	// Update is called once per frame
	void Update () {
        stillTime = 1.0f / frequency * 0.5f;
        //if (!stroboRunning) StartCoroutine(StroboChanger(stillTime));
    }

    

    IEnumerator StroboChanger(float waitSecs)
    {
        //stroboRunning = true;
        yield return new WaitForSeconds(waitSecs);
        GetComponent<Light>().enabled = !(GetComponent<Light>().isActiveAndEnabled); //reverse light state
        //stroboRunning = false;
        StartCoroutine(StroboChanger(stillTime));
    }

}
