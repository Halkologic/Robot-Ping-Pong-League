using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

    [SerializeField] private float DegPerSec;

	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0, DegPerSec * Time.deltaTime, 0);
    }

}
