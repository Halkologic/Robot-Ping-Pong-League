using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongReferee : MonoBehaviour {

    //OppositeSide = player1
    //ThisSide = player2

    private int pointsPerGame = 11;
    private int gamesToWinAMatch = 2;


    private enum PointState {
        player1Serves,
        player1Served,
        player1ServedBouncedOwnSide,
        player1ServedBouncedOpponentsSide,
        player2Passed,
        player2Bounced,
        player1Passed,
        player1Bounced,
        player2Serves,
        player2Served,
        player2ServedBouncedOwnSide,
        player2ServedBouncedOpponentsSide,
    };
    private PointState pointState;

    private enum NextServer {player1, player2};
    private NextServer nextServer;
    private int bounces = 0;

    [SerializeField]
    private PingPongBall pingPongBall;


    public delegate void ScoreChangeAction();
    public static event ScoreChangeAction ScoreChanged;

    public delegate void PointToPlayer1Action();
    public static event PointToPlayer1Action PointToPlayer1Event;

    public delegate void PointToPlayer2Action();
    public static event PointToPlayer1Action PointToPlayer2Event;

    public delegate void GameToPlayer1Action();
    public static event GameToPlayer1Action GameToPlayer1Event;

    public delegate void GameToPlayer2Action();
    public static event GameToPlayer1Action GameToPlayer2Event;

    public delegate void MatchToPlayer1Action();
    public static event MatchToPlayer1Action MatchToPlayer1Event;

    public delegate void MatchToPlayer2Action();
    public static event MatchToPlayer1Action MatchToPlayer2Event;

    private int player1Points = 0;
    private int player2Points = 0;
    private int player1Games = 0;
    private int player2Games = 0;

    public int Player1Points
    {
        get { return player1Points; }
        private set { player1Points = value; }
    }

    public int Player2Points
    {
        get { return player2Points; }
        private set { player2Points = value; }
    }

    public int Player1Games
    {
        get { return player1Games; }
        private set { player1Games = value; }
    }

    public int Player2Games
    {
        get { return player2Games; }
        private set { player2Games = value; }
    }

    // Use this for initialization
    void Awake()
    {
        if (Random.value > 0.5f)
        {
            nextServer = NextServer.player1;
            pointState = PointState.player1Serves;
        }
        else
        {
            nextServer = NextServer.player2;
            pointState = PointState.player2Serves;
        }
    }

    void Start () {
        if (ScoreChanged != null) ScoreChanged();
    }

    public void StartMatch()
    {
        ResetBall();

    }

    void OnEnable()
    {
        PingPongBall.Player1Hit += JudgePlayer1Hit;
        PingPongBall.Player2Hit += JudgePlayer2Hit;
        PingPongBall.TableBouncePlayer1Side += JudgePlayer1SideBounce;
        PingPongBall.TableBouncePlayer2Side += JudgePlayer2SideBounce;
        PingPongBall.FloorBounce += JudgeFloorBounce;
    }


    void OnDisable()
    {
        PingPongBall.Player1Hit -= JudgePlayer1Hit;
        PingPongBall.Player2Hit -= JudgePlayer2Hit;
        PingPongBall.TableBouncePlayer1Side -= JudgePlayer1SideBounce;
        PingPongBall.TableBouncePlayer2Side -= JudgePlayer2SideBounce;
        PingPongBall.FloorBounce -= JudgeFloorBounce;
    }

    private void JudgePlayer1Hit()
    {
        switch (pointState)
        {
            case PointState.player1Serves:
                serveCount++;
                pointState = PointState.player1Served;
                break;

            case PointState.player1Served:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player1ServedBouncedOwnSide:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player1ServedBouncedOpponentsSide:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player2Passed:
                //If player1 hits the ball before it bounces, it's called volleying. 
                //In reality this is complicated case and it depends on situation whether it is allowed to hit the ball
                //For the sake of simplicity, here it is always a fault. 
                //Point to player2
                PointToPlayer2();
                break;

            case PointState.player2Bounced:
                //now is the moment when it is legal for player1 to hit the ball.
                pointState = PointState.player1Passed;
                break;

            case PointState.player1Passed:
                //player1 has just passed the ball. (s)he cannot hit it again.
                //point to player2
                PointToPlayer2();
                break;

            case PointState.player1Bounced:
                //it's player2's turn to hit. not player1's
                //point to player2
                PointToPlayer2();
                break;

            case PointState.player2Serves:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player2Served:
                //too early! point to player2
                PointToPlayer2();
                break;

            case PointState.player2ServedBouncedOwnSide:
                //too early! point to player2
                PointToPlayer2();
                break;

            case PointState.player2ServedBouncedOpponentsSide:
                //now is the moment when it is legal for player1 to hit the ball.
                pointState = PointState.player1Passed;
                break;
        }
            
    }

    private void JudgePlayer2Hit()
    {
        switch (pointState)
        {
            case PointState.player1Serves:
                //Does not make much sense, but not allowed. Point to player1
                PointToPlayer1();
                break;

            case PointState.player1Served:
                //Does not make much sense, but not allowed. Point to player1
                PointToPlayer1();
                break;

            case PointState.player1ServedBouncedOwnSide:
                //Does not make much sense, but not allowed. Point to player1
                PointToPlayer1();
                break;

            case PointState.player1ServedBouncedOpponentsSide:
                //now is the moment when it is legal for player2 to hit the ball.
                pointState = PointState.player2Passed;
                break;

            case PointState.player2Passed:
                //player2 has just passed the ball. (s)he cannot hit it again.
                //point to player1
                PointToPlayer1();
                break;

            case PointState.player2Bounced:
                //it's player1's turn to hit. not player2's
                //point to player1
                PointToPlayer1();
                break;

            case PointState.player1Passed:
                //If player2 hits the ball before it bounces, it's called volleying. 
                //In reality this is complicated case and it depends on situation whether it is allowed to hit the ball
                //For the sake of simplicity, here it is always a fault. 
                //Point to player1
                PointToPlayer1();
                break;

            case PointState.player1Bounced:
                //now is the moment when it is legal for player1 to hit the ball.
                pointState = PointState.player2Passed;
                break;

            case PointState.player2Serves:
                serveCount++;
                pointState = PointState.player2Served;
                break;

            case PointState.player2Served:
                //Does not make much sense, but not allowed. Point to player1
                PointToPlayer1();
                break;

            case PointState.player2ServedBouncedOwnSide:
                //Does not make much sense, but not allowed. Point to player1
                PointToPlayer1();
                break;

            case PointState.player2ServedBouncedOpponentsSide:
                //Does not make much sense, but not allowed. Point to player1
                PointToPlayer1();
                break;
        }
    }

    private void JudgePlayer1SideBounce()
    {
        switch (pointState)
        {
            case PointState.player1Serves:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player1Served:
                //First serve bounce on own side of the table
                pointState = PointState.player1ServedBouncedOwnSide;
                break;

            case PointState.player1ServedBouncedOwnSide:
                //Second bounce on own side not allowed! Point to player2
                PointToPlayer2();
                break;

            case PointState.player1ServedBouncedOpponentsSide:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player2Passed:
                //normal bounce on opposite side of the table.
                pointState = PointState.player2Bounced;
                break;

            case PointState.player2Bounced:
                //Second bounce not allowed! Point to player1
                PointToPlayer1();
                break;

            case PointState.player1Passed:
                //No bounces on own side allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player1Bounced:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player2Serves:
                //Does not make much sense, but not allowed. Point to player1
                PointToPlayer1();
                break;

            case PointState.player2Served:
                //First bounce should be on own side. Not allowed. Point to player1
                PointToPlayer1();
                break;

            case PointState.player2ServedBouncedOwnSide:
                //Second bounce is allowed on opponent's side
                pointState = PointState.player2ServedBouncedOpponentsSide;
                break;

            case PointState.player2ServedBouncedOpponentsSide:
                //Two bounce's on opponent's side is a fault. Point to player1
                PointToPlayer1();
                break;
        }
    }

    private void JudgePlayer2SideBounce()
    {
        switch (pointState)
        {
            case PointState.player1Serves:
                //Does not make much sense, but not allowed. Point to player2
                PointToPlayer2();
                break;

            case PointState.player1Served:
                //Ball need's to bounce on own side first. Point to player2
                PointToPlayer2();
                break;

            case PointState.player1ServedBouncedOwnSide:
                //Completely legal bounce!
                pointState = PointState.player1ServedBouncedOpponentsSide;
                break;

            case PointState.player1ServedBouncedOpponentsSide:
                //Two bounces on opponent's side os no no. Point to player2
                PointToPlayer2();
                break;

            case PointState.player2Passed:
                //Bounce on own side is not allowed! Point to player1
                PointToPlayer1();
                break;

            case PointState.player2Bounced:
                //Still not allowed to bounce on own side. Point to player1
                PointToPlayer1();
                break;

            case PointState.player1Passed:
                //Bounce on opponent's side allowed
                pointState = PointState.player1Bounced;
                break;

            case PointState.player1Bounced:
                //Second bounce is a fault. Point to player2
                PointToPlayer2();
                break;

            case PointState.player2Serves:
                //Does not make much sense, but bounce before serve is fault anyway. Point to player1
                PointToPlayer1();
                break;

            case PointState.player2Served:
                //First bounce shound hit the own side of the table. Allowed bounce!
                pointState = PointState.player2ServedBouncedOwnSide;
                break;

            case PointState.player2ServedBouncedOwnSide:
                //Second bounce on own side is not allowed. It's a fault. Point to player1
                PointToPlayer1();
                break;

            case PointState.player2ServedBouncedOpponentsSide:
                //Third serve bounce on own side sounds unlikely, but it's a fault never the less. Point to player1
                PointToPlayer1();
                break;
        }
    }

    private void JudgeFloorBounce()
    {
        switch (pointState)
        {
            case PointState.player1Serves:
                //Does not seem likely scenario, but if realized -> Point to player2
                PointToPlayer2();
                break;

            case PointState.player1Served:
                //If serve goes to floor, it's point to player2!
                PointToPlayer2();
                break;

            case PointState.player1ServedBouncedOwnSide:
                //Serve that bounces to floor through own side of the table is a fault! -> point to player2
                PointToPlayer2();
                break;

            case PointState.player1ServedBouncedOpponentsSide:
                //If player2 forgets to pass it back, it's point to player1
                PointToPlayer1();
                break;

            case PointState.player2Passed:
                //Pass to the floor, and player1 takes the point
                PointToPlayer1();
                break;

            case PointState.player2Bounced:
                //If player1 forgets to pass it back, it's point to player2
                PointToPlayer2();
                break;

            case PointState.player1Passed:
                //Pass to the floor and player2 takes the point
                PointToPlayer2();
                break;

            case PointState.player1Bounced:
                //Bounce and out is player2 fault. Point to player1
                PointToPlayer1();
                break;

            case PointState.player2Serves:
                //If this somehow can happen, it must be point to player1
                PointToPlayer1();
                break;

            case PointState.player2Served:
                //Don't serve to floor! point to player1
                PointToPlayer1();
                break;

            case PointState.player2ServedBouncedOwnSide:
                //One bounce and out does not equal legal serve. point to player1
                PointToPlayer1();
                break;

            case PointState.player2ServedBouncedOpponentsSide:
                //Maybe player1 forgot to pass it back? point to player2
                PointToPlayer2();
                break;
        }
    }


    private void ResetBall()
    {
        UpdateServer();
        if (nextServer == NextServer.player1)
        {
            pingPongBall.resetBallToPlayer1();
            
        }
        else
        {
            pingPongBall.resetBallToPlayer2();
            
        }
            
    }

    private int serveCount = 0;
    private void UpdateServer()
    {
        if (serveCount >= 2)
        {
            if (nextServer == NextServer.player1) nextServer = NextServer.player2;
            else nextServer = NextServer.player1;
            serveCount = 0;
        }
        if (nextServer == NextServer.player1) { pointState = PointState.player1Serves; }
        else { pointState = PointState.player2Serves; }
    }

    private void PointToPlayer1()
    {

        ResetBall();
        
        player1Points++;
        if (PointToPlayer1Event != null) PointToPlayer1Event();
        if (ScoreChanged != null) ScoreChanged();

        //Check if game is won
        if ((player1Points >= pointsPerGame) && 
            (player1Points > (player2Points + 1)))
        {
            player1Games++;
            player1Points = 0;
            Player2Points = 0;
            if (player1Games >= gamesToWinAMatch)
            {
                if (MatchToPlayer1Event != null) MatchToPlayer1Event();
                MatchWonPlayer1();
            }
            else
            {
                if (GameToPlayer1Event != null) GameToPlayer1Event();
            }
        } else
        {
            if (PointToPlayer1Event != null) PointToPlayer1Event();
        }
    }

    private void PointToPlayer2()
    {

        ResetBall();

        player2Points++;
        if (PointToPlayer2Event != null) PointToPlayer2Event();
        if (ScoreChanged != null) ScoreChanged();

        //Check if game is won
        if ((player2Points >= pointsPerGame) && 
            (player2Points > (player1Points + 1)))
        {
            player2Games++;
            player1Points = 0;
            Player2Points = 0;
            if (player2Games >= gamesToWinAMatch)
            {
                if (MatchToPlayer2Event != null) MatchToPlayer2Event();
                MatchWonPlayer2();
            }
            else
            {
                if (GameToPlayer2Event != null) GameToPlayer2Event();
            }
        }
        else
        {
            if (PointToPlayer2Event != null) PointToPlayer2Event();
        }
    }




    // depricated
    private void MatchWonPlayer1()
    {
        print("PLAYER 1 WON!!!!");
      //  Application.LoadLevel("lockerroomscene");
    }

    // depricated
    private void MatchWonPlayer2()
    {
        print("PLAYER 2 WON!!!!");
       // Application.LoadLevel("lockerroomscene");
    }

    public bool Player1ShouldPass()
    {
        return pointState == PointState.player2Bounced || pointState == PointState.player2ServedBouncedOpponentsSide;
    }

    public bool Player2ShouldPass()
    {
        return pointState == PointState.player1Bounced || pointState == PointState.player1ServedBouncedOpponentsSide;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
