using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedMove : MonoBehaviour
{
    [Header("Turn Settings")]
    public float moveSpeed = 5f;
    public Transform movePoint;
    public float moveDistance;
    public int numTurns;
    public int turnsInputed;
    public float timeBettweenWalk;
    public LayerMask cannotWalk;
    public KeyCode confirmMoves = KeyCode.Space;
    public KeyCode removeTurnKey = KeyCode.Backspace;

    private bool waitingForQueue = true;
    public  bool stopPlayerMovement = false;
    public bool stopPlayerInput = false;
    public Queue<Vector3> movementQueue = new Queue<Vector3>();

    [Header("References")]
    public BatteryManager batteryManager;
    public MovementUI movementUI;


    private void Start()
    {
        movePoint.parent = null;
        if (batteryManager == null)
            batteryManager = GetComponent<BatteryManager>();
    }

    private void Update()
    {
        if (stopPlayerInput) return;
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (movementQueue.Count < numTurns && waitingForQueue)
        {
            movementUI.UpdateStatus(true);
            if (Input.GetKeyDown(KeyCode.W))
            {
                movementQueue.Enqueue(Vector2.up * moveDistance);
                turnsInputed++;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                movementQueue.Enqueue(Vector2.down * moveDistance);
                turnsInputed++;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                movementQueue.Enqueue(Vector2.left * moveDistance);
                turnsInputed++;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                movementQueue.Enqueue(Vector2.right * moveDistance);
                turnsInputed++;
            }
            movementUI.UpdateQueueUI();
        }

        if (Input.GetKeyUp(confirmMoves) && movementQueue.Count > 0)
        {
            waitingForQueue = false;
            movementUI.UpdateStatus(false);
            Invoke(nameof(MovePlayer), timeBettweenWalk);
            batteryManager.DecreaseBattery();
        }

        if (Input.GetKeyUp(removeTurnKey) && waitingForQueue && movementQueue.Count > 0)
        {
            RemoveFirstTurn();
            movementUI.UpdateQueueUI();
        }
    }


    private void MovePlayer()
    {
        if (movementQueue.Count <= 0 || stopPlayerMovement) return;
        Vector3 moveDir = movementQueue.Dequeue().normalized;
        Invoke(nameof(RoundPosition), 0.2f);


        if (!Physics2D.OverlapCircle(movePoint.position + moveDir, 0.2f, cannotWalk))
        {
            movePoint.position = transform.position + moveDir;
        }


        turnsInputed--;
        movementUI.UpdateQueueUI();
        if (movementQueue.Count <= 0) waitingForQueue = true;
        else Invoke(nameof(MovePlayer), timeBettweenWalk);
    }

    private void RoundPosition()
    {
        movePoint.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    private void RemoveFirstTurn()
    {
        movementQueue.Dequeue();
        turnsInputed--;
        Debug.Log("Dequeued first turn");
    }

    public void AskToQueue(Vector2 vector2)
    {
        movementQueue.Enqueue(vector2);
        MovePlayer();
    }

    public void DequeueCompeltly()
    {
        movementUI.UpdateStatus(false);
        movePoint.position = transform.position;
        if (movementQueue.Count > 0)
            for (int i = 0; i < movementQueue.Count; i++)
                movementQueue.Dequeue();
        
        stopPlayerMovement = false;
    }

    public int GetQueueCount()
    {
        return movementQueue.Count;
    }

    public int GetNumInputs()
    {
        return turnsInputed;
    }

    public bool GetWaitForQueue()
    {
        return waitingForQueue;
    }

    public void SetStopPlayer(bool thisBool)
    {
        movementUI.UpdateStatus(false);
        stopPlayerMovement = thisBool;
    }

    public void SetStopPlayerInput(bool thisBool)
    {
        movementUI.UpdateStatus(false);
        stopPlayerInput = thisBool;
    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object that collided with me: " + other.gameObject.name);

        Vector2 collisionDirection = (other.transform.position - transform.position).normalized;

        //Use collisionDirection to determine the direction of the trigger event.
        if (Vector2.Dot(collisionDirection, Vector2.up) == collisionDirection.magnitude * Vector3.up.magnitude)
        {
            Debug.Log("Blocked from above");
            blockedAbove = true;
        }
        else if (Vector2.Dot(collisionDirection, Vector2.down) == collisionDirection.magnitude * Vector3.down.magnitude)
        {
            Debug.Log("Blocked from below");
            blockedBelow = true;
        }
        else if (Vector2.Dot(collisionDirection, Vector2.right) == collisionDirection.magnitude * Vector3.right.magnitude)
        {
            Debug.Log("Blocked from the right");
            blockedRight = true;
        }
        else if (Vector2.Dot(collisionDirection, Vector2.left) == collisionDirection.magnitude * Vector3.left.magnitude)
        {
            Debug.Log("Blocked from the left");
            blockedLeft = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (blockedBelow) blockedBelow = false;
        if (blockedAbove) blockedAbove = false;
        if (blockedLeft) blockedLeft = false;
        if (blockedRight) blockedRight = false;
    }
    */


    /*
     * 
    private bool blockedAbove;
    private bool blockedBelow;
    private bool blockedRight;
    private bool blockedLeft;
     * 
    bool upDir = Vector3.Dot(moveDir, Vector3.up) == moveDir.magnitude * Vector3.up.magnitude;
    bool downDir = Vector3.Dot(moveDir, Vector3.down) == moveDir.magnitude * Vector3.down.magnitude;
    bool leftDir = Vector3.Dot(moveDir, Vector3.left) == moveDir.magnitude * Vector3.left.magnitude;
    bool rightDir = Vector3.Dot(moveDir, Vector3.right) == moveDir.magnitude * Vector3.right.magnitude;

    if (upDir && !blockedAbove)
    {
        Debug.Log("Moving Player Up");
        transform.Translate(moveDir);
    }
    else if (downDir && !blockedBelow)
    {
        Debug.Log("Moving Player Down");
        transform.Translate(moveDir);
    }
    else if (leftDir && !blockedLeft)
    {
        Debug.Log("Moving Player Left");
        transform.Translate(moveDir);
    }
    else if (rightDir && !blockedRight)
    {
        Debug.Log("Moving Player Right");
        transform.Translate(moveDir);
    }
    */
}