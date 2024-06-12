using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class PlayerAgent : Agent
{
    public Transform aimTarget;
    public PlayerAgent player_adversar;
    public int semn;

    [SerializeField] NNModel easy_model;
    [SerializeField] NNModel medium_model;
    [SerializeField] NNModel hard_model;
    public static bool isPaused = false;
    public static int count = 0;












    public GameObject aim_target_opus;

    public GameObject aim_target;

    public GameObject left_part_court;
    public GameObject right_part_court;

    public GameObject mijloc_serva;

    public enum Axis
    {
        X,
        Y,
        Z
    }
    float limitPosition = 0f;
    float currentPosition = 0f;
    public Axis axisToCompare;

    public KeyCode front;
    public KeyCode back;
    public KeyCode left;
    public KeyCode right;


    public KeyCode aim_left;
    public KeyCode aim_right;
    public KeyCode serve_1;

    public int variabila;
    public string hitter;

    public float variablia_v;


    public int inmultire_h;
    public int sens_minge;


    float speed = 3f;
    float force = 10f;
    bool wasPressedServe = false;


    private Rigidbody ballRigidbody;
    private Vector3 initialPosition;


    public Transform ball;

    [SerializeField] GameObject aim_target_left;
    [SerializeField] GameObject aim_target_right;
    [SerializeField] GameObject limit_player;





    int sens;
    bool move;
    public bool served = false;


    Animator animator;
    ShotManager shotManager;

    [SerializeField] Transform serveRight;
    [SerializeField] Transform serveLeft;

    public bool servedRight = true;


    Shot currentShot;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        shotManager = GetComponent<ShotManager>();
        currentShot = shotManager.shot;
        ballRigidbody = ball.GetComponent<Rigidbody>();
        initialPosition = transform.position;
        servedRight = true;
        InvokeRepeating("repeat_action", 0f, 1f); // Repeat the action every 1 second

        if (semn == -2)
        {

            if (PlayingOptions.s_gameMode == GameMode.EASY)
            {
                GetComponent<BehaviorParameters>().Model = easy_model;
                GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.InferenceOnly;

            }

            if (PlayingOptions.s_gameMode == GameMode.MEDIUM)
            {
                GetComponent<BehaviorParameters>().Model = medium_model;
                GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.InferenceOnly;

            }

            if (PlayingOptions.s_gameMode == GameMode.HARD)
            {
                GetComponent<BehaviorParameters>().Model = hard_model;
                GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.InferenceOnly;

            }

            if (PlayingOptions.s_gameMode == GameMode.MULTIPLAYER)
            {
                GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.HeuristicOnly;


            }
        }











        // Calculate the direction vectors from the local object's position to player_adversar and aim_target_opus

    }
    public override void OnEpisodeBegin()
    {
        // Reset positions and states
        transform.position = initialPosition;
        ball.position = initialPosition;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }
    public void endOfEpisode()
    {

        transform.position = initialPosition;
        ball.position = initialPosition;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        EndEpisode();
    }



    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent position
        sensor.AddObservation(transform.localPosition);
        Vector3 directionToPlayer = player_adversar.transform.position - transform.position;
        Vector3 directionToAimTarget = aim_target_opus.transform.position - transform.position;
        // Calculate the angle between the lines defined by the vectors to player_adversar and aim_target_opus
        float angleBetweenLines = Vector3.Angle(directionToPlayer, directionToAimTarget);
        sensor.AddObservation(angleBetweenLines);


        Vector3 direction_player_1 = player_adversar.transform.position - transform.position;
        Vector3 direction_to_target = aimTarget.transform.position - transform.position;
        float angle_between_lines = Vector3.Angle(direction_player_1, direction_to_target);
        sensor.AddObservation(angle_between_lines);

        sensor.AddObservation(Vector3.Distance(transform.localPosition, ball.localPosition));


        sensor.AddObservation(Math.Abs(aim_target_left.transform.position.z - aimTarget.position.z));
        sensor.AddObservation(Math.Abs(aim_target_right.transform.position.z - aimTarget.position.z));

        if (ball.GetComponent<Ball>().end_point == 0 + variabila)
        {
            sensor.AddObservation((mijloc_serva.transform.position.z - aim_target.transform.position.z) * semn);

        }
        if (ball.GetComponent<Ball>().end_point == 2 + variabila)
        {
            sensor.AddObservation((aim_target.transform.position.z - mijloc_serva.transform.position.z) * semn);

        }
        // Ball position
        sensor.AddObservation(ball.transform.localPosition);
        sensor.AddObservation(ballRigidbody.velocity);
        sensor.AddObservation(player_adversar.transform.localPosition);

        sensor.AddObservation(Vector3.Distance(transform.localPosition, aim_target_opus.transform.localPosition));



        // Opponent position (assuming a reference to the opponent is available)
        // sensor.AddObservation(opponent.localPosition);

        // sensor.AddObservation(player_adversar.transform.position);

        // End point

    }


    void repeat_action()
    {
        if (ball.GetComponent<Ball>().end_point == 0 + variabila)
        {

            //Debug.Log(IsObjectInside(aim_target, left_part_court));
            //Debug.Log((mijloc_serva.transform.position.z - aim_target.transform.position.z) * semn);



        }
        if (ball.GetComponent<Ball>().end_point == 2 + variabila)
        {
            //Debug.Log(IsObjectInside(aim_target, right_part_court));
            //Debug.Log((aim_target.transform.position.z - mijloc_serva.transform.position.z) * semn);

        }

    }
    void MoveAgent(ActionSegment<int> act)
    {





        var discreteActions = act;

        float h = 0f;
        float v = 0f;

        float h_aim = 0f;
        float v_aim = 0f;

        // Horizontal movement
        if (discreteActions[2] == 1)
        {
            h = 1f; // Right
            sens = 1;
            move = true;
            v = -variablia_v;

        }
        else if (discreteActions[2] == 2)
        {
            h = -1f;
            sens = -1;
            move = true;// Left
            v = variablia_v;
        }

        // Vertical movement
        if (discreteActions[3] == 1)
        {
            v = 1f;
            move = false;// Forward
        }
        else if (discreteActions[3] == 2)
        {
            v = -1f; // Backward
            move = false;
        }

        if (discreteActions[0] == 1)
        {
            h_aim = 1f;
            v_aim = -variablia_v;
        }
        else if (discreteActions[0] == 2)
        {
            h_aim = -1f;
            v_aim = variablia_v;
        }




        switch (discreteActions[1])
        {

            case 0:
                if ((ball.GetComponent<Ball>().end_point == 0 + variabila || ball.GetComponent<Ball>().end_point == 2 + variabila) && wasPressedServe)

                {

                    served = true;
                    ball.GetComponent<Ball>().index++;
                    AddReward(0.01f);


                    wasPressedServe = false;
                    GetComponent<BoxCollider>().enabled = true;
                    ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);

                    Vector3 dir = aimTarget.position - transform.position;
                    ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
                    animator.Play("serve");
                    ball.GetComponent<Ball>().end_point = (ball.GetComponent<Ball>().end_point + 1) % 8;


                    ball.GetComponent<Ball>().hitter = hitter;
                    ball.GetComponent<Ball>().playing = true;



                }
                break;



            case 1:
                if (ball.GetComponent<Ball>().end_point == 0 + variabila || ball.GetComponent<Ball>().end_point == 2 + variabila)
                {
                    AddReward(0.0001f);
                    wasPressedServe = true;



                    currentShot = shotManager.Serve;
                    GetComponent<BoxCollider>().enabled = false;
                    animator.Play("serve-prepare");


                }
                break;
        }


        if (h_aim != 0)
        {



            if (aim_target_left.transform.position.z > aimTarget.position.z)
            {
                Debug.Log("tinta_blocat");
                AddReward(-0.5f);
                aimTarget.Translate(new Vector3(0, 0, -Math.Abs(h_aim)) * speed * Time.deltaTime);


            }


            else if (aim_target_right.transform.position.z < aimTarget.position.z)
            {
                AddReward(-0.5f);
                Debug.Log("tinta_blocat");

                aimTarget.Translate(new Vector3(0, 0, Math.Abs(h_aim)) * speed * Time.deltaTime);

            }
            else
            {

                aimTarget.Translate(new Vector3(0, 0, h_aim) * speed * Time.deltaTime);
            }

        }

        if ((h != 0 || v != 0))
        {


            switch (axisToCompare)
            {
                case Axis.X:
                    limitPosition = limit_player.transform.position.x;
                    currentPosition = transform.position.x;
                    break;
                case Axis.Y:
                    limitPosition = limit_player.transform.position.y;
                    currentPosition = transform.position.y;
                    break;
                case Axis.Z:
                    limitPosition = limit_player.transform.position.z;
                    currentPosition = transform.position.z;
                    break;
            }





            if (ball.GetComponent<Ball>().end_point == 0 + variabila && limitPosition < currentPosition)
            {

                if (move == true)
                {
                    transform.Translate(new Vector3(v, 0, -Math.Abs(h)) * speed * Time.deltaTime);
                    ball.GetComponent<Ball>().transform.Translate(new Vector3(v, 0, -inmultire_h * Math.Abs(h)) * speed * Time.deltaTime);
                }

            }

            else if (ball.GetComponent<Ball>().end_point == 2 + variabila && limitPosition > currentPosition)
            {


                if (move == true)
                {
                    transform.Translate(new Vector3(v, 0, Math.Abs(h)) * speed * Time.deltaTime);
                    ball.GetComponent<Ball>().transform.Translate(new Vector3(v, 0, inmultire_h * Math.Abs(h)) * speed * Time.deltaTime);
                }

            }
            else
            {

                if (ball.GetComponent<Ball>().end_point % 2 == 0)
                {
                    v = -variablia_v;
                    h = 1;
                    transform.Translate(new Vector3(-v * sens, 0, h * sens) * speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(new Vector3(-v, 0, h) * speed * Time.deltaTime);
                }
                if (ball.GetComponent<Ball>().end_point == 0 + variabila || ball.GetComponent<Ball>().end_point == 2 + variabila)
                {
                    AddReward(0.0000000001f);
                    ball.GetComponent<Ball>().transform.Translate(new Vector3(v * sens, 0, sens_minge * sens * h) * speed * Time.deltaTime);
                }
            }


        }




    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && semn == 1)
        {
            TogglePause();
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);

    }

    void TogglePause()
    {
        isPaused = !isPaused;
        count++;

        if (isPaused && count == 1)
        {
            Time.timeScale = 0f; // Pauses the game
            SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
        }



    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {



        var actions = actionsOut.DiscreteActions;

        if (Input.GetKey(right))
        {
            actions[2] = 1; // Right
        }
        else if (Input.GetKey(left))
        {
            actions[2] = 2; // Left
        }

        if (Input.GetKey(front))
        {
            actions[3] = 1; // Forward
        }
        else if (Input.GetKey(back))
        {
            actions[3] = 2; // Backward
        }


        if (Input.GetKey(serve_1))
        {
            actions[1] = 1;
        }
        else
        {
            actions[1] = 0;
        }

        if (Input.GetKey(aim_left))
        {
            actions[0] = 2;
        }
        else if (Input.GetKey(aim_right))
        {
            actions[0] = 1;
        }

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && ball.GetComponent<Ball>().end_point % 2 == 1)
        {

            if (ball.GetComponent<Ball>().index >= 2)
            {
                AddReward(0.1f);
                player_adversar.AddReward(0.1f);
                served = false;


                //trebuie sa verific cine loveste mingea 


                Vector3 direction_player_1 = player_adversar.transform.position - transform.position;
                Vector3 direction_to_target = aimTarget.transform.position - transform.position;

                float angle_between_lines = Vector3.Angle(direction_player_1, direction_to_target);

                served = false;



                if (angle_between_lines > 6f)
                {
                    AddReward(0.5f);
                    Debug.Log("bun");
                }
                else
                {
                    Debug.Log("rau");
                }



            }
            ball.GetComponent<Ball>().index++;



            Vector3 dir = aimTarget.position - transform.position;

            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
            animator.Play("backend");
            Vector3 ballDir = ball.position - transform.position;
            if (ballDir.x >= 0)
                animator.Play("forehand");
            else
                animator.Play("backend");

            ball.GetComponent<Ball>().hitter = hitter;


        }
    }




    public void Reset()
    {
        ball.GetComponent<Ball>().index = 0;
        if (servedRight)
        {
            transform.position = serveRight.position;
        }
        else
        {
            transform.position = serveLeft.position;
        }

        if (ball.GetComponent<Ball>().end_point == 0 + variabila)
        {
            ball.position = serveLeft.position;
        }

        if (ball.GetComponent<Ball>().end_point == 2 + variabila)
        {
            ball.position = serveRight.position;
        }
        servedRight = !servedRight;
    }
}
