using UnityEngine;
using System.Collections;

public class PingPongBall : MonoBehaviour
{

    [SerializeField]
    private Vector3 hitPower;
    [SerializeField]
    private Vector3 hitPower2;

    [SerializeField]
    private AudioSource hitPad;
    [SerializeField]
    private AudioSource hitTable;
    private bool served = false;
    private Vector3 defaultPosition;
    [SerializeField]
    private bool autoServe;
    private float autoServeWait = 3.0f;

    public delegate void TableBouncePlayer1SideAction();
    public static event TableBouncePlayer1SideAction TableBouncePlayer1Side;

    public delegate void TableBouncePlayer2SideAction();
    public static event TableBouncePlayer2SideAction TableBouncePlayer2Side;

    public delegate void FloorBounceAction();
    public static event FloorBounceAction FloorBounce;

    public delegate void Player1HitAction();
    public static event Player1HitAction Player1Hit;

    public delegate void Player2HitAction();
    public static event Player2HitAction Player2Hit;

    void Awake()
    {
        defaultPosition = this.transform.position;
    }

    // Use this for initialization
    void Start()
    {
        
        Physics.bounceThreshold = 0.6f;
        // this.GetComponent<Rigidbody>().AddForce(hitPower, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().isKinematic = true;
    }


    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {


        if (served == false) autoServeWait = autoServeWait - Time.fixedDeltaTime;

        if ((Input.GetKeyUp(KeyCode.S) || GvrController.ClickButtonUp || (autoServe && autoServeWait < 0.0f)) && served == false)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            //player2 side
            if (this.transform.position.z > 0.0f) HitBall(hitPower);
            //player1 side
            else HitBall(hitPower2);

            served = true;
        }
        /**
        if (Input.GetKeyUp(KeyCode.R) || GvrController.AppButtonUp) 
        {
            //resetBall();
        }
        **/
    }

    public void playPadSound()
    {
        hitPad.Play();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Pad")
        {
            hitPad.Play();
            this.GetComponent<Rigidbody>().isKinematic = false;
            served = true;

        }
        else if (col.gameObject.name == "Table")
        {
            if (this.transform.position.z < 0.0f)
            {
                if (TableBouncePlayer1Side != null) TableBouncePlayer1Side();
            }
            else
            {
                if (TableBouncePlayer2Side != null) TableBouncePlayer2Side();
            }
            hitTable.Play();
        }
        
           
        else if (col.gameObject.name == "Floor")
        {
            if (FloorBounce != null) FloorBounce();
            //FloorBounce();
        }
        //else hitTable.Play();
    }

    public void HitBall(Vector3 force)
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        served = true;
        this.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        this.playPadSound();
        if (this.transform.position.z < 0.0f) Player1Hit();
        else Player2Hit();
    }

    public void resetBallToPlayer2()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        served = false;
        autoServeWait = 3.0f;
        this.transform.position = defaultPosition;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void resetBallToPlayer1()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        served = false;
        autoServeWait = 3.0f;
        this.transform.position = new Vector3(-defaultPosition.x, defaultPosition.y, -defaultPosition.z);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }



}
