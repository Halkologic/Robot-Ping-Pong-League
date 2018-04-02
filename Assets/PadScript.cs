using UnityEngine;
using System.Collections;

public class PadScript : MonoBehaviour {


    
    [SerializeField]
    private Material transParentPadMaterial;
    [SerializeField]
    private GameObject pingPongBall;
    [SerializeField]
    private GameObject pingPongBallShine;
    [SerializeField]
    private AvatarScript avatarScript;
    [SerializeField]
    private GameObject avatarObject;

    [SerializeField]
    private PingPongReferee pingPongReferee;

    private Vector3 zeroVector3 = new Vector3(0.0f, 0.0f, 0.0f);

    private bool sideIsOpposite;

    private Vector3 defaultPosition;

    [SerializeField]
    private GameObject hitTarget;

    [SerializeField]
    private SceneController sceneController;

    private enum Intelligence
    {
        manualMouse,
        simpleGesture,
        advancedGesture1,
        simpleSuperAI,
        intelligentAI_One
        
    };

    [SerializeField]
    private Intelligence intelligence;

    
    private float simpleGestureCoolDown = 0.0f;


    void OnEnable()
    {
        avatarScript.HandSideChanged += HandSideJustChanged;
    }


    void OnDisable()
    {
        avatarScript.HandSideChanged -= HandSideJustChanged;
    }



    private float handSideChangeTimer = 0.0f;
    private void HandSideJustChanged()
    {
        handSideChangeTimer = 1.5f;
    }

    private PingPongGame pingPongGame;

    // Use this for initialization
    void Start () {
        sideIsOpposite = this.transform.position.z < 0.0f;

        pingPongGame = GameObject.Find("PingPongGame").GetComponent<PingPongGame>();
        if (!sideIsOpposite) InitializeOpponent();

        defaultPosition = this.transform.position;
        if (this.sideIsOpposite) this.GetComponent<MeshRenderer>().material = transParentPadMaterial;
    }

    //Initialize current opponent values to pad.
    private void InitializeOpponent()
    {
        highBallChance = pingPongGame.GetCurrentOpponent().highHitChance;
        middleBallChance = pingPongGame.GetCurrentOpponent().midHitChance;
        lowBallChance = pingPongGame.GetCurrentOpponent().lowHitChance;

        xUnaccuracy = pingPongGame.GetCurrentOpponent().xUnaccuracy;
        zUnaccuracy = pingPongGame.GetCurrentOpponent().zUnaccuracy;
        forceUnaccuracy = pingPongGame.GetCurrentOpponent().forceUnaccuracy;
    }

    void FixedUpdate()
    {
        if (simpleGestureCoolDown > 0.0f) simpleGestureCoolDown = simpleGestureCoolDown - Time.fixedDeltaTime;
    }

    private void printAccelometerData()
    {
        //Quaternion q = GvrController.Orientation;
        Quaternion q = this.transform.rotation;
        Vector3 gravity = new Vector3(0, 9.81f, 0);
        Vector3 orientationCorrectedAccel = q * GvrController.Accel;
        Vector3 oriAndGravCorrAccel = orientationCorrectedAccel - gravity;
        print(oriAndGravCorrAccel.sqrMagnitude + " " +
            oriAndGravCorrAccel.magnitude + " " +
            oriAndGravCorrAccel.x + " " +
            oriAndGravCorrAccel.y + " " +
            oriAndGravCorrAccel.z);
    }

	// Update is called once per frame
	void Update ()
    {


        switch (intelligence)
        { 
            case Intelligence.advancedGesture1:
                ExecuteAdvancedGeture1();
                break;
            case Intelligence.manualMouse:
                ExecuteManualMouse();
                break;
            case Intelligence.simpleGesture:
                ExecuteSimpleGesture();
                break;
            case Intelligence.simpleSuperAI:
                ExecuteSimpleSuperAI();
                break;
            case Intelligence.intelligentAI_One:
                ExecuteIntelligentAiOne();
                break;

        }

        if (handSideChangeTimer > 0.0f) handSideChangeTimer = handSideChangeTimer - Time.deltaTime;
    }

    private bool BallApproaches()
    {
        if (sideIsOpposite) return (pingPongBall.GetComponent<Rigidbody>().velocity.z < 0.0f);
        else return (pingPongBall.GetComponent<Rigidbody>().velocity.z > 0.0f);

    }

    private bool IntelligentHandSideDecision()
    {
        Vector3 temporaryTargetVector;
        //what to do when ball is approaching?
        if (BallApproaches())
        {
            temporaryTargetVector = avatarScript.calculateBallArrivalPosition();


            if (sideIsOpposite)
            {
                if (temporaryTargetVector.x >= avatarObject.transform.position.x)
                {
                    return true;
                }
                else return false;
            } else
            {
                if (temporaryTargetVector.x >= avatarObject.transform.position.x)
                {
                    return false;
                }
                else return true;
            }
                

        }
        //if the ball is stationary, there is no way to calculate arrival position. Therefore, let's just use the ball's actual x-coordinate.
        else if (pingPongBall.GetComponent<Rigidbody>().velocity.z == 0.0f)
        {
            temporaryTargetVector = new Vector3(pingPongBall.transform.position.x,
                this.transform.position.y,
                this.transform.position.z);

            if (sideIsOpposite)
            {
                if (temporaryTargetVector.x >= avatarObject.transform.position.x)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                if (temporaryTargetVector.x >= avatarObject.transform.position.x)
                {
                    return false;
                }
                else return true;
            }

        }
        //what to do when ball is moving away? -> (move back to center, if avatar is far from it)
        else
        {
            return true;
        }
    }

    public bool HandSideShouldBeRight()
    {
        switch (intelligence)
        {
            case Intelligence.advancedGesture1:
                //return (GvrController.Orientation.ToEulerAngles().y > 0.0f);
                return (this.transform.rotation.ToEulerAngles().y > 0.0f);
            case Intelligence.manualMouse:
                //does not imply
                return true;
            case Intelligence.simpleGesture:
                //does not imply
                return true;
            case Intelligence.simpleSuperAI:
                //does not imply
                return true;
            case Intelligence.intelligentAI_One:
                return IntelligentHandSideDecision();
            default:
                return true;
                

        }
    }



    [SerializeField]
    private float xUnaccuracy;
    [SerializeField]
    private float zUnaccuracy;
    [SerializeField]
    private float forceUnaccuracy;
    [SerializeField]
    private float highBallChance;
    [SerializeField]
    private float middleBallChance;
    [SerializeField]
    private float lowBallChance;


    Vector3 targetOfPlayer1Left = new Vector3(-0.7f, 1.3f, 1.7f);
    Vector3 targetOfPlayer2Left = new Vector3(0.7f, 1.3f, -1.7f);
    Vector3 targetOfPlayer1Right = new Vector3(0.7f, 1.3f, 1.7f);
    Vector3 targetOfPlayer2Right = new Vector3(-0.7f, 1.3f, -1.7f);

    private Vector3 generateIntelligentAiHitForce()
    {
        //this is where all the intellingent shot logic should happen!
        //1. Choose which side to direct the hit

        Vector3 target = zeroVector3;

       // if (this.avatarObject.transform.position.x < 0.5f && this.avatarObject.transform.position.x > -0.5f)
       // {
            //location at center
            if (Random.value > 0.5f)
            {
                //shoot right
                if (sideIsOpposite) //player1
                    target = targetOfPlayer1Right;
                else //player2
                    target = targetOfPlayer2Right;
            }  else
            {
                //shoot left
                if (sideIsOpposite) //player1
                    target = targetOfPlayer1Left;
                else //player2
                    target = targetOfPlayer2Left;
            }
       // } else if (this.avatarObject.transform.position.x < 0.5f)


        target = new Vector3(target.x + (Mathf.Pow(2.0f * (Random.value - 0.5f), 2.0f)) * xUnaccuracy,
            target.y,
            target.z + (Mathf.Pow(2.0f * (Random.value - 0.5f), 2.0f)) * zUnaccuracy);


        float HitAngle = GenerateAiHitAngle();

        Vector3 forceVector = GenerateHitForceForIntelligentAIHelper(target, HitAngle);

        //use handSideJustChangedTimer here!!!!

        forceVector = forceVector + forceVector * (Mathf.Pow(2.0f * (Random.value - 0.5f), 2.0f)) * forceUnaccuracy;
        return forceVector;
        //dummy
        


    }

    private float GenerateAiHitAngle()
    {
        float toss = Random.value;

        float highBallRelativeChance = highBallChance / (highBallChance + middleBallChance + lowBallChance);
        float middleBallRelativeChance = middleBallChance / (highBallChance + middleBallChance + lowBallChance);
        float lowBallRelativeChance = lowBallChance / (highBallChance + middleBallChance + lowBallChance);



        if (toss < (highBallRelativeChance))
        {
            return 45.0f;
        }
        else if (toss < (highBallRelativeChance + middleBallRelativeChance))
        {
            return 30.0f;
        }
        else return 20.0f;
    }

    IEnumerator IntelligentAiWaitBeforeHitting(float waitSecs)
    {
        intelligentAiHitWaitStarted = true;
        yield return new WaitForSeconds(waitSecs);
        print("Hit wait reached");
        intelligentAiHits = true;
    }

    private void UpdatePadOrietationForAi()
    {
        if (avatarScript.frontHand() && !sideIsOpposite) this.transform.rotation = new Quaternion(-0.5f, -0.5f, 0.5f, 0.5f);
        else if (!avatarScript.frontHand() && !sideIsOpposite) this.transform.rotation = new Quaternion(0.5f, 0.5f, 0.5f, 0.5f);
        else if (avatarScript.frontHand() && sideIsOpposite) this.transform.rotation = new Quaternion(0.5f, 0.5f, 0.5f, 0.5f);
        else this.transform.rotation = new Quaternion(-0.5f, -0.5f, 0.5f, 0.5f);
    }
    

    private bool intelligentAiHits = false;
    private bool intelligentAiHitWaitStarted = false;

    private void ExecuteIntelligentAiOne()
    {
        if (simpleGestureCoolDown <= 0.0f) padBallDistance = Mathf.Min(padBallDistance + Time.deltaTime, 0.5f);
        else padBallDistance = Mathf.Max(padBallDistance - (Time.deltaTime * 10.0f), 0.0f);
        //gesture pad shoudn't collide
        this.GetComponent<CapsuleCollider>().enabled = false;

        //AI has no controller!
        //AI assumed to be right handed!
        UpdatePadOrietationForAi();

        float ballDistance = Mathf.Abs(defaultPosition.z - pingPongBall.transform.position.z);
        //print(ballDistance);
        //Time.timeScale = 1.0f;

        //let's reset hit wait etc. if ball goes to other side of the table. Could help with AI ghost hits.
        if (ballDistance > 2.5f)
        {
            intelligentAiHitWaitStarted = false;
            intelligentAiHits = false;
            simpleGestureCoolDown = 0.2f;
        }
            

        if (executeAdvancedPadPositioning(ballDistance) )
        {
            pingPongBallShine.SetActive(true);

            if (this.sideIsOpposite && pingPongReferee.Player1ShouldPass() || !this.sideIsOpposite && pingPongReferee.Player2ShouldPass())
            {
                if (simpleGestureCoolDown <= 0.0f && !intelligentAiHitWaitStarted) StartCoroutine(IntelligentAiWaitBeforeHitting(0.1f + Random.value * 0.2f));

                if (intelligentAiHits && simpleGestureCoolDown <= 0.0f)
                {
                    intelligentAiHitWaitStarted = false;
                    intelligentAiHits = false;
                    simpleGestureCoolDown = 1.0f;

                    this.GetComponent<AudioSource>().Play();

                    pingPongBall.GetComponent<Rigidbody>().velocity = zeroVector3;
                    Vector3 dynamicHitForce = GenerateDynamic3dDirectedHitForce();
                    if (!avatarScript.frontHand()) dynamicHitForce = dynamicHitForce * -1.0f;
                    print(dynamicHitForce.magnitude + " " + dynamicHitForce);
                    //pingPongBall.GetComponent<Rigidbody>().AddForce(dynamicHitForce, ForceMode.Impulse);
                    //pingPongBall.GetComponent<PingPongBall>().HitBall(dynamicHitForce);
                    //col.gameObject.GetComponent<Rigidbody>().AddForce(GenerateHitForce(), ForceMode.Impulse);
                    //pingPongBall.GetComponent<PingPongBall>().HitBall(GenerateHitForce());
                    pingPongBall.GetComponent<PingPongBall>().HitBall(generateIntelligentAiHitForce());

                    
                }
            }  

        }
        else
        {

            //PAD SHOULD CONTROL PINGPONG SHINE ONLY ON OWN SIDE!
            if (this.sideIsOpposite)
                if (pingPongBall.transform.position.z > 0.0f) pingPongBallShine.SetActive(false);
            else
                if (pingPongBall.transform.position.z < 0.0f) pingPongBallShine.SetActive(false);
            //this.transform.position = new Vector3(this.transform.position.x,
            //    this.transform.position.y,
            //    defaultPosition.z);
        }
    }

    private bool executeAdvancedPadPositioning(float ballDistance)
    {

        //return whether the ball is hittable or not.

        /**
        rules:
        -never on the wrong side of avatar
        -if ball is close, follow the ball 
        -if ball is far stay at side specific resting position

        **/


        //start and end of shift between rest and hitting poses
        float rampMax = 3.0f;
        float rampMin = 1.5f;

        Vector3 restPosition = avatarScript.padRestingPosition();
        Vector3 ballHittingPosition = pingPongBall.transform.position;

        Vector3 targetPosition;

        if (ballDistance > rampMax) { targetPosition = restPosition; }
        else if (ballDistance <= rampMax && ballDistance > rampMin)
        {
            targetPosition =
                Vector3.Lerp(restPosition, ballHittingPosition, (ballDistance - rampMin) / (rampMax - rampMin));
        }
        else { targetPosition = ballHittingPosition; }

        //never let pad to slip on wrong side of the avatar!
        //never let pad to slip out of arm reach!
        targetPosition = new Vector3(avatarScript.padXCorrection(targetPosition.x),
            targetPosition.y,
            targetPosition.z);

        Vector3 targetPositionWithOffset;

        //add offset around the ceter position
        //if you play with backhand the pad is on opposite side of the ball.
        if (avatarScript.frontHand())
            targetPositionWithOffset = targetPosition
                //+ (GvrController.Orientation * (Vector3.down * padBallDistance));
                + (this.transform.rotation * (Vector3.down * padBallDistance));
        else
            targetPositionWithOffset = targetPosition
               + (this.transform.rotation * (Vector3.up * padBallDistance));

        //this.transform.position = targetPositionWithOffset;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPositionWithOffset, 1 - Mathf.Exp(-20 * Time.deltaTime));

        return (avatarScript.ballXWithinArmReach() && ballDistance < rampMin);

    }

    private void ExecuteAdvancedGeture1()
    {
        if (simpleGestureCoolDown <= 0.0f) padBallDistance = Mathf.Min(padBallDistance + Time.deltaTime, 0.5f);
        else padBallDistance = Mathf.Max(padBallDistance - (Time.deltaTime * 10.0f), 0.0f);
        //gesture pad shoudn't collide
        this.GetComponent<CapsuleCollider>().enabled = false;
        //this.transform.position = new Vector3(this.transform.position.x, -4.0f, this.transform.position.z);


        //Lock rotation if touch is down

        if ((Application.platform == RuntimePlatform.Android && !GvrController.ClickButton) ||
            (Application.platform == RuntimePlatform.WindowsEditor && !GvrController.IsTouching) ||
            (Application.platform == RuntimePlatform.LinuxEditor && !GvrController.IsTouching) ||
            (Application.platform == RuntimePlatform.OSXEditor && !GvrController.IsTouching))
        {
            this.transform.rotation = GvrController.Orientation;
            //print("update orientation");
        }
                
        
        //this.transform.rotation = new Quaternion(GvrController.Orientation.x + 180f,
        //    GvrController.Orientation.y, GvrController.Orientation.z, GvrController.Orientation.w);




        float ballDistance = Mathf.Abs(defaultPosition.z - pingPongBall.transform.position.z);
        //print(ballDistance);
        //Time.timeScale = 1.0f;
        sceneController.setNormalTimeScale();

        //print(this.transform.rotation.ToString());

        //executeAdvancedPadPositioning(ballDistance);


        //print(avatarScript.ballXWithinArmReach().ToString());
        //if (avatarScript.ballXWithinArmReach() && ballDistance < 1.5f)
        if (executeAdvancedPadPositioning(ballDistance))
        {
            pingPongBallShine.SetActive(true);
            if (simpleGestureCoolDown <= 0.0f) sceneController.setSlowMotionTimeScale();

            //let's set pad position near to ping pong ball when it is hittable
            //this.transform.position = pingPongBall.transform.position
            //    + (GvrController.Orientation *  (Vector3.down * 0.5f ));
            //this.transform.position = pingPongBall.transform.position
            //    + (GvrController.Orientation * (Vector3.forward * 0.5f));
            if (AccelometerMagnitude() > 6.0f && 
                    ((Application.platform == RuntimePlatform.Android && GvrController.ClickButton) ||
                    (Application.platform == RuntimePlatform.WindowsEditor && GvrController.IsTouching) ||
                    (Application.platform == RuntimePlatform.LinuxEditor && GvrController.IsTouching) ||
                    (Application.platform == RuntimePlatform.OSXEditor && GvrController.IsTouching))
                    && AccelometerForward() && simpleGestureCoolDown <= 0.0f)
            {

                this.GetComponent<AudioSource>().Play();
                pingPongBall.GetComponent<Rigidbody>().velocity = zeroVector3;
                Vector3 dynamicHitForce = GenerateDynamic3dDirectedHitForce();
                if (!avatarScript.frontHand()) dynamicHitForce = dynamicHitForce * -1.0f;
                //print(dynamicHitForce.magnitude + " " + dynamicHitForce);
                //pingPongBall.GetComponent<Rigidbody>().AddForce(dynamicHitForce, ForceMode.Impulse);
                pingPongBall.GetComponent<PingPongBall>().HitBall(dynamicHitForce);
                simpleGestureCoolDown = 0.5f;
            }

        }
        else
        {

            //PAD SHOULD CONTROL PINGPONG SHINE ONLY ON OWN SIDE!
            if (this.sideIsOpposite)
                if (pingPongBall.transform.position.z > 0.0f) pingPongBallShine.SetActive(false);
                else
                if (pingPongBall.transform.position.z < 0.0f) pingPongBallShine.SetActive(false);
            //this.transform.position = new Vector3(this.transform.position.x,
            //    this.transform.position.y,
            //    defaultPosition.z);
        }
    }


    private void ExecuteManualMouse()
    {
        float minX = -3.0f;
        float maxX = 3.0f;
       
        float manualX = minX + ((Screen.width - Input.mousePosition.x) / Screen.width) * (maxX - minX);
        print(Input.mousePosition.x.ToString() + " " + Screen.width.ToString() + " " + manualX.ToString());
        this.transform.position = new Vector3(manualX, DynamicPadHeight(), this.transform.position.z);
    }

    private void ExecuteSimpleSuperAI()
    {
        //this.transform.position = new Vector3(pingPongBall.transform.position.x, pingPongBall.transform.position.y, this.transform.position.z);
        this.transform.position = new Vector3(pingPongBall.transform.position.x, DynamicPadHeight(), this.transform.position.z);
    }

    private float padBallDistance;

    

    private void ExecuteSimpleGesture()
    {
        if (simpleGestureCoolDown <= 0.0f) padBallDistance = Mathf.Min(padBallDistance + Time.deltaTime, 0.5f);
        else padBallDistance = Mathf.Max(padBallDistance - (Time.deltaTime * 10.0f), 0.0f);
        //gesture pad shoudn't collide
            this.GetComponent<CapsuleCollider>().enabled = false;
        //this.transform.position = new Vector3(this.transform.position.x, -4.0f, this.transform.position.z);

        this.transform.rotation = GvrController.Orientation;

        this.GetComponent<MeshRenderer>().material = transParentPadMaterial;

        float ballDistance = Mathf.Abs(defaultPosition.z - pingPongBall.transform.position.z);
        //print(ballDistance);
        //Time.timeScale = 1.0f;
        sceneController.setNormalTimeScale();
        
        this.transform.position = pingPongBall.transform.position
                + (GvrController.Orientation * (Vector3.down * padBallDistance));

        if (ballDistance < 1.5f)
        {
            pingPongBallShine.SetActive(true);
            if (simpleGestureCoolDown <= 0.0f) sceneController.setSlowMotionTimeScale();

            //let's set pad position near to ping pong ball when it is hittable
            //this.transform.position = pingPongBall.transform.position
            //    + (GvrController.Orientation *  (Vector3.down * 0.5f ));
            //this.transform.position = pingPongBall.transform.position
            //    + (GvrController.Orientation * (Vector3.forward * 0.5f));

            if (AccelometerSqrMagn() > 70.0f && simpleGestureCoolDown <= 0.0f)
            {
                
                
                pingPongBall.GetComponent<Rigidbody>().velocity = zeroVector3;
                //pingPongBall.GetComponent<Rigidbody>().AddForce(Generate3dDirectedHitForce(), ForceMode.Impulse);
                //pingPongBall.GetComponent<PingPongBall>().playPadSound();
                pingPongBall.GetComponent<PingPongBall>().HitBall(Generate3dDirectedHitForce());
                simpleGestureCoolDown = 0.5f;
            }

        }
        else
        {

            pingPongBallShine.SetActive(false);
            this.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y,
                defaultPosition.z);
        }
        
        
    }

    private bool AccelometerForward()
    {
        //Quaternion q = GvrController.Orientation;
        Quaternion q = this.transform.rotation;
        Vector3 gravity = new Vector3(0, 9.81f, 0);
        Vector3 orientationCorrectedAccel = q * GvrController.Accel;
        Vector3 oriAndGravCorrAccel = orientationCorrectedAccel - gravity;



        if (this.sideIsOpposite && oriAndGravCorrAccel.z > 0.0f) return true;
        else if (!this.sideIsOpposite && oriAndGravCorrAccel.z < 0.0f) return true;
        else return false;

    }

    private float AccelometerSqrMagn()
    {
        return (Mathf.Pow(AccelometerMagnitude(), 2.0f));
    }

    private float AccelometerMagnitude()
    {


        //Quaternion q = GvrController.Orientation;
        Quaternion q = this.transform.rotation;
        Vector3 gravity = new Vector3(0, 9.81f, 0);
        Vector3 orientationCorrectedAccel = q * GvrController.Accel;
        Vector3 oriAndGravCorrAccel = orientationCorrectedAccel - gravity;


        return oriAndGravCorrAccel.magnitude;
    }

    private float DynamicPadHeight() {
        float defaulHeight = 1.6f;
        float ballHeight = pingPongBall.transform.position.y;

        float maxDistance = 2.8f;
        float minDistance = 0.0f;
        float ballDistance = Mathf.Abs(this.transform.position.z - pingPongBall.transform.position.z);

        float fraction = ballDistance / (maxDistance - minDistance);
        if (fraction > 1.0f) fraction = 1.0f;
        if (fraction < 0.0f) fraction = 0.0f;
        return lerp(ballHeight, defaulHeight, fraction);
    }

    private float lerp(float a, float b, float f)
    {
        return (a * (1.0f - f)) + (b * f);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "ping pong ball")
        {
            col.gameObject.GetComponent<Rigidbody>().velocity = zeroVector3;
            //col.gameObject.GetComponent<Rigidbody>().AddForce(GenerateHitForce(), ForceMode.Impulse);
            pingPongBall.GetComponent<PingPongBall>().HitBall(GenerateHitForce());
        }
    }

    private Vector3 GenerateHitForceForIntelligentAIHelper(Vector3 target, float angle)
    {
        float sideMultiplier;
        if (sideIsOpposite) sideMultiplier = 1.0f;
        else sideMultiplier = -1.0f;

        float randomX = (Random.value - 0.5f) * 1.3f;

        //direction vector from ball to random target on opposite side of the table
        Vector3 hitVector = ((target + new Vector3(randomX, 0.0f, 0.0f))
            - pingPongBall.transform.position)
            .normalized;
        //remove up-down axis and normalize to length 1.
        hitVector = new Vector3(hitVector.x, 0.0f, hitVector.z).normalized;
        //change angle to face 25 degrees up
        hitVector = Quaternion.Euler(angle * -sideMultiplier, 0.0f, 0.0f) * hitVector;
        //correct to fixed force
        hitVector = hitVector * 0.06f;

        //High angle requires less force.
        
        if (angle > 40.0f) hitVector = hitVector * 0.92f;
        else if (angle <= 40.0f && angle > 25.0f) hitVector = hitVector * 0.95f;
        else hitVector = hitVector * 1.04f;

        return hitVector;
        //return new Vector3(0.0f, 0.025f, 0.05f * sideMultiplier);
    }

    private Vector3 GenerateHitForce()
    {
        float sideMultiplier;
        if (sideIsOpposite) sideMultiplier = 1.0f;
        else sideMultiplier = -1.0f;

        float randomX = (Random.value - 0.5f) * 1.3f;

        //direction vector from ball to random target on opposite side of the table
        Vector3 hitVector = ((hitTarget.transform.position + new Vector3(randomX, 0.0f, 0.0f)) 
            - pingPongBall.transform.position)
            .normalized;
        //remove up-down axis and normalize to length 1.
        hitVector = new Vector3(hitVector.x, 0.0f, hitVector.z).normalized;
        //change angle to face 25 degrees up
        hitVector = Quaternion.Euler(25.0f * -sideMultiplier, 0.0f, 0.0f) * hitVector;
        //correct to fixed force
        hitVector = hitVector * 0.06f;

        return hitVector;
        //return new Vector3(0.0f, 0.025f, 0.05f * sideMultiplier);
    }

    private Vector3 Generate2dDirectedHitForce()
    {
        //Quaternion q = GvrController.Orientation;
        Quaternion q = this.transform.rotation;
        Vector3 gravity = new Vector3(0, 9.81f, 0);
        Vector3 orientationCorrectedAccel = q * GvrController.Accel;
        Vector3 oriAndGravCorrAccel = orientationCorrectedAccel - gravity;

        

        float sideMultiplier;
        if (sideIsOpposite) sideMultiplier = 1.0f;
        else sideMultiplier = -1.0f;

        float randomX = (Random.value - 0.5f) * 1.3f;

        Vector3 hitVector = oriAndGravCorrAccel;
        hitVector = new Vector3(hitVector.x, 0.0f, hitVector.z).normalized;
        hitVector = Quaternion.Euler(25.0f * -sideMultiplier, 0.0f, 0.0f) * hitVector;
        hitVector = hitVector * 0.06f;

        return hitVector;
    
        //return new Vector3(0.0f, 0.025f, 0.05f * sideMultiplier);
    }

    private Vector3 Generate3dDirectedHitForce()
    {
        //Quaternion q = GvrController.Orientation;
        Quaternion q = this.transform.rotation;
        Vector3 gravity = new Vector3(0, 9.81f, 0);
        Vector3 orientationCorrectedAccel = q * GvrController.Accel;
        Vector3 oriAndGravCorrAccel = orientationCorrectedAccel - gravity;



        float sideMultiplier;
        if (sideIsOpposite) sideMultiplier = 1.0f;
        else sideMultiplier = -1.0f;

        float randomX = (Random.value - 0.5f) * 1.3f;

        //changed dir vector to orientation related!
        //Vector3 hitVector = oriAndGravCorrAccel.normalized;
        Vector3 hitVector = this.transform.rotation * (Vector3.up).normalized;
        hitVector = hitVector * 0.06f;
        
        return hitVector;

        //return new Vector3(0.0f, 0.025f, 0.05f * sideMultiplier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private Vector3 GenerateDynamic3dDirectedHitForce()

    {
        float forceAdjustor = 1.05f;
        Vector3 hitVector = Generate3dDirectedHitForce();

        //dyamic 
        float accel = AccelometerMagnitude();
        float forceFactor = Mathf.Max(0.85f, Mathf.Min(1.1f, DynamicForceFactorFunction(accel)));


        return hitVector * forceFactor * forceAdjustor;



    }

    private float DynamicForceFactorFunction(float accel)
    {
        return (0.014285714f * accel + 0.814285714f);
        //0,014285714	0,814285714
    }
}
