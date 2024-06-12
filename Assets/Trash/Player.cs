using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public class Player : Agent
{
    /*
    public Transform aimTarget;
    public KeyCode front;



    float speed = 3f;
    float force = 10f;
    bool wasPressedM = false;
    bool wasPressedN = false;
    bool wasPressedServe = false;

    private Rigidbody ballRigidbody;
    private Vector3 initialPosition;


    public Transform ball;

    [SerializeField]  GameObject aim_target_1_left;
    [SerializeField]  GameObject aim_target_1_right;
    [SerializeField] GameObject limit_player1;
    




    bool hitting;


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

        // Ball position and velocity
        sensor.AddObservation(ball.localPosition);
        sensor.AddObservation(ballRigidbody.velocity);

        // Opponent position (assuming a reference to the opponent is available)
        // sensor.AddObservation(opponent.localPosition);

        // End point
        sensor.AddObservation(ball.GetComponent<Ball>().end_point);
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
        }
        else if (discreteActions[3] == 2)
        {
            h = -1f; // Left
        }

        // Vertical movement
        if (discreteActions[4] == 1)
        {
            v = 1f; // Forward
        }
        else if (discreteActions[4] == 2)
        {
            v = -1f; // Backward
        }


        switch (discreteActions[0])
        {
            // action: nothing
            case 0:
                if (wasPressedM)
                {
                    wasPressedM = false;
                    hitting = false;

                }
                break;

            // action: pressed
            case 1:
                wasPressedM = true;
                hitting = true;
                currentShot = shotManager.topSpin;
                break;

                break;
        }

        switch (discreteActions[1])
        {
            // action: nothing
            case 0:
                if (wasPressedN)
                {
                    wasPressedN = false;
                    hitting = false;

                }
                break;

            // action: pressed
            case 1:
                wasPressedN = true;
                hitting = true;
                currentShot = shotManager.topSpin;
                break;

                break;
        }

        switch (discreteActions[2])
        {
            case 0:
                if ((ball.GetComponent<Ball>().end_point == 0 || ball.GetComponent<Ball>().end_point == 2) && wasPressedServe)

                {
                    hitting = false;
                    wasPressedServe = false;
                    GetComponent<BoxCollider>().enabled = true;
                    ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
                    Vector3 dir = aimTarget.position - transform.position;
                    ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
                    animator.Play("serve");
                    ball.GetComponent<Ball>().end_point = (ball.GetComponent<Ball>().end_point + 1) % 8;

                    
                    ball.GetComponent<Ball>().hitter = "player";
                    ball.GetComponent<Ball>().playing = true;
                }
                    break;
                
                

            case 1:
                if (ball.GetComponent<Ball>().end_point == 0 || ball.GetComponent<Ball>().end_point == 2)
                {
                    wasPressedServe = true;
                    if (ball.GetComponent<Ball>().end_point == 0 || ball.GetComponent<Ball>().end_point == 2)
                    {
                        hitting = true;
                        currentShot = shotManager.kickServe;
                        GetComponent<BoxCollider>().enabled = false;
                        animator.Play("serve-prepare");

                       
                    }
                }
                    break;
                

            case 2:
                if (ball.GetComponent<Ball>().end_point == 0 || ball.GetComponent<Ball>().end_point == 2)
                {
                    wasPressedServe = true;
                    if ((ball.GetComponent<Ball>().end_point == 0 || ball.GetComponent<Ball>().end_point == 2))
                    {

                        hitting = true;
                        currentShot = shotManager.flatServe;
                        GetComponent<BoxCollider>().enabled = false;
                        animator.Play("serve-prepare");

                        
                    }
                }
                break;
        }
       
       
        if (hitting)
        {
            if ( aim_target_1_left.transform.position.z > aimTarget.position.z)
            {
                aimTarget.Translate(new Vector3(0, 0, -Math.Abs(h)) * speed * Time.deltaTime);


            }


            else if (aim_target_1_right.transform.position.z < aimTarget.position.z)
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




            if (ball.GetComponent<Ball>().end_point % 2 == 0)
            {
                v = 0;

            }

            if (ball.GetComponent<Ball>().end_point == 0 && limit_player1.transform.position.x < transform.position.x)
            {
                transform.Translate(new Vector3(-v, 0, -Math.Abs(h)) * speed * Time.deltaTime);
                ball.GetComponent<Ball>().transform.Translate(new Vector3(-v, 0, -Math.Abs(h)) * speed * Time.deltaTime);


            }

            else if (ball.GetComponent<Ball>().end_point == 2 && limit_player1.transform.position.x > transform.position.x)
            {
                transform.Translate(new Vector3(-v, 0, Math.Abs(h)) * speed * Time.deltaTime);
                ball.GetComponent<Ball>().transform.Translate(new Vector3(-v, 0, Math.Abs(h)) * speed * Time.deltaTime);


            }
            else
            {
                transform.Translate(new Vector3(-v, 0, h) * speed * Time.deltaTime);
                if (ball.GetComponent<Ball>().end_point == 0 || ball.GetComponent<Ball>().end_point == 2)
                {
                    ball.GetComponent<Ball>().transform.Translate(new Vector3(-v, 0, h) * speed * Time.deltaTime);
                }
            }


        }




    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

       

        if (Input.GetKey(KeyCode.D))
        {
            actions[3] = 1; // Right
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actions[3] = 2; // Left
        }

        if (Input.GetKey(KeyCode.W))
        {
            actions[4] = 1; // Forward
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actions[4] = 2; // Backward
        }

        if (Input.GetKey(KeyCode.M))
        {
            actions[0] = 1;
        }
        else
        {
            actions[0] = 0;
        }

        if (Input.GetKey(KeyCode.N))
        {
            actions[1] = 1;
        }
        else
        {
            actions[1] = 0;
        }
        if (Input.GetKey(KeyCode.B))
        {
            actions[2] = 1;
        }
        else if (Input.GetKey(KeyCode.V))
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
        if(other.CompareTag("Ball") && ball.GetComponent<Ball>().end_point%2==1)
        {
            AddReward(0.2f);
            Vector3 dir = aimTarget.position - transform.position;
            
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitforce + new Vector3(0, currentShot.upforce, 0);
            animator.Play("backend");
            Vector3 ballDir=ball.position-transform.position;
            if (ballDir.x >= 0)
                animator.Play("forehand");
            else
                animator.Play("backend");

            ball.GetComponent<Ball>().hitter = "player";

        }
    }

    


    public void Reset()
    {

        Debug.Log("servedRight" + servedRight+ gameObject.transform.parent.name);
        

        

        if (servedRight)
        {
            transform.position = serveRight.position;
            
            



        }
        else
        {
            transform.position = serveLeft.position;
            
        }

        if(ball.GetComponent<Ball>().end_point==0)
        {
            ball.position = serveLeft.position;
        }

        if (ball.GetComponent<Ball>().end_point==2)
        {
            ball.position = serveRight.position;
        }





        servedRight = !servedRight;
        
        
        
        
    }
    */
}
