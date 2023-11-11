using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementUI : MonoBehaviour
{
    [Header("References")]
    public TurnBasedMove playerScript;
    public Image status;
    public Image[] arrowSprites;

    [Header("Status")]
    public Color32 readyToMove;
    public Color32 cannotMove;

    [Header("Arrow References")]
    public Sprite emptyArrow;
    public Sprite arrowUp;
    public Sprite arrowDown;
    public Sprite arrowLeft;
    public Sprite arrowRight;

    void Start()
    {
        UpdateQueueUI();
        UpdateStatus(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateQueueUI()
    {
        foreach (Image arrow in arrowSprites)
            arrow.sprite = emptyArrow;

        int i = 0;
        foreach (Vector3 command in playerScript.movementQueue)
        {
            if (Vector3.Angle(command, Vector3.up) == 0)
                arrowSprites[i].sprite = arrowUp;
            else if (Vector3.Angle(command, Vector3.down) == 0)
                arrowSprites[i].sprite = arrowDown;
            else if (Vector3.Angle(command, Vector3.left) == 0)
                arrowSprites[i].sprite = arrowLeft;
            else if (Vector3.Angle(command, Vector3.right) == 0)
                arrowSprites[i].sprite = arrowRight;
            else 
                arrowSprites[i].sprite = emptyArrow;

            i++;
        }
    }

    public void UpdateStatus(bool canMove)
    {
        if(canMove)
            status.GetComponent<Image>().color = readyToMove;
        else
            status.GetComponent<Image>().color = cannotMove;
        
    }
}
