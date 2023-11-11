using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractor : MonoBehaviour
{
    [Header("References")]
    public TurnBasedMove playerMovement;
    public GameObject puzzle;
    public EnemyAI anyEnemyAI;

    [Header("Interaction Settings")]
    public float interactRadius = 1;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask whatIsPuzzle;
    public bool interacting = false;

    private Collider2D[] hitColliders;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, interactRadius, whatIsPuzzle) && !interacting && Input.GetKeyUp(interactKey))
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, interactRadius, whatIsPuzzle);
            GetPuzzle();

            LoadPuzzle();
        }
        else if(Physics2D.OverlapCircle(transform.position, interactRadius, whatIsPuzzle) && Input.GetKeyUp(interactKey) && interacting)
        {
            //LeavePuzzle();
        } 
    }

    private void GetPuzzle()
    {
        puzzle = hitColliders[0].transform.Find("Puzzle").gameObject;
    }

    private void LoadPuzzle()
    {
        interacting = true;
        Debug.Log("Load Puzzle");
        puzzle.SetActive(true);
        playerMovement.SetStopPlayerInput(true);
        anyEnemyAI.SetFreeTurn(true);
        Invoke(nameof(ResetFreeTurn), 0.2f);

        Invoke(nameof(LeavePuzzle), 0.5f);
    }

    private void LeavePuzzle()
    {
        interacting = false;
        Debug.Log("Leave Puzzle");
        //puzzle.SetActive(false);
        playerMovement.SetStopPlayerInput(false);
    }

    private void ResetFreeTurn()
    {
        anyEnemyAI.SetFreeTurn(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);  
    }
}
