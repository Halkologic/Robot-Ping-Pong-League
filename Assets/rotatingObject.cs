using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingObject : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
    }
}
