using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public Transform[] puzzles;
    public bool openDoors;

    public GameEvents gameEvents;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        for (int i = 0; i < puzzles.Length; i++)
            if (!puzzles[i].gameObject.activeInHierarchy) return;

        openDoors = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!openDoors) return;
        if (collision.gameObject.name.CompareTo("Player") != 0) return;

        Debug.Log("Winner Winner chicken Dinner");
        gameEvents.Winner();
    }
}
