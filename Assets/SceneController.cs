using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    [SerializeField]
    PingPongReferee referee;
    [SerializeField]
    private float normalTimeScale;
    [SerializeField]
    private float slowmotionTimeScale;

    // Use this for initialization
    void Start () {
        referee.StartMatch();
        setNormalTimeScale();
    }

	// Update is called once per frame
	void Update () {

	}



    public void setNormalTimeScale()
    {
        Time.timeScale = normalTimeScale;
    }


    public void setSlowMotionTimeScale()
    {
        Time.timeScale = slowmotionTimeScale;
    }
}
