using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{


    [SerializeField]
    private GameObject pingPongBall;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pingPongBall != null) transform.LookAt(pingPongBall.transform);
    }
}

