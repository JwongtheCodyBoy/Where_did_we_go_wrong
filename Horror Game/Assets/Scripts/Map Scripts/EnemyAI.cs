using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    NavMeshAgent agent;
    public TurnBasedMove playerScript;
    public Transform player;
    public LayerMask playerLayer;

    [Header("Turn Settings")]
    public float moveSpeed = 5f;
    public Transform movePoint;
    public float timeBettweenWalk;
    public LayerMask cannotWalk;
    public KeyCode confirmMoves = KeyCode.Space;
    public int lowestMoves;
    public int highestMoves;

    private Vector3 walkpoint;
    private Queue<Vector3> movementQueue = new Queue<Vector3>();
    private int currentMove;
    private bool canAttack = true;
    private static bool freeTurn = false;

    private void Start()
    {
        movePoint = transform.Find("Enemy Move Point").transform;
        movePoint.parent = null;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        //Debug.Log(agent.desiredVelocity.normalized);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        CollisionDetection();
        if (!canAttack && movementQueue.Count >0)
            movementQueue.Dequeue();
        
        if ((Input.GetKeyUp(confirmMoves) && !playerScript.GetWaitForQueue()) || freeTurn)
        {
            EnemyNumTurns();
            //agent.SetDestination(player.position);
            NextStep();
            Invoke(nameof(MoveToPlayer), timeBettweenWalk);
            Debug.Log("Enemy Trying to go to player");
        }
    }


    private void MoveToPlayer()
    {
        if (movementQueue.Count <= 0) return;
        currentMove--;
        Vector3 moveDir = movementQueue.Dequeue().normalized;
        Invoke(nameof(RoundPosition), 0.2f);

        if (!Physics2D.OverlapCircle(movePoint.position + moveDir, 0.2f, cannotWalk))
        {
            movePoint.position = transform.position + moveDir;
        }
        Debug.Log("Enemy Moves Left: "+ currentMove);
        if (currentMove > 0)
        {
            NextStep();
            Invoke(nameof(MoveToPlayer), timeBettweenWalk);
        }
    }

    private void EnemyNumTurns()
    {
        if(playerScript.GetNumInputs() > 2) currentMove = highestMoves;
        else currentMove = lowestMoves;

        Invoke(nameof(ResetAttack), 0.2f);
    }

    private void NextStep()
    {
        agent.SetDestination(player.position);
        if (agent.desiredVelocity.x != 0 || agent.desiredVelocity.y != 0)
        {
            if(Mathf.Abs(agent.desiredVelocity.x) >= Mathf.Abs(agent.desiredVelocity.y))
            {
                if (agent.desiredVelocity.x < 0)
                    movementQueue.Enqueue(Vector2.left);
                else
                    movementQueue.Enqueue(Vector2.right);
            }
            else
            {
                if (agent.desiredVelocity.y < 0)
                    movementQueue.Enqueue(Vector2.down);
                else
                    movementQueue.Enqueue(Vector2.up);
            }
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.CompareTo("Player") == 0)
        {
            //collision.GetComponent<TurnBasedMove>().SetStopPlayer(true);
            collision.GetComponent<TurnBasedMove>().DequeueCompeltly();
            collision.GetComponent<PlayerHealth>().LoseHealth();
            currentMove = 0;
            for (int i = 0; i < movementQueue.Count; i++)
                movementQueue.Dequeue();

            if (Physics2D.Raycast(transform.position, Vector2.up, 1.2f, playerLayer))
            {
                Debug.Log("Hit Player from below");
                collision.GetComponent<TurnBasedMove>().AskToQueue(Vector2.up);
            }
            else if (Physics2D.Raycast(transform.position, Vector2.down, 1.2f, playerLayer))
            {
                Debug.Log("Hit Player from above");
                collision.GetComponent<TurnBasedMove>().AskToQueue(Vector2.down);
            }
            else if (Physics2D.Raycast(transform.position, Vector2.left, 1.2f, playerLayer))
            {
                Debug.Log("Hit Player from right");
                collision.GetComponent<TurnBasedMove>().AskToQueue(Vector2.left);
            }
            else if (Physics2D.Raycast(transform.position, Vector2.right, 1.2f, playerLayer))
            {
                Debug.Log("Hit Player from left");
                collision.GetComponent<TurnBasedMove>().AskToQueue(Vector2.right);
            }
            else Debug.Log("Unity is scuffed");
        }
    }
    */
    private void CollisionDetection()
    {
        //if (!canAttack) return;
        if (Physics2D.Raycast(transform.position, Vector2.up, 0.6f, playerLayer))
        {
            Debug.Log("Hit Player from below");
            DoingTheRest();
            if (player.GetComponent<TurnBasedMove>().GetQueueCount() == 0)
                player.GetComponent<TurnBasedMove>().AskToQueue(Vector2.up);
            canAttack = false;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.down, 0.6f, playerLayer))
        {
            Debug.Log("Hit Player from above");
            DoingTheRest();
            if (player.GetComponent<TurnBasedMove>().GetQueueCount() == 0)
                player.GetComponent<TurnBasedMove>().AskToQueue(Vector2.down);
            canAttack = false;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.left, 0.6f, playerLayer))
        {
            Debug.Log("Hit Player from right");
            DoingTheRest(); if (player.GetComponent<TurnBasedMove>().GetQueueCount() == 0)
                player.GetComponent<TurnBasedMove>().AskToQueue(Vector2.left);
            canAttack = false;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.right, 0.6f, playerLayer))
        {
            Debug.Log("Hit Player from left");
            DoingTheRest(); if (player.GetComponent<TurnBasedMove>().GetQueueCount() == 0)
                player.GetComponent<TurnBasedMove>().AskToQueue(Vector2.right);
            canAttack = false;
        }
        //else Debug.Log("Unity is scuffed");
    }

    private void DoingTheRest() 
    {
        if (!canAttack) return;
        player.GetComponent<TurnBasedMove>().DequeueCompeltly();
        player.GetComponent<PlayerHealth>().LoseHealth();
        for (int i = 0; i < movementQueue.Count; i++)
            movementQueue.Dequeue();
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private void RoundPosition()
    {
        movePoint.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    public void SetFreeTurn(bool thisBool)
    {
        freeTurn = thisBool;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y +0.6f, transform.position.z));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + 0.6f, transform.position.y, transform.position.z));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - 0.6f, transform.position.y, transform.position.z));
    }
}