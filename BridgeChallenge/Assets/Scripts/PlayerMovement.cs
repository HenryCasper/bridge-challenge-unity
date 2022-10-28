using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 1f;
    [SerializeField]
    float snapDistance = 0.25f;

    //Important move related vectors
    Vector3 targetPosition;
    Vector3 currentMoveDirection;
    Vector3 originalPosition;

    //Information for gameplay mechanics
    float max;
    bool moving;

    void Start()
    {
        max = FindObjectOfType<GameManager>().gridmax;
    }

    void Update()
    {
        if (!moving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                CreateMovementVectors(Vector3.forward);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                CreateMovementVectors(Vector3.left);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                CreateMovementVectors(Vector3.back);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                CreateMovementVectors(Vector3.right);
            }
        }

        else DealWithMove();
    }

    private void FixedUpdate()
    {
        CheckForStuck();
    }

    //Lose state checker method.
    private void CheckForStuck()
    {
        Ray frontray = new Ray(transform.position, Vector3.forward);
        Ray rightray = new Ray(transform.position, Vector3.right);
        Ray leftray = new Ray(transform.position, Vector3.left);
        Ray backray = new Ray(transform.position, Vector3.back);
        RaycastHit fronthit, righthit, lefthit, backhit;
        if(Physics.Raycast(frontray,out fronthit,0.5f) && Physics.Raycast(rightray, out righthit, 0.5f)
            && Physics.Raycast(leftray, out lefthit, 0.5f) && Physics.Raycast(backray, out backhit, 0.5f))
        {
            if(fronthit.collider.name.Contains("Cube") && righthit.collider.name.Contains("Cube") &&
                lefthit.collider.name.Contains("Cube") && backhit.collider.name.Contains("Cube"))
            {
                FindObjectOfType<GameManager>().EndGame();
            }
        }
    }

    //Creation of movement vectors to deal with move
    private void CreateMovementVectors(Vector3 dir)
    {
        targetPosition = transform.position + dir;
        currentMoveDirection = dir;
        originalPosition = transform.position;
        moving = true;
    }

    //Method to check if move is possible and if possible, to deal with a gradual move so the visual of the movement is better.
    private void DealWithMove()
    {
        //If movement leads to a cube ------------------------
        Ray rayfromcylinder = new Ray(transform.position, currentMoveDirection);
        RaycastHit hit;
        Debug.DrawRay(transform.position,currentMoveDirection, Color.red);
        if(Physics.Raycast(rayfromcylinder,out hit, 0.5f))
        {
            if (hit.collider.name.Contains("Cube"))
            {
                moving = false;
                transform.position = originalPosition;
            }
        }
        //If movement leads to out of bounds ----------------------
        if(targetPosition.x > max || targetPosition.x < -max || targetPosition.z > max || targetPosition.z < -max)
        {
            moving = false;
            transform.position = originalPosition;
        }

        //If after both checkers before the move can still be done, deal with it
        if(moving)
        {
            if (Vector3.Distance(transform.position, targetPosition) > snapDistance)
            {
                transform.position += currentMoveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = targetPosition;
                moving = false;
            }
        }

    }
}
