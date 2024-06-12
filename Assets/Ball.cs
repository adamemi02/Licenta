using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public Vector3 initialPos;
    public string hitter;
    public int index;


    bool isOnCourt;
    float timeOnCourt;// Reset the timer when the ball enters the court



    public int playerScore;
    public int player2Score;

    public bool episodeEnded = false;

    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text player2ScoreText;
    Rigidbody ballRigidbody;
    int nr_bounce = 0;

    public bool playing = true;

    public int end_point = 0;

    [SerializeField] private PlayerAgent player_1;

    [SerializeField] private PlayerAgent player_2;


    void Start()
    {
        initialPos = transform.position;
        playerScore = 0;
        player2Score = 0;
        end_point = 0;

        ballRigidbody = GetComponent<Rigidbody>();

    }








    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {






        if (end_point == 1 && collision.transform.CompareTag("player_1_stanga") && player_1.served == true)
        {
            Debug.Log("serva_proasta");

            player2Score++;
            player_1.AddReward(-0.5f);

            resetare();

        }

        if (end_point == 3 && collision.transform.CompareTag("player_1_dreapta") && player_1.served == true)
        {

            Debug.Log("serva_proasta");
            player2Score++;
            player_1.AddReward(-0.5f);

            resetare();

        }

        if (end_point == 5 && collision.transform.CompareTag("player_2_stanga") && player_2.served == true)
        {
            Debug.Log("serva_proasta");

            playerScore++;

            player_2.AddReward(-0.5f);

            resetare();
        }
        if (end_point == 7 && collision.transform.CompareTag("player_2_dreapta") && player_2.served == true)
        {
            Debug.Log("serva_proasta");

            playerScore++;

            player_2.AddReward(-0.5f);

            resetare();
        }

        if (end_point == 1 && collision.transform.CompareTag("player_1_dreapta") && player_1.served == true)
        {

            Debug.Log("serva_buna");

            player_1.AddReward(0.5f);



        }

        if (end_point == 3 && collision.transform.CompareTag("player_1_stanga") && player_1.served == true)
        {

            Debug.Log("serva_buna");
            player_1.AddReward(0.5f);



        }

        if (end_point == 5 && collision.transform.CompareTag("player_2_dreapta") && player_2.served == true)
        {

            Debug.Log("serva_buna");
            player_2.AddReward(0.5f);


        }
        if (end_point == 7 && collision.transform.CompareTag("player_2_stanga") && player_2.served == true)
        {

            Debug.Log("serva_buna");
            player_2.AddReward(0.5f);


        }






        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Net"))
        {






            if (collision.transform.CompareTag("Wall") && playing && end_point % 2 != 0)

            {






                if (hitter == "player")
                {
                    playerScore++;

                }
                else
                {

                    player2Score++;
                }
                playing = false;
                resetare();


            }

            if (collision.transform.CompareTag("Net") && playing && end_point % 2 != 0)
            {



                if (hitter == "player")
                {
                    player2Score++;
                    player_1.AddReward(-1f);
                    player_2.AddReward(1f);
                }
                else
                {
                    playerScore++;
                    player_2.AddReward(-1f);
                    player_1.AddReward(1f);
                }
                playing = false;
                resetare();


            }





        }









    }

    private void OnTriggerEnter(Collider other)
    {




        if (other.CompareTag("Out") && playing && end_point % 2 != 0)
        {




            if (hitter == "player")
            {
                player2Score++;
                player_1.AddReward(-1f);
                player_2.AddReward(1f);

            }
            else
            {

                playerScore++;
                player_2.AddReward(-1f);
                player_1.AddReward(1f);

            }
            playing = false;
            resetare();
        }

    }




    public void updateScores()
    {
        playerScoreText.text = "Player : " + playerScore;
        player2ScoreText.text = "Player2 : " + player2Score;
        if (playerScore >= 7 || player2Score >= 7)
        {
            playerScore = 0;
            player2Score = 0;
            player_1.endOfEpisode();
            player_2.endOfEpisode();
        }
    }




    public void resetare()
    {

        Debug.Log(index);


        if (index >= 11)
        {
            player_1.AddReward(0.5f);
            player_2.AddReward(0.5f);
        }
        end_point = (end_point + 1) % 8;


        nr_bounce = 0;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        transform.position = initialPos;
        player_2.Reset();
        player_1.Reset();
        updateScores();
    }


}
