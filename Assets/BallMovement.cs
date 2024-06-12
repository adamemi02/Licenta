using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;





public class BallMovementCheck : MonoBehaviour
{
    public float thresholdTime = 5f; // Time in seconds
    private float stationaryTime = 0f;
    private Rigidbody rb;
    private bool isStationary = false;
  [SerializeField]  public Ball ball;
    [SerializeField] public PlayerAgent player_1;
    [SerializeField] public PlayerAgent player_2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude < 0.01f) // Threshold to consider the ball as stationary
        {
            stationaryTime += Time.deltaTime;
            if (stationaryTime >= thresholdTime && !isStationary)
            {
                DoSomething();
                isStationary = true; // Ensure the action is only performed once
            }
        }
        else
        {
            stationaryTime = 0f; // Reset the timer if the ball starts moving
            isStationary = false; // Reset the stationary flag
        }
    }

    void DoSomething()
    {
        // Implement your logic here for what should happen if the ball is stationary for more than 5 seconds
        
        if(ball.end_point%2==0)
        {
            if(ball.end_point<=2)
            {
                player_1.AddReward(-1f);
                ball.player2Score++;
                ball.end_point = (ball.end_point + 2) % 8;
               
                
                

            }
            else
            {
                player_2.AddReward(-1f);
                ball.end_point=(ball.end_point+2)%8;
                ball.playerScore++;
            }
           
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ball.transform.position = ball.initialPos;
            player_2.Reset();
            player_1.Reset();
            ball.updateScores();
        }
        else
        {
            if (ball.end_point <= 3)
            {
                player_1.AddReward(-0.1f);
                ball.player2Score++;
                ball.end_point = (ball.end_point + 1) % 8;

            }
            else
            {
                player_2.AddReward(-0.1f);
                ball.end_point = (ball.end_point + 1) % 8;
                ball.playerScore++;

            }
         
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ball.transform.position = ball.initialPos;
            player_2.Reset();
            player_1.Reset();
            ball.updateScores();
        }
        
        // You can add more actions here, like triggering an event, playing a sound, etc.
    }
}