﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private BoxCollider2D checkUp;
    private BoxCollider2D checkDown;
    private BoxCollider2D checkLeft;
    private BoxCollider2D checkRight;

    private Health health;

    private string[] directions = {"Up", "Down", "Left", "Right"};
    private bool[] eligibleDirections = { true, true, true, true };
    private Vector2[] directionVectors = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private bool disabled = false;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        checkUp = GameObject.Find("CheckUp").GetComponent<BoxCollider2D>();
        checkDown = GameObject.Find("CheckDown").GetComponent<BoxCollider2D>();
        checkLeft = GameObject.Find("CheckLeft").GetComponent<BoxCollider2D>();
        checkRight = GameObject.Find("CheckRight").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled)
        {
            GetMove();
        }
    }

    public void SetDisabled(bool tOrF)
    {
        disabled = tOrF;
    }

    private void GetMove()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            if (Input.GetButtonDown(directions[i]))
            {
                if (eligibleDirections[i])
                {
                    transform.position = new Vector3(transform.position.x + directionVectors[i].x, transform.position.y + directionVectors[i].y);
                }
                else
                {
                    HitWall();
                }
            }
        }
    }

    private void HitWall()
    {
        Debug.LogWarning("You hit a wall!"); // TODO give player feedback besides just losing a heart
        health.LoseHealth();
    }

    public void TriggerDetected(Checker checker, Collider2D collision)
    {
        if (collision.CompareTag("Walls"))
        {
            switch (checker.name)
            {
                case "CheckUp":
                    eligibleDirections[0] = false;
                    break;
                case "CheckDown":
                    eligibleDirections[1] = false;
                    break;
                case "CheckLeft":
                    eligibleDirections[2] = false;
                    break;
                case "CheckRight":
                    eligibleDirections[3] = false;
                    break;
                default:
                    Debug.LogError("Invalid Checker Collision Detection");
                    break;
            }
        }
    }

    public void TriggerReset(Checker checker)
    {
        switch (checker.name)
        {
            case "CheckUp":
                eligibleDirections[0] = true;
                break;
            case "CheckDown":
                eligibleDirections[1] = true;
                break;
            case "CheckLeft":
                eligibleDirections[2] = true;
                break;
            case "CheckRight":
                eligibleDirections[3] = true;
                break;
            default:
                Debug.LogError("Invalid Checker Collision Reset");
                break;
        }
    }
}