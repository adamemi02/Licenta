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
using UnityEngine.SocialPlatforms;

public class AimTarget : MonoBehaviour
{
    private void Start()
    {

    }

    public Ball ball;
    private void OnCollisionEnter(Collision other)
    {
        if (ball.end_point == 0)

        {

            Debug.Log(other.transform.CompareTag("player_1_stanga"));

        }

        if (ball.end_point == 2)
        {
            Debug.Log(other.transform.CompareTag("player_1_dreapta"));
        }
        if (ball.end_point == 4)
        {
            Debug.Log(other.transform.CompareTag("player_2_stanga"));
        }
        if (ball.end_point == 6)
        {
            Debug.Log(other.transform.CompareTag("player_2_dreapta"));
        }

    }


}