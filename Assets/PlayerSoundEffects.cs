using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour {

    [SerializeField]
    private AudioSource PlayerWonAudioSource;

    [SerializeField]
    private AudioSource PlayerLostAudioSource;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        PingPongReferee.PointToPlayer1Event += PlaySoundPlayerWonPoint;
        PingPongReferee.PointToPlayer2Event += PlaySoundPlayerLostPoint;
    }


    void OnDisable()
    {
        PingPongReferee.PointToPlayer1Event -= PlaySoundPlayerWonPoint;
        PingPongReferee.PointToPlayer2Event -= PlaySoundPlayerLostPoint;
    }

    void PlaySoundPlayerWonPoint()
    {
        this.PlayerWonAudioSource.Play();
    }

    void PlaySoundPlayerLostPoint()
    {
        this.PlayerLostAudioSource.Play();
    }
}
