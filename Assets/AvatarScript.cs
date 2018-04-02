using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarScript : MonoBehaviour {

    [SerializeField]
    private GameObject pingPingBall;

    [SerializeField]
    private bool isOpposite;

    [SerializeField]
    private PadScript padScript;

    private Vector3 originalPosition;

    private Vector3 targetPosition;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Material transParentAvatarMaterial;

    private float armXReachNegative;

    private enum PlayingHand {leftHand, rightHand};
    [SerializeField]
    private PlayingHand playingHand;
    private enum HandSide {left, right};
    private HandSide handSide;

    private float randomness;

    private PingPongGame pingPongGame;

    // Use this for initialization
    void Start () {

        //
        if (isOpposite)
        {
            if (GvrSettings.Handedness.Equals(GvrSettings.UserPrefsHandedness.Left)) { playingHand = PlayingHand.leftHand; }
            else { playingHand = PlayingHand.rightHand; }
        }
            

        pingPongGame = GameObject.Find("PingPongGame").GetComponent<PingPongGame>();
        if (!isOpposite) InitializeOpponent();
        randomness = Random.value;
        originalPosition = this.transform.position;
        targetPosition = originalPosition;
        IntermediateTargetPoisition = targetPosition;
        //GetComponent<Renderer>().material.color.a = 0;

        if (isOpposite) setAvatarTransparent();
    }

    private void InitializeOpponent()
    {
        speed = pingPongGame.GetCurrentOpponent().speed;
        armReach = pingPongGame.GetCurrentOpponent().reach;

        setAvatarColor();

        /**GetComponent<Renderer>().material.SetColor("_Color",
            new Color(pingPongGame.GetCurrentOpponent().colorRed / 255f,
            pingPongGame.GetCurrentOpponent().colorGreen / 255f,
            pingPongGame.GetCurrentOpponent().colorBlue / 255f));
        **/
    }


    private void setAvatarTransparent()
    {
        setObjectTranparent(this.gameObject);
        //print("entered trasparency method");
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        //foreach (Transform child in transform)
        {
            //print(child.name);
            setObjectTranparent(child.gameObject);
        }
    }

    private void setAvatarColor()
    {
        //setObjectTranparent(this.gameObject);
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_Color", Color.red);
        print("entered set color method method");
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        //foreach (Transform child in transform)
        {
            print(child.name);
            setObjectColor(child.gameObject);
        }
    }

    private void setObjectTranparent(GameObject gameObject)
    {
        if (gameObject.GetComponent<MeshRenderer>() != null)
            gameObject.GetComponent<MeshRenderer>().material = transParentAvatarMaterial;

    }

    private void setObjectColor(GameObject gameObject)
    {
        
        if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            /**
            GetComponent<Renderer>().material.SetColor("_Color",
            new Color(pingPongGame.GetCurrentOpponent().colorRed / 255f,
            pingPongGame.GetCurrentOpponent().colorGreen / 255f,
            pingPongGame.GetCurrentOpponent().colorBlue / 255f));
            **/
            Renderer rend = gameObject.GetComponent<Renderer>();
            rend.material.shader = Shader.Find("Standard");
            rend.material.SetColor("_Color", new Color(pingPongGame.GetCurrentOpponent().colorRed / 255f,
            pingPongGame.GetCurrentOpponent().colorGreen / 255f,
            pingPongGame.GetCurrentOpponent().colorBlue / 255f));
        }
            
        
        //gameObject.GetComponent<Renderer>().material.color = Color.green;

    }

    public bool frontHand()
    {
        return ((playingHand == PlayingHand.leftHand && handSide == HandSide.left)
            || (playingHand == PlayingHand.rightHand && handSide == HandSide.right));
    }

    public float padXCorrection(float currentX)
    {
        bool xDiffIsPositive = currentX >= this.transform.position.x;
        float xDiff = currentX - this.transform.position.x;
        bool currentXIsWithinArmReach = (armReach - Mathf.Abs(xDiff) >= 0.0f);

        if (handSide == HandSide.left)
        {
            if (isOpposite && xDiffIsPositive) return this.transform.position.x;
            else if (isOpposite && !xDiffIsPositive)
            {
                if (currentXIsWithinArmReach) return currentX;
                else return this.transform.position.x - armReach;
            }    
            else if (!isOpposite && xDiffIsPositive)
            {
                if (currentXIsWithinArmReach) return currentX;
                else return this.transform.position.x + armReach;
            }
            else return this.transform.position.x;

        }
        else
        {
            if (isOpposite && xDiffIsPositive) 
            {
                if (currentXIsWithinArmReach) return currentX;
                else return this.transform.position.x + armReach;
            } 
            else if (isOpposite && !xDiffIsPositive) return this.transform.position.x;
            else if (!isOpposite && xDiffIsPositive) return this.transform.position.x;
            else 
            {
                if (currentXIsWithinArmReach) return currentX;
                else return this.transform.position.x - armReach;
            }
        }

    }
    [SerializeField]
    private float armReach = 1.5f;
    public bool ballXWithinArmReach()
    {
        

        float xDiff = pingPingBall.transform.position.x - this.transform.position.x;
        bool armReaches = (armReach - Mathf.Abs(xDiff) >= 0.0f);
        if (!armReaches) return false;
        if (handSide == HandSide.left)
        {
            if (isOpposite) return xDiff <= 0.0f;
            else return xDiff >= 0.0f;
        } else
        {
            if (this.transform.position.z < 0.0f) return xDiff >= 0.0f;
            else return xDiff <= 0.0f;
        }
    }

    public Vector3 padRestingPosition()
    {

        float xOffset = 0.8f;
        float yOffset = 0.5f;
        float zOffset = 1.1f;
        

        if (isOpposite)
        {
            if (handSide == HandSide.left) return new Vector3(this.transform.position.x - xOffset, this.transform.position.y + yOffset, this.transform.position.z + zOffset);
            else return new Vector3(this.transform.position.x + xOffset, this.transform.position.y + yOffset, this.transform.position.z + zOffset);
        } else
        {
            if (handSide == HandSide.left) return new Vector3(this.transform.position.x + xOffset, this.transform.position.y + yOffset, this.transform.position.z - zOffset);
            else return new Vector3(this.transform.position.x - xOffset, this.transform.position.y + yOffset, this.transform.position.z - zOffset);

        }

        
    }

    void FixedUpdate() {
        executeAvatarMovement();
    }

    //This method controls the avatars movement
    private void executeAvatarMovement()
    {

        Vector3 temporaryTargetVector;
        //what to do when ball is approaching?
        if (ballApproaches())
        {
            temporaryTargetVector = calculateBallArrivalPosition();
            
            //add small offset
            float offsetX = 0.5f;
            this.targetPosition = targetPositionOffsetCorrector(temporaryTargetVector, offsetX);

        }
        //if the ball is stationary, there is no way to calculate arrival position. Therefore, let's just use the ball's actual x-coordinate.
        else if (ballStationary())
        {   
            temporaryTargetVector = new Vector3(pingPingBall.transform.position.x,
                this.transform.position.y,
                this.transform.position.z);
            //add small offset
            float offsetX = 0.5f;
            this.targetPosition = targetPositionOffsetCorrector(temporaryTargetVector, offsetX);
        }
        //what to do when ball is moving away? -> (move back to center, if avatar is far from it)
        else
        {
            float maxDistanceFromCenter = 0.7f;
            if (Mathf.Abs(this.transform.position.x) <= 0.7f) this.targetPosition = this.transform.position;
            else
            {
                if (this.transform.position.x > 0.7f) this.targetPosition = new Vector3(0.7f, this.transform.position.y, this.transform.position.z);
                else this.targetPosition = new Vector3(-0.7f, this.transform.position.y, this.transform.position.z);
            }
        }
        AvatarZMovement();
    }

    private void AvatarZMovement()
    {
        float ballOffsetZ = 0.5f;
        if (isOpposite)
        {
            if (pingPingBall.transform.position.z - ballOffsetZ < originalPosition.z) this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, pingPingBall.transform.position.z - ballOffsetZ);
            else this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, originalPosition.z);
        }
        else
        {
            if (pingPingBall.transform.position.z + ballOffsetZ > originalPosition.z) this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, pingPingBall.transform.position.z + ballOffsetZ);
            else this.targetPosition = new Vector3(targetPosition.x, targetPosition.y, originalPosition.z);
        }

    }


    //A helper method for calcualting the avatars target position
    private Vector3 targetPositionOffsetCorrector(Vector3 temporaryTargetVector, float offsetX)
    {
        Vector3 targetPositionWithOffset;

        if (isOpposite)
        {
            if (handSide == HandSide.left) targetPositionWithOffset = new Vector3(temporaryTargetVector.x + offsetX, temporaryTargetVector.y, temporaryTargetVector.z);
            else targetPositionWithOffset = new Vector3(temporaryTargetVector.x - offsetX, temporaryTargetVector.y, temporaryTargetVector.z);
        }
        else
        {
            if (handSide == HandSide.left) targetPositionWithOffset = new Vector3(temporaryTargetVector.x - offsetX, temporaryTargetVector.y, temporaryTargetVector.z);
            else targetPositionWithOffset = new Vector3(temporaryTargetVector.x + offsetX, temporaryTargetVector.y, temporaryTargetVector.z);

        }

        return targetPositionWithOffset;
    }

    public Vector3 calculateBallArrivalPosition()
    {

        float z1 = pingPingBall.transform.position.z + this.pingPingBall.GetComponent<Rigidbody>().velocity.z;
        float x1 = pingPingBall.transform.position.x + this.pingPingBall.GetComponent<Rigidbody>().velocity.x;
        float z2 = this.originalPosition.z;
        float x2 = z2 * x1 / z1;

        return new Vector3(x2, originalPosition.y, originalPosition.z);
        
    }

    private bool ballApproaches()
    {
        if (isOpposite) return (pingPingBall.GetComponent<Rigidbody>().velocity.z < 0.0f);
        else return (pingPingBall.GetComponent<Rigidbody>().velocity.z > 0.0f);

    }

    private bool ballStationary()
    {
        return pingPingBall.GetComponent<Rigidbody>().velocity.z == 0.0f;
    }

    public delegate void HandSideChangedAction();
    public event HandSideChangedAction HandSideChanged;

    private void UpdateHandSide()
    {

        if (padScript.HandSideShouldBeRight())
        {
            if (handSide == HandSide.left && HandSideChanged != null)HandSideChanged();
            handSide = HandSide.right;
        } 
        else
        {
            if (handSide == HandSide.right && HandSideChanged != null) HandSideChanged();
            handSide = HandSide.left;
        }
            
    }

    private Vector3 IntermediateTargetPoisition;


	// Update is called once per frame
	void Update () {
        UpdateHandSide();
        float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        IntermediateTargetPoisition = Vector3.MoveTowards(IntermediateTargetPoisition, targetPosition, step);
        this.transform.position = Vector3.Lerp(this.transform.position, IntermediateTargetPoisition, 1 - Mathf.Exp(-20 * Time.deltaTime));

        //this movement style did not work fast enough
        //this.transform.position = Vector3.Lerp(this.transform.position, IntermediateTargetPoisition, 2.0f * Time.deltaTime);

        this.transform.position = new Vector3(this.transform.position.x, originalPosition.y + (0.05f) * Mathf.Sin(Time.time * (3.0f + randomness / 100.0f)), this.transform.position.z);
    }
}
