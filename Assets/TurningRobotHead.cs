using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningRobotHead : MonoBehaviour {

    [SerializeField]
    private GameObject pingPongBall;
    [SerializeField]
    private GameObject joint;
    private Vector3 offset;
    private Quaternion originalOrientation;

    private bool isOpposite;
    private GameObject targetRotation;
    private float turnSpeed = 2.0f;

    // Use this for initialization
	void Start () {
        originalOrientation = this.transform.rotation;
        targetRotation = new GameObject();
        targetRotation.transform.position = this.transform.position;
        targetRotation.transform.rotation = originalOrientation;
        offset = this.transform.position - joint.transform.position;
        if (this.transform.position.z > 0.0f) isOpposite = false;
        else isOpposite = true;
	}
	
	// Update is called once per frame
	void Update () {
        targetRotation.transform.LookAt(pingPongBall.transform.position);
      
        if (isOpposite) targetRotation.transform.Rotate(new Vector3(-270.0f, 0.0f, 0.0f));
        else targetRotation.transform.Rotate(new Vector3( -90.0f, 0.0f, 0.0f));

        //this.transform.position = joint.transform.position + ((this.transform.rotation * originalOrientation) * offset);
        //targetRotation.transform.position = this.transform.position;
        targetRotation.transform.position = joint.transform.position + ((targetRotation.transform.rotation * originalOrientation) * offset);
        //this.transform.position = targetRotation.transform.position;
        this.transform.position = Vector3.Lerp(this.transform.position, targetRotation.transform.position, Time.deltaTime * turnSpeed);

        if (isOpposite) targetRotation.transform.Rotate(new Vector3(180.0f, 0.0f, 0.0f));

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation.transform.rotation, Time.deltaTime * turnSpeed);
        
    }
}
