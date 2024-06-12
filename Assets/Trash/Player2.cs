using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Integrations.Match3;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Jobs;

public class Player2 : Agent
{
    /*
    public Transform aimTarget;

    float speed = 3f;
    float force = 10f;
    bool wasPressedServe = false;
    bool wasPressedU = false;
    bool wasPressedY = false;




    public Transform ball;

    private Rigidbody ballRigidbody;
    private Vector3 initialPosition;



    bool hitting;


    Animator animator;
    ShotManager shotManager;

    [SerializeField] Transform serveRight2;
    [SerializeField] Transform serveLeft2;


    [SerializeField] GameObject aim_target_2_left;
    [SerializeField] GameObject aim_target_2_right;
    [SerializeField] GameObject limit_player2;

    bool servedRight = true;

    int sens;
    bool move;



    Shot currentShot;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        shotManager = GetComponent<ShotManager>();
        currentShot = shotManager.topSpin;
        ballRigidbody = ball.GetComponent<Rigidbody>();
        initialPosition = transform.position;
        servedRight = true;




    }

    public override void OnEpisodeBegin()
    {
        // Reset positions and states
        transform.position = initialPosition;
        ball.position = initialPosition;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent position
        sensor.AddObservation(transform.localPosition);

        // Ball position and velocity
        sensor.AddObservation(ball.localPosition);
        sensor.AddObservation(ballRigidbody.velocity);

        // Opponent position (assuming a reference to the opponent is available)
        // sensor.AddObservation(opponent.localPosition);

        // End point
        sensor.AddObservation(ball.GetComponent<Ball>().end_point);
    }
    public void endOfEpisode()
    {

        transform.position = initialPosition;
        ball.position = initialPosition;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        EndEpisode();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);

    }

    void MoveAgent(ActionSegment<int> act)
    {


        var discreteActions = act;

        float h = 0f;
        float v = 0f;

        // Horizontal movement
        if (discreteActions[3] == 1)
        {
            h = 1f; // Right
            sens = 1;
            v = -1 / 3.5f;
            move = true;
        }
        else if (discreteActions[3] == 2)
        {
            h = -1f;
            sens = -1;
            v = 1 / 3.5f;
            move = true;

        }

        // Vertical movement
        if (discreteActions[4] == 1)
        {
            v = 1f; // Forward

            move = false;


        }
        else if (discreteActions[4] == 2)
        {
            v = -1f;
            move = false;

            // Backward
        }


        switch (discreteActions[0])
        {
            // action: nothing
            case 0:
                if (wasPressedU)
                {
                    wasPressedU = false;
                    hitting = false;

                }
                break;

            // action: pressed
            case 1:
                wasPressedU = true;
                hitting = true;
                currentShot = shotManager.topSpin;
                break;

                break;
        }

        switch (discreteActions[1])
        {
            // action: nothing
            case 0:
                if (wasPressedY)
                {
                    wasPressedY = false;
                    hitting = false;

                }
                break;

            // action: pressed
            case 1:
                wasPressedY = true;
                hitting = true;
                currentShot = shotManager.topSpin;
                break;

                break;
        }

        switch (discreteActions[2])
        {
            case 0:
                if ((ball.GetComponent<Ball>().end_point == 4 || ball.GetComponent<Ball>().end_point == 6) && wasPressedServe)

                {
                    hitting = false;
                    wasPressedServe = false;
                    GetComponent<BoxCollider>().enabled = true;
                    ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
                    Vector3 dir = aimTarget.position - transform.position;
                    ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
                    animator.Play("serve");
                    ball.GetComponent<Ball>().end_point = (ball.GetComponent<Ball>().end_point + 1) % 8;


                    ball.GetComponent<Ball>().hitter = "player2";
                    ball.GetComponent<Ball>().playing = true;
                }
                break;



            case 1:
                if (ball.GetComponent<Ball>().end_point == 4 || ball.GetComponent<Ball>().end_point == 6)
                {
                    wasPressedServe = true;

                    hitting = true;
                    currentShot = shotManager.kickServe;
                    GetComponent<BoxCollider>().enabled = false;
                    animator.Play("serve-prepare");



                }
                break;


            case 2:
                if (ball.GetComponent<Ball>().end_point == 4 || ball.GetComponent<Ball>().end_point == 6)
                {
                    wasPressedServe = true;


                    hitting = true;
                    currentShot = shotManager.flatServe;
                    GetComponent<BoxCollider>().enabled = false;
                    animator.Play("serve-prepare");



                }
                break;
        }

        if (hitting)
        {

            if (aim_target_2_left.transform.position.z > aimTarget.position.z)
            {
                aimTarget.Translate(new Vector3(0, 0, -Math.Abs(h)) * speed * Time.deltaTime);


            }


            else if (aim_target_2_right.transform.position.z < aimTarget.position.z)
            {

                aimTarget.Translate(new Vector3(0, 0, Math.Abs(h)) * speed * Time.deltaTime);

            }
            else
            {
                aimTarget.Translate(new Vector3(0, 0, h) * speed * Time.deltaTime);
            }

        }

        if ((h != 0 || v != 0) && !hitting)
        {









            if (ball.GetComponent<Ball>().end_point == 4 && limit_player2.transform.position.z < transform.position.z)
            {
                if (move == true)
                {
                    transform.Translate(new Vector3(v, 0, -Math.Abs(h)) * speed * Time.deltaTime);
                    ball.GetComponent<Ball>().transform.Translate(new Vector3(v, 0, Math.Abs(h)) * speed * Time.deltaTime);
                }

            }

            else if (ball.GetComponent<Ball>().end_point == 6 && limit_player2.transform.position.z > transform.position.z)
            {
                if (move == true)
                {
                    transform.Translate(new Vector3(v, 0, Math.Abs(h)) * speed * Time.deltaTime);
                    ball.GetComponent<Ball>().transform.Translate(new Vector3(v, 0, -Math.Abs(h)) * speed * Time.deltaTime);
                }

            }
            else
            {
                if (ball.GetComponent<Ball>().end_point % 2 == 0)
                {
                    v = -1 / 3.5f;
                    h = 1;
                    transform.Translate(new Vector3(-v * sens, 0, h * sens) * speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(new Vector3(-v, 0, h) * speed * Time.deltaTime);
                }
                if (ball.GetComponent<Ball>().end_point == 4 || ball.GetComponent<Ball>().end_point == 6)
                {
                    ball.GetComponent<Ball>().transform.Translate(new Vector3(v * sens, 0, -sens * h) * speed * Time.deltaTime);
                }
            }
        }





    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;



        if (Input.GetKey(KeyCode.L))
        {
            actions[3] = 1; // Right
        }
        else if (Input.GetKey(KeyCode.J))
        {
            actions[3] = 2; // Left
        }

        if (Input.GetKey(KeyCode.I))
        {
            actions[4] = 1; // Forward
        }
        else if (Input.GetKey(KeyCode.K))
        {
            actions[4] = 2; // Backward
        }

        if (Input.GetKey(KeyCode.U))
        {
            actions[0] = 1;
        }
        else
        {
            actions[0] = 0;
        }

        if (Input.GetKey(KeyCode.Y))
        {
            actions[1] = 1;
        }
        else
        {
            actions[1] = 0;
        }
        if (Input.GetKey(KeyCode.T))
        {
            actions[2] = 1;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            actions[2] = 2;
        }
        else
        {
            actions[2] = 0;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && ball.GetComponent<Ball>().end_point % 2 == 1)
        {
            AddReward(0.2f);
            Vector3 dir = aimTarget.position - transform.position;


            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
            animator.Play("backend");
            Vector3 ballDir = ball.position - transform.position;
            if (ballDir.x >= 0)
                animator.Play("forehand");
            else
                animator.Play("backend");

            ball.GetComponent<Ball>().hitter = "player2";

        }
    }


    public void Reset()
    {






        if (servedRight)
        {
            transform.position = serveRight2.position;





        }
        else
        {
            transform.position = serveLeft2.position;

        }
        servedRight = !servedRight;

        if (ball.GetComponent<Ball>().end_point == 4)
        {
            ball.position = serveLeft2.position;
        }

        if (ball.GetComponent<Ball>().end_point == 6)
        {
            ball.position = serveRight2.position;
        }




    }
    */
}
